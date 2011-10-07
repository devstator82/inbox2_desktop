using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Framework
{
	public class ThreadFlag : IDisposable
	{
		[ThreadStatic] public static bool IsSet;

		public ThreadFlag()
		{
			IsSet = true;
		}

		public void Dispose()
		{
			IsSet = false;
		}
	}
}
