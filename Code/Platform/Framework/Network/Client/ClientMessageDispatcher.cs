using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Inbox2.Platform.Logging;

namespace Inbox2.Platform.Framework.Network.Client
{
	public class ClientMessageDispatcher<R, T>
	{
		private readonly byte method;		
		private readonly R request;
		private readonly IPayloadSerializer<R, T> serializer;

		public ClientMessageDispatcher(byte method, R request, IPayloadSerializer<R, T> serializer)
		{
			this.method = method;
			this.request = request;
			this.serializer = serializer;
		}

		public T Send(string ip, int port, int timeout)
		{
			var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			var sc = new SocketConnection(client);

			try
			{
				client.Connect(new IPEndPoint(IPAddress.Parse(ip), port));

				if (sc.Socket.Connected)
				{
					// Send request to AppServer
					var data = PrepareRequestBuffer();

					sc.Socket.Send(data, 0, data.Length, SocketFlags.None);

					int bytesReceived = sc.Socket.Receive(sc.SocketBuffer, 0, sc.BufferSize, SocketFlags.None);

					while (bytesReceived > 0)
					{
						// There might be more data, so store the data received so far and check for eof
						if (sc.CopyBuffer(bytesReceived))
						{
							// All the data has arrived.
							break;
						}
						else
						{
							bytesReceived = sc.Socket.Receive(sc.SocketBuffer, 0, sc.BufferSize, SocketFlags.None);
						}
					}

					return BuildResponse(sc);
				}
				else
				{
					throw new ApplicationException("Socket was not connected");
				}
			}
			catch (Exception ex)
			{
				Logger.Error("An error has occured while trying to connect to the AppServer. Exception = {0}", LogSource.AppServer, ex);

				throw new ClientException("An error has occured", ex);
			}
			finally
			{
				try { sc.Close(); }
				catch { }

				try
				{
					if (sc.Socket.Connected)
						sc.Socket.Shutdown(SocketShutdown.Both);
				}
				catch { }
			}
		}

		T BuildResponse(SocketConnection sc)
		{
			byte[] responseBuffer = sc.GetDataBuffer();

			// Timeout occured
			if (responseBuffer.Length == 0)
				throw new ClientException("Timeout");

			// Create memorystream for deserialization purposes:
			// - skip the first 4 bytes (length (l) + control byte (cb))
			// - read up to boundaryMarker minus 7 (= l + cb + crlf)
			using (var ms = new MemoryStream(responseBuffer, 1, responseBuffer.Length - 1))
			using (var reader = new StreamReader(ms))
			{
				var controlChar = responseBuffer[0];

				switch (controlChar)
				{
					case WireFormat.SuccessControlChar:
						return serializer.Deserialize(ms);
					case WireFormat.ErrorControlChar:
						throw new ClientException(reader.ReadToEnd());
					default:
						throw new ClientException("Unknown control character detected");
				}
			}
		}

		/// <summary>
		/// Serializes the request to a byte buffer.
		/// </summary>
		/// <returns></returns>
		byte[] PrepareRequestBuffer()
		{
			using (var ms = new MemoryStream())
			{
				serializer.Serialize(ms, request);

				return WireFormat.GetBytes(method, ms);
			}
		}
	}
}
