using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Channels.Extensions
{
	public static class StringBuilderExtensions
	{
		public static void Clear(this StringBuilder sb)
		{
			sb.Remove(0, sb.Length -1);
		}
	}
}
