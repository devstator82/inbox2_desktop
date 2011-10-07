using System;
using Inbox2.Platform.Interfaces;

namespace Inbox2.Platform.Framework.Locking
{
	public class WriterLock : IDisposable
	{
		protected ISynchronizedObject synchronizedObject;

		public WriterLock(ISynchronizedObject synchronizedObject)
		{
			this.synchronizedObject = synchronizedObject;

			synchronizedObject.AcquireWriterLock();
		}

		public void Dispose()
		{
			synchronizedObject.ReleaseWriterLock();
		}
	}
}
