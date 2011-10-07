using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Framework.Extensions;

namespace Inbox2.Framework.Utils
{
	public static class DebugKeys
	{
		public static bool IsManaged
		{
			get
			{
				return "/Inbox2/Client/Debug/ForceManagedMode".AsKey(false);
			}
		}

		public static string StubbedAuthToken
		{
			get
			{
				return "/Inbox2/Client/Debug/StubbedAuthToken".AsKey(String.Empty);
			}
		}
	}
}
