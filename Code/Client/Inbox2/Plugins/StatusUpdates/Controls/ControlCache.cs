using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels;

namespace Inbox2.Plugins.StatusUpdates.Controls
{
	internal static class ControlCache
	{
		static Dictionary<ChannelInstance, RealtimeStream> channelsDict;
		static Dictionary<string, RealtimeStream> searchDict;

		static ControlCache()
		{
			channelsDict = new Dictionary<ChannelInstance, RealtimeStream>();
			searchDict = new Dictionary<string, RealtimeStream>();
		}

		internal static RealtimeStream Get(ChannelInstance channel, string keyword, bool isColumnView)
		{
			RealtimeStream stream;

			if (!String.IsNullOrEmpty(keyword))
			{
				// Search stream
				if (!searchDict.ContainsKey(keyword))
				{
					stream = new RealtimeStream(channel, keyword);

					searchDict.Add(keyword, stream);
				}

				// Return cached copy
				var view = searchDict[keyword];
				view.IsColumnView = isColumnView;
				
				return view;
			}
			else
			{
				if (!channelsDict.ContainsKey(channel))
				{
					stream = new RealtimeStream(channel, keyword);

					channelsDict.Add(channel, stream);
				}

				var view = channelsDict[channel];
				view.IsColumnView = isColumnView;

				// Return cached copy
				return view;
			}
		}
	}
}
