using System;
using System.Collections.Generic;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Framework.VirtualMailBox.Comparers
{
	public class MailAddressEqualityComparer : IEqualityComparer<SourceAddress>
	{
		public bool Equals(SourceAddress x, SourceAddress y)
		{
			return y.Address.Equals(x.Address, StringComparison.InvariantCultureIgnoreCase);
		}

		public int GetHashCode(SourceAddress obj)
		{
			return obj.GetHashCode();
		}
	}
}


