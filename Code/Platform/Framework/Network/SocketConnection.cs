using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Inbox2.Platform.Framework.Network.Server;
using Inbox2.Platform.Framework.Network.Utils;

namespace Inbox2.Platform.Framework.Network
{
	public class SocketConnection
	{
		private readonly Socket socket;
		private readonly byte[] socketBuffer;

		private byte[] dataBuffer;
		private int totalBytesRead;

		public int BufferSize
		{
			get
			{
				return 1024;
			}
		}

		public byte[] SocketBuffer
		{
			get { return socketBuffer; }
		}

		public Socket Socket
		{
			get { return socket; }
		}

		public SocketConnection(Socket socket)
		{
			this.socket = socket;
			this.socketBuffer = new byte[BufferSize];
			this.dataBuffer = new byte[4194304];
		}

		public bool CopyBuffer(int bytesRead)
		{
			var indx = BufferUtils.IndexOf(socketBuffer, WireFormat.Eof, bytesRead);

			if (indx > -1)
			{
				// Append bytes up to eof
				Buffer.BlockCopy(socketBuffer, 0, dataBuffer, totalBytesRead, indx);

				totalBytesRead += indx;
				return true;
			}
			else
			{
				// Append all bytes
				Buffer.BlockCopy(socketBuffer, 0, dataBuffer, totalBytesRead, bytesRead);

				totalBytesRead += bytesRead;
				return false;
			}
		}

		public byte[] GetDataBuffer()
		{
			var trimmedBuffer = new byte[totalBytesRead];

			Buffer.BlockCopy(dataBuffer, 0, trimmedBuffer, 0, totalBytesRead);

			return trimmedBuffer;
		}

		public void Close()
		{
			try
			{
				dataBuffer = null;

				socket.Close();
			}
			catch (Exception ex)
			{				
			}
		}
	}
}
