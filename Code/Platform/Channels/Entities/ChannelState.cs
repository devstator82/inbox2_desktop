using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Channels.Entities
{
	[Serializable]
	public enum ChannelState
	{
		Connecting,
		Connected,
		Authenticating,
		Authenticated,
		Open,
		Broken,
		Closed
	}
}