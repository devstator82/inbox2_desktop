using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Collections;

namespace Inbox2.Framework.Extensions
{
	public static class SourceAddressExtensions
	{
		public static string ToHumanFriendlyString(this AdvancedObservableCollection<SourceAddress> source)
		{
			if (source == null || source.Count == 0)
				return String.Empty;

			if (source.Count == 1)
				return source[0].DisplayName;

			return String.Format("{0} and {1} other person(s)", source[0].DisplayName, source.Count - 1);
		}
	}
}
