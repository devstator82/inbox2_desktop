using System;
using System.Collections.Generic;
using System.Linq;
using Inbox2.Core.Configuration.Channels;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Framework.VirtualMailBox
{
	public static class ChannelFactory
	{
		private static ChannelLoadHelper _helper;
		private static readonly object synclock = new object();

		static IEnumerable<ChannelConfiguration> AvailableChannels
		{
			get
			{
				if (_helper == null)
				{
					lock (synclock)
						_helper = new ChannelLoadHelper();
				}

				return _helper.AvailableChannels;
			}
		}

		public static ChannelConfiguration Create(ChannelConfig channel)
		{
			return channel.ChannelConnection == ChannelConnection.Local ? 
				BuildLocalChannel(channel) : BuildCloudChannel(channel);
		}

		static ChannelConfiguration BuildCloudChannel(ChannelConfig channel)
		{
			var instance = BuildLocalChannel(channel);

			instance.IsConnected = true;
			instance.ChannelKey = channel.ChannelKey;

			return instance;
		}

		static ChannelConfiguration BuildLocalChannel(ChannelConfig channel)
		{
			var configuration = AvailableChannels.First(c => c.DisplayName == channel.DisplayName).Clone();

			configuration.DisplayEnabled = true;
			configuration.ChannelId = channel.ChannelConfigId.Value;
			configuration.IsDefault = channel.IsDefault;

			if (configuration.DisplayStyle == DisplayStyle.Other ||
				channel.IsManuallyCustomized)
			{
				configuration.IsCustomized = channel.IsManuallyCustomized;

				ChannelBuilder.SetChannelAuthentication(channel.Username, channel.Password,
					configuration.InputChannel,
					configuration.ContactsChannel,
					configuration.CalendarChannel,
					configuration.StatusUpdatesChannel);

				configuration.InputChannel.Hostname = channel.Hostname;
				configuration.InputChannel.TypeSurrogate = channel.Type;				
				configuration.InputChannel.Port = channel.Port ?? 0;
				configuration.InputChannel.IsSecured = channel.SSL;

				ChannelBuilder.SetChannelAuthentication(
					channel.OutgoingUsername, channel.OutgoingPassword, configuration.OutputChannel);					
	
				configuration.OutputChannel.Hostname = channel.OutgoingHostname;
				configuration.OutputChannel.Port = channel.OutgoingPort ?? 0;
				configuration.OutputChannel.IsSecured = channel.OutgoingSSL;				
			}
			else if (configuration.DisplayStyle == DisplayStyle.Advanced)
			{
				ChannelBuilder.SetChannelHostname(channel.Hostname, configuration.InputChannel,
					configuration.OutputChannel, configuration.ContactsChannel,
					configuration.CalendarChannel, configuration.StatusUpdatesChannel);

				ChannelBuilder.SetChannelAuthentication(channel.Username, channel.Password,
					configuration.InputChannel,
					configuration.OutputChannel, configuration.ContactsChannel,
					configuration.CalendarChannel,
					configuration.StatusUpdatesChannel);
			}
			else
			{
				ChannelBuilder.SetChannelAuthentication(channel.Username, channel.Password,
					configuration.InputChannel,
					configuration.OutputChannel, configuration.ContactsChannel,
					configuration.CalendarChannel,
					configuration.StatusUpdatesChannel);
			}

			return configuration;
		}
	}
}
