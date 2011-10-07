using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Channels.Entities
{
	public class SourceAddressComparer : IEqualityComparer<SourceAddress>
	{
		public bool Equals(SourceAddress x, SourceAddress y)
		{
			return x.Address.Equals(y.Address, StringComparison.InvariantCultureIgnoreCase);
		}

		public int GetHashCode(SourceAddress obj)
		{
			return obj.ToString().GetHashCode();
		}
	}
}
