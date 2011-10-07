using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Framework.Extensions
{
	public static class ListExtensions
	{
		public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
		{
			foreach (var item in sequence)
			{
				action(item);
			}
		}
	}
}