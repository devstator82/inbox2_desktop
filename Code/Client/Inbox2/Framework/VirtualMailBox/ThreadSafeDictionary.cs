using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Inbox2.Platform.Framework.Locking;
using Inbox2.Platform.Interfaces;

namespace Inbox2.Framework.VirtualMailBox
{
	public class ThreadSafeDictionary<K, V> : Dictionary<K, V>, ISynchronizedObject
	{
		private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

		#region Locking implementation

		public void AcquireReaderLock()
		{
			_lock.EnterReadLock();
		}

		public void ReleaseReaderLock()
		{
			_lock.ExitReadLock();
		}

		public void AcquireWriterLock()
		{
			_lock.EnterWriteLock();
		}

		public void ReleaseWriterLock()
		{
			_lock.ExitWriteLock();
		}

		public ReaderLock ReaderLock
		{
			get
			{
				return new ReaderLock(this);
			}
		}

		public WriterLock WriterLock
		{
			get
			{
				return new WriterLock(this);
			}
		}

		#endregion	
	}
}
