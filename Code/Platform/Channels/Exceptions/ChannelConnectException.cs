using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Interfaces;

namespace Inbox2.Platform.Channels.Exceptions
{
	public class ChannelConnectException : ChannelException
	{
		private readonly ConnectResult result;

		private readonly string authMessage;

		public ConnectResult Result
		{
			get { return result; }
		}

		public string AuthMessage
		{
			get { return authMessage; }
		}

		public ChannelConnectException(ConnectResult result, string authMessage) : base(string.Empty)
		{
			this.result = result;
			this.authMessage = authMessage;
		}
	}
}
