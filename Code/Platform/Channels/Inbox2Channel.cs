using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Configuration;

namespace Inbox2.Platform.Channels
{
	public class Inbox2Channel : ChannelConfiguration
	{
		public override string DisplayName
		{
			get { return "Inbox2"; }
		}

		public override ChannelConfiguration Clone()
		{
			return new Inbox2Channel();
		}
	}
}
