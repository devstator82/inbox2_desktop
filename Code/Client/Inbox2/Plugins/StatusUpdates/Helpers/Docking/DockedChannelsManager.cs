using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Inbox2.Core.Configuration;
using Inbox2.Framework;
using Inbox2.Platform.Channels;

namespace Inbox2.Plugins.StatusUpdates.Helpers.Docking
{
	public static class DockedChannelsManager
	{
		private const string SettingsKey = "/Settings/UI/Overview/DockedChannels";
		private static List<DockedChannel> _list;

		public static bool HasDockedChannels()
		{
			EnsureList();

			return _list.Any();
		}

		public static List<DockedChannel> GetChannels()
		{
			EnsureList();

			var removed = _list.Where(c => c.Channel == null).ToList();

			if (removed.Count > 0)
			{
				foreach (var channel in removed)
					_list.Remove(channel);

				SaveValue();
			}

			return _list;
		}

		public static bool IsDocked(long channelId)
		{
			EnsureList();

			return _list.Any(l => l.ChannelId == channelId && String.IsNullOrEmpty(l.Keyword));
		}

		public static bool IsDocked(string keyword)
		{
			EnsureList();

			return _list.Any(l => l.ChannelKeyword == keyword);
		}

		public static void Dock(ChannelInstance channel, string keyword)
		{
			EnsureList();

			_list.Add(new DockedChannel { ChannelId = channel.Configuration.ChannelId, Keyword = keyword });

			SaveValue();
		}

		public static void Undock(ChannelInstance channel)
		{
			EnsureList();

			_list.RemoveAll(l => l.ChannelId == channel.Configuration.ChannelId);

			SaveValue();
		}

		public static void Undock(string keyword)
		{
			EnsureList();

			_list.RemoveAll(l => l.Keyword == keyword);

			SaveValue();
		}

		static void EnsureList()
		{
			if (_list == null)
			{
				var obj = ClientState.Current.Context.GetSetting(SettingsKey);

				if (obj is List<DockedChannel>)
					_list = (List<DockedChannel>)obj;
				else
					_list = new List<DockedChannel>();
			}
		}

		static void SaveValue()
		{
			ClientState.Current.Context.SaveSetting(SettingsKey, _list);
		}
	}
}