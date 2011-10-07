using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Channels.Twitter
{
	public class TwitterProfileInfoBuilder : ChannelProfileInfoBuilder
	{
		public override string StreamUrl
		{
			get { return "http://twitter.com/home"; }
		}

		public override string BuildServiceProfileUrl(SourceAddress address)
		{
			return String.Format("http://www.twitter.com/{0}", address.Address);
		}

		public override string BuildServiceProfileUrl(ChannelProfile profile)
		{
			return String.Format("http://www.twitter.com/{0}", profile.ScreenName);
		}
	}
}
