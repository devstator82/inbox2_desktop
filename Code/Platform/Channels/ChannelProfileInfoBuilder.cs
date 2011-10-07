using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Platform.Channels
{
	public class ChannelProfileInfoBuilder
	{
		public virtual string StreamUrl
		{
			get { return String.Empty; }
		}

		public virtual string InboxUrl
		{
			get { return String.Empty; }
		}

		public virtual string BuildServiceProfileUrl(SourceAddress address)
		{
			return String.Empty;
		}

		public virtual string BuildServiceProfileUrl(ChannelProfile profile)
		{
			return String.Empty;
		}
	}
}
