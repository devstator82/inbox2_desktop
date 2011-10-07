using System.Collections.Generic;
using Inbox2.Framework.VirtualMailBox.Entities;

namespace Inbox2.Framework.VirtualMailBox.Comparers
{
	public class MessageEqualityComparer : IEqualityComparer<Message>
	{
		public bool Equals(Message x, Message y)
		{
			return x == y || x.MessageId.Equals(y.MessageId);
		}

		public int GetHashCode(Message obj)
		{
			return obj.MessageId.GetHashCode();
		}
	}
}


