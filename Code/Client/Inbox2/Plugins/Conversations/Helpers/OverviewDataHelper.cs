using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Plugins.Conversations.Helpers
{
	[DataContract]
	public class OverviewDataHelper
	{
		[DataMember]
		public long MessageId { get; set; }

        public bool MakeNavigatorCurrent { get; set; }

		public bool IgnoreNavigatorHistory { get; set; }

		public override bool Equals(object obj)
		{
			var other = obj as OverviewDataHelper;

			if (other == null)
				return false;

			return MessageId.Equals(other.MessageId);
		}

		public override int GetHashCode()
		{
			return MessageId.GetHashCode();
		}
	}
}