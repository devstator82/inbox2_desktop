using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Channels.Facebook
{
	public class FacebookProfileInfoBuilder : ChannelProfileInfoBuilder
	{
		public override string StreamUrl
		{
			get { return "http://www.facebook.com/home.php"; }
		}

		public override string InboxUrl
		{
			get { return "http://www.facebook.com/?sk=messages"; }
		}

		public override string BuildServiceProfileUrl(SourceAddress address)
		{
			return String.Format("http://www.facebook.com/profile.php?id={0}", address.Address);
		}

		public override string BuildServiceProfileUrl(ChannelProfile profile)
		{
			return String.Format("http://www.facebook.com/profile.php?id={0}", profile.SourceAddress.Address);
		}
	}
}
