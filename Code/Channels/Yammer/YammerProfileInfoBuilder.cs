using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Channels.Yammer
{
	public class YammerProfileInfoBuilder : ChannelProfileInfoBuilder
	{
		public override string StreamUrl
		{
			get { return "http://www.yammer.com/home"; }
		}

		public override string BuildServiceProfileUrl(SourceAddress address)
		{
			return String.Format("https://www.yammer.com/users/{0}", address.Address);
		}

		public override string BuildServiceProfileUrl(ChannelProfile profile)
		{
			return String.Format("https://www.yammer.com/users/{0}", profile.SourceAddress.Address);
		}
	}
}
