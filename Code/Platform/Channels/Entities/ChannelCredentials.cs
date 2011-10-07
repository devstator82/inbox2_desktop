using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Inbox2.Platform.Channels.Entities
{
	public class ChannelCredentials
	{
		public string Claim;

		public string Evidence;

		public NetworkCredential ToNetworkCredential()
		{
			return new NetworkCredential(Claim, Evidence);
		}
	}
}