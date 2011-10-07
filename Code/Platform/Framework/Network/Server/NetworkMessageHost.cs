using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Inbox2.Platform.Logging;

namespace Inbox2.Platform.Framework.Network.Server
{
	public class NetworkMessageHost
	{
		private readonly IMessageSelector selector;
		private readonly Socket listener;
		private readonly List<SocketConnection> connections;
		private readonly object synclock = new object();
		private bool shuttingDown;

		public NetworkMessageHost(IMessageSelector selector)
		{
			this.selector = selector;
			this.listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);			
			this.connections = new List<SocketConnection>();
		}

		public void Start(int port)
		{
			listener.Bind(new IPEndPoint(IPAddress.Any, port));
			listener.Listen(1000);
			listener.BeginAccept(new AsyncCallback(OnAccept), listener);			
		}

		public void Stop()
		{
			shuttingDown = true;
			listener.Close();
		}

		void OnAccept(IAsyncResult ar)
		{
			try
			{
				if (shuttingDown)
					return;

				Socket serverSocket = (Socket)ar.AsyncState;
				Socket clientSocket = serverSocket.EndAccept(ar);

				// Get ready for next incoming connection.
				serverSocket.BeginAccept(OnAccept, serverSocket);

				if (!clientSocket.Connected)
					return;

				var sc = new SocketConnection(clientSocket);

				lock (synclock)
					connections.Add(sc);

				try
				{
					sc.Socket.BeginReceive(sc.SocketBuffer, 0, sc.BufferSize, SocketFlags.None, OnReceive, sc);
				}
				catch (Exception ex)
				{
					Logger.Error("An error has occured while trying to receive data from the client. Exception = {0}", LogSource.AppServer, ex);
				}
			}
			catch (Exception ex)
			{
				Logger.Error("An error has occured while trying to accept a new connection from the client. Exception = {0}", LogSource.AppServer, ex);
			}			
		}

		void OnReceive(IAsyncResult ar)
		{
			var sc = (SocketConnection)ar.AsyncState;

			try
			{
				int bytesReceived = sc.Socket.EndReceive(ar);

				if (bytesReceived > 0)
				{
					// There  might be more data, so store the data received so far and check for eof
					if (sc.CopyBuffer(bytesReceived))
					{
						var replyBuffer = selector.Select(sc.GetDataBuffer());

						sc.Socket.BeginSend(replyBuffer, 0, replyBuffer.Length, SocketFlags.None, OnSend, sc);
					}
					else
					{
						// Not there, read more data
						sc.Socket.BeginReceive(sc.SocketBuffer, 0, sc.BufferSize, SocketFlags.None, OnReceive, sc);	
					}					
				}				
			}
			catch (Exception ex)
			{
				CloseConnection(sc);

				Logger.Error("An error has occured while trying to receive data from the client. Exception = {0}", LogSource.AppServer, ex);
			}
		}

		void OnSend(IAsyncResult ar)
		{
			var sc = (SocketConnection)ar.AsyncState;

			try
			{
				sc.Socket.EndSend(ar);

				CloseConnection(sc);
			}
			catch (Exception ex)
			{
				CloseConnection(sc);

				Logger.Error("An error has occured while trying to receive data from the client. Exception = {0}", LogSource.AppServer, ex);
			}			
		}

		void CloseConnection(SocketConnection sc)
		{
			lock (synclock)
				connections.Remove(sc);

			sc.Close();
		}
	}
}
