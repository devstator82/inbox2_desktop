using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Framework.Extensions
{
	public static class GuidExtensions
	{
		public static string ToConversationId(this Guid guid)
		{
			return "ib2" + guid.ToString().Replace("-", "");
		}

		public static string GetHash(this Guid guid)
		{
			return GetHash(guid, 12);
		}

		public static string GetHash(this Guid guid, int length)
		{
			string guidResult = string.Empty;

			while (guidResult.Length < length)
			{
				// Get the GUID.
				guidResult += Guid.NewGuid().ToString().GetHashCode().ToString("x");
			}

			// Return the first length bytes.
			return guidResult.Substring(0, length);
		}
	}
}