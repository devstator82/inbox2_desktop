using System;
using System.Collections.Generic;
using Inbox2.Platform.Channels;

namespace Inbox2.Framework
{
	public static class ListOrSingle<T> where T : class
	{
		public static List<T> Get(T single, IEnumerable<T> list)
		{
			if (single == null)
				return new List<T>(list);

			return new List<T> { single };
		}
	}
}
