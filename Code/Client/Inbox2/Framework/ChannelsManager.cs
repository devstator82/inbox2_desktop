using System;
using System.Collections.Generic;
using System.Linq;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Collections;
using Inbox2.Platform.Framework.Configuration;

namespace Inbox2.Framework
{
	public static class ChannelsManager
	{
		private static readonly AdvancedObservableCollection<ChannelInstance> _Channels;
		private static readonly List<ChannelProperties> _ChannelProperties;

		static ChannelsManager()
		{
			_Channels = new AdvancedObservableCollection<ChannelInstance>();
			_ChannelProperties = AdditionalChannelProperties.GetConfiguration();
		}

		public static string GetChannelColor(ChannelConfiguration config)
		{
			var properties = _ChannelProperties.FirstOrDefault(c => c.Name == config.DisplayName);

			return properties == null ? "#124" : String.IsNullOrEmpty(properties.PreferedColour) ? "#124" : properties.PreferedColour;
		}

		public static string GetChannelHelpUrl(ChannelConfiguration config)
		{
			var properties = _ChannelProperties.FirstOrDefault(c => c.Name == config.DisplayName);

			return properties == null ? String.Empty : properties.HelpUrl;
		}

		public static ChannelProperties GetChannelProperties(string displayName)
		{
			return _ChannelProperties.FirstOrDefault(c => c.Name == displayName);
		}

		/// <summary>
		/// Gets the channels.
		/// </summary>
		/// <value>The channels.</value>
		public static AdvancedObservableCollection<ChannelInstance> Channels
		{
			get { return _Channels; }
		}

		/// <summary>
		/// Adds the specified channel.
		/// </summary>
		/// <param name="channel">The channel.</param>
		public static ChannelInstance Add(ChannelConfiguration channel)
		{
			var channelInstance = new ChannelInstance(channel);

			_Channels.Add(channelInstance);

			return channelInstance;
		}

		/// <summary>
		/// Removes the specified channel.
		/// </summary>
		/// <param name="channel">The channel.</param>
		public static void Remove(ChannelInstance channel)
		{
			_Channels.Remove(channel);
		}

		/// <summary>
		/// Gets the channel object with the given search parameters.
		/// </summary>
		/// <param name="channelId"></param>
		/// <returns></returns>
		public static ChannelInstance GetChannelObject(long channelId)
		{
			foreach (ChannelInstance channel in _Channels)
			{
				if (channel.Configuration.ChannelId == channelId)
					return channel;
			}

			return null;
		}

		public static ChannelInstance GetChannelObject(string channelKey)
		{
			return GetChannelObject(channelKey, true);
		}

		public static ChannelInstance GetChannelObject(string channelKey, bool throwIfNull)
		{
			var channel = Channels
				.Where(c => c.Configuration.IsConnected)
				.FirstOrDefault(c => c.Configuration.ChannelKey == channelKey);

			if (channel == null && throwIfNull)
				throw new ApplicationException(String.Format("Unable to locate channel with key '{0}'", channelKey));

			return channel;
		}

		/// <summary>
		/// Gets the default channel based on a best guess.
		/// </summary>
		/// <returns></returns>
		public static ChannelInstance GetDefaultChannel()
		{
			foreach (var channel in _Channels)
			{
				if (channel.Configuration.IsDefault)
					return channel;
			}

			// No channel with default flag found, try to return the first available mail channel
			foreach (var channel in _Channels)
			{
				if (channel.Configuration.Charasteristics.SupportsHtml)
					return channel;
			}

			// No mail channel found, return the first channel
			return _Channels.FirstOrDefault();
		}

		/// <summary>
		/// Gets the default source address.
		/// </summary>
		/// <returns></returns>
		public static SourceAddress GetDefaultSourceAddress()
		{
			var channel = GetDefaultChannel();

			return new SourceAddress(channel.InputChannel.SourceAdress, ClientState.Current.Context.DisplayName);
		}

		/// <summary>
		/// Gets a list of all the channels that support status updates.
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<ChannelInstance> GetStatusChannels()
		{
			return Channels.Where(c => c.StatusUpdatesChannel != null);
		}

		/// <summary>
		/// Gets a list of all the unique status update channels.
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<ChannelInstance> GetUniqueStatusChannels()
		{
			var types = new List<string>();
			var channels = Channels.Where(c => c.StatusUpdatesChannel != null);

			foreach (var channel in channels)
			{
				if (types.Contains(channel.Configuration.DisplayName))
					continue;

				types.Add(channel.Configuration.DisplayName);

				yield return channel;
			}
		}

		/// <summary>
		/// Gets the list of all connected channels.
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<ChannelConfiguration> GetConnectedChannels()
		{
			return Channels
				.Where(c => c.Configuration.IsConnected)
				.Select(c => c.Configuration);
		}
	}
}