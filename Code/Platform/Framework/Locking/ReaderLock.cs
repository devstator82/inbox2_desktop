using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Interfaces;
using System.Diagnostics;

namespace Inbox2.Platform.Framework.Locking
{
	[Serializable]
	public class ReaderLock : IDisposable
	{
		protected ISynchronizedObject synchronizedObject;

		public ReaderLock(ISynchronizedObject synchronizedObject)
		{
			this.synchronizedObject = synchronizedObject;

			if (!Debugger.IsAttached)
				synchronizedObject.AcquireReaderLock();
		}

		public virtual void Dispose()
		{
			if (!Debugger.IsAttached)
				synchronizedObject.ReleaseReaderLock();
		}
	}
}