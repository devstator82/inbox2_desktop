using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Inbox2.Platform.Channels.Connections;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Exceptions;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Logging;
using LumiSoft.Net.POP3.Client;
using Logger=Inbox2.Platform.Logging.Logger;

namespace Inbox2.Channels.Pop3
{
	public class Pop3Connection : ChannelConnectionBase
	{
		#region Fields

		private POP3_Client client;

		#endregion

		#region Properties

		internal POP3_Client Client
		{
			get { return client; }
		}
		
		#endregion

		#region Constructors

		public Pop3Connection(string hostname, int port, bool isSecured, string username, string password)
			: base(hostname, port, isSecured, username, password)
		{
		}

		#endregion

		public override void Open()
		{
			if (!(channelState == ChannelState.Closed || channelState == ChannelState.Broken))
			{
				return;
			}

			channelState = ChannelState.Connecting;

			try
			{
				client = new POP3_Client();

				if ("/Settings/Channels/LoggerEnabled".AsKey(false))
				{
					client.Logger = new LumiSoft.Net.Log.Logger();
					client.Logger.WriteLog += (sender, e) => Logger.Debug(e.LogEntry.Text, LogSource.Channel);
				}

				client.Connect(Hostname, Port, IsSecured);				

				channelState = ChannelState.Connected;
			}
			catch (Exception ex)
			{
				channelState = ChannelState.Closed;

				throw new ChannelException("Unable to connect to server", ex);
			}
		}

		public override void Close()
		{
			if (client != null && client.IsConnected)
			{
				client.Disconnect();
				client.Dispose();
			}

			channelState = ChannelState.Closed;
		}

		public override void Authenticate()
		{
			if (channelState == ChannelState.Authenticated)
				return;

			if (channelState != ChannelState.Connected)
				throw new ChannelException("Unable to authenticate in current channel state");

			try
			{
				client.Authenticate(Username, Password, true);

				channelState = ChannelState.Authenticated;
			}
			catch (POP3_ClientException ex)
			{
				channelState = ChannelState.Broken;

				Logger.Debug("FAILED Authenticating connection {0}. Server said {1}", LogSource.Channel, UniqueId, ex.ResponseText);

				throw new ChannelAuthenticationException(ex.ResponseText, ex);
			}			
		}

		public override void Dispose()
		{
		}
	}
}
