using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;

namespace Inbox2.Platform.Channels.Connections
{
	public abstract class ChannelConnectionBase : IChannelConnection
	{
		[ThreadStatic]
		public static long UserId;

		private readonly Guid uniqueId = Guid.NewGuid();
		private readonly string hostname;
		private readonly int port;
		private readonly bool isSecured;
		private readonly string username;
		private readonly string password;
		protected ChannelState channelState = ChannelState.Closed;

		public Guid UniqueId
		{
			get { return uniqueId; }
		}

		public bool IsConnected
		{
			get
			{
				return !(ChannelState == ChannelState.Broken || ChannelState == ChannelState.Closed);
			}
		}

		public ChannelState ChannelState
		{
			get { return channelState; }
		}

		public string Hostname
		{
			get { return hostname; }
		}

		public int Port
		{
			get { return port; }
		}

		public bool IsSecured
		{
			get { return isSecured; }
		}

		public string Username
		{
			get { return username; }
		}

		public string Password
		{
			get { return password; }
		}

		public ChannelProgressDelegate BytesRead { get; set; }
		public ChannelDataDelegate DataRead { get; set; }
		public ChannelProgressDelegate BytesWritten { get; set; }
		public ChannelDataDelegate DataWritten { get; set; }

		public bool IsLocked { get; set; }

		public ChannelConnectionBase(string hostname, int port, bool isSecured, string username, string password)
		{
			this.hostname = hostname;
			this.port = port;
			this.isSecured = isSecured;
			this.username = username;
			this.password = password;
		}

		public abstract void Open();

		public abstract void Close();

		public abstract void Authenticate();

		public abstract void Dispose();
	}
}
