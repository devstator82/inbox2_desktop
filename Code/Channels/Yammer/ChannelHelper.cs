using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels;

namespace Inbox2.Channels.Yammer
{
	public static class ChannelHelper
	{
		public const string ConsumerKey = "IR9NNT4JOxODau1wHzySgA";
		public const string ConsumerSecret = "VwkIF3L6ZAVtt7jTDFQbfrfcwDdTgBHa0xN32qeI";

		public static string Token
		{
			get { return ChannelContext.Current.ClientContext.GetSetting("/Channels/Yammer/AuthToken") as string; }
		}

		public static string TokenSecret
		{
			get { return ChannelContext.Current.ClientContext.GetSetting("/Channels/Yammer/AuthSecret") as string; }
		}
	}
}