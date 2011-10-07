using System;
using System.Collections.Generic;
using System.Linq;
using Inbox2.Framework;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Logging;

namespace Inbox2.Plugins.StatusUpdates.Helpers
{
	internal class SearchKeywordsHelper
	{
		internal static bool HasSearchChannel()
		{
			return ChannelsManager.GetStatusChannels().Any(c => c.Configuration.DisplayName == "Twitter");
		}

		internal static ChannelInstance GetSearchChannel()
		{
			return ChannelsManager.GetStatusChannels().First(c => c.Configuration.DisplayName == "Twitter");
		}

		internal static IEnumerable<string> GetKeywords()
		{
			var remove = new List<string>();
			var mailbox = VirtualMailBox.Current;

			// Create new result toolbar elements for all search keywords
			foreach (var kw in mailbox.StreamSearchKeywords.GetKeyWords())
			{
				var parts = kw.Split('|');

				if (parts.Length != 2)
				{
					Logger.Warn("Invalid search keyword. Keyword = {0}", LogSource.Sync, kw);
					continue;
				}

				var channelname = parts[0];
				var keyword = parts[1];

				// Find channel with given name
				var channel = ChannelsManager.GetStatusChannels().FirstOrDefault(c => c.Configuration.DisplayName == channelname);

				if (channel == null)
				{
					Logger.Warn("Could not find channel for search keyword. ChannelName = {0}, Keyword = {1}. Removing keyword from search list.", LogSource.Sync, channelname, keyword);
					remove.Add(kw);

					continue;
				}

				yield return keyword;
			}

			// Remove all invalid keywords
			foreach (var kw in remove)
				mailbox.StreamSearchKeywords.Remove(kw);
		}
	}
}
