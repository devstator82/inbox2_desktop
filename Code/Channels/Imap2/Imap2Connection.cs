using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Connections;
using Inbox2.Platform.Channels.Exceptions;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Logging;
using LumiSoft.Net.IMAP.Client;
using Logger = Inbox2.Platform.Logging.Logger;

namespace Inbox2.Channels.Imap2
{
	public class Imap2Connection : ChannelConnectionBase
	{
		private IMAP_Client client;

		public IMAP_Client Client
		{
			get { return client; }
		}

		public Imap2Connection(string hostname, int port, bool isSecured, string username, string password) : base(hostname, port, isSecured, username, password)
		{
				
		}

		public override void Open()
		{
			Logger.Debug("Opening IMAP connection {0}", LogSource.Channel, UniqueId);

			if (!(channelState == ChannelState.Closed || channelState == ChannelState.Broken))
			{
				Logger.Debug("SUCCESS Connection was allready open {0}", LogSource.Channel, UniqueId);

				return;
			}

			channelState = ChannelState.Connecting;
			client = new IMAP_Client();

			if ("/Settings/Channels/LoggerEnabled".AsKey(false))
			{
				client.Logger = new LumiSoft.Net.Log.Logger();
				client.Logger.WriteLog += (sender, e) => Logger.Debug(e.LogEntry.Text.Replace("{", "{{").Replace("}", "}}"), LogSource.Channel);
			}

			try
			{
				client.Connect(Hostname, Port, IsSecured);

				channelState = ChannelState.Connected;
			}
			catch (Exception ex)
			{
				channelState = ChannelState.Closed;

				Logger.Debug("Unable to connect to server. Exception = {0}", LogSource.Channel, ex);

				throw new ChannelException("Unable to connect to server", ex);
			}

			Logger.Debug("SUCCESS Opening connection {0}", LogSource.Channel, UniqueId);
		}

		public override void Authenticate()
		{
			Logger.Debug("Authenticating connection {0}", LogSource.Channel, UniqueId);

			if (channelState == ChannelState.Authenticated)
				return;

			if (channelState != ChannelState.Connected)
			{
				Logger.Error("Unable to authenticate in current channel state. ChannelState = {0}", LogSource.Channel, channelState);

				throw new ChannelException("Unable to authenticate in current channel state");
			}

			try
			{
				client.Capability();
				client.YahooImap();
				client.Authenticate(Username, Password);

				channelState = ChannelState.Authenticated;

				Logger.Debug("SUCCESS Authenticating connection {0}", LogSource.Channel, UniqueId);
			}
			catch (IMAP_ClientException ex)
			{
				channelState = ChannelState.Broken;

				Logger.Debug("FAILED Authenticating connection {0}. Server said {1}", LogSource.Channel, UniqueId, ex.ResponseText);

				throw new ChannelAuthenticationException(ex.ResponseText, ex);
			}
		}

		public override void Close()
		{
			if (client != null && client.IsConnected)
			{
				client.Disconnect();
			}

			channelState = ChannelState.Closed;
		}

		public override void Dispose()
		{
		}
	}
}
