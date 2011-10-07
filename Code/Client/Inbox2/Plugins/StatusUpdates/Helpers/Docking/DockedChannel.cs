using System;
using Inbox2.Framework;
using Inbox2.Platform.Channels;

namespace Inbox2.Plugins.StatusUpdates.Helpers.Docking
{
	public class DockedChannel
	{
		public long ChannelId { get; set; }

		public string Keyword { get; set; }

		public bool IsSearchChannel
		{
			get { return !String.IsNullOrEmpty(Keyword); }
		}

		public string ChannelKeyword
		{
			get { return String.Format("{0}|{1}", Channel.Configuration.DisplayName, Keyword); }
		}

		public ChannelInstance Channel
		{
			get
			{
				return ChannelsManager.GetChannelObject(ChannelId);
			}
		}
	}
}