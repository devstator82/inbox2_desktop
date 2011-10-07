using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Channels.LinkedIn
{
	public class LinkedInProfileInfoBuilder : ChannelProfileInfoBuilder
	{
		public override string StreamUrl
		{
			get { return "http://www.linkedin.com/home"; }
		}

		public override string BuildServiceProfileUrl(SourceAddress address)
		{
			return address.ProfileUrl;
		}

		public override string BuildServiceProfileUrl(ChannelProfile profile)
		{
			return profile.Url;
		}
	}
}
