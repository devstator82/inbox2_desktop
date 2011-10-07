using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Threading;
using Inbox2.Platform.Framework.Collections;
using Inbox2.Platform.Framework.Locking;
using Inbox2.Platform.Interfaces;

namespace Inbox2.Framework.VirtualMailBox
{
	public class ThreadSafeCollection<T> : AdvancedObservableCollection<T>, ISynchronizedObject
	{
		private readonly Dispatcher _dispatcher;
		private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

		public ThreadSafeCollection()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        protected override void ClearItems()
        {
            if (_dispatcher.CheckAccess())
            {
                using (WriterLock)
					base.ClearItems();
            }
            else
            {
                _dispatcher.Invoke(DispatcherPriority.DataBind, (SendOrPostCallback)delegate { Clear(); }, this);
            }
        }

        protected override void InsertItem(int index, T item)
        {
            if (_dispatcher.CheckAccess())
            {
                if (index > this.Count)
                    return;

				using (WriterLock)
					base.InsertItem(index, item);
            }
            else
            {
                object[] e = new object[] { index, item };
                _dispatcher.Invoke(DispatcherPriority.DataBind, (SendOrPostCallback)delegate { InsertItemImpl(e); }, e);
            }
        }

        void InsertItemImpl(object[] e)
        {
            if (_dispatcher.CheckAccess())
            {
                InsertItem((int)e[0], (T)e[1]);
            }
            else
            {
                _dispatcher.Invoke(DispatcherPriority.DataBind, (SendOrPostCallback)delegate { InsertItemImpl(e); });
            }
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            if (_dispatcher.CheckAccess())
            {
                if (oldIndex >= this.Count | newIndex >= this.Count | oldIndex == newIndex)
                    return;

				using (WriterLock)
					base.MoveItem(oldIndex, newIndex);
            }
            else
            {
                object[] e = new object[] { oldIndex, newIndex };
                _dispatcher.Invoke(DispatcherPriority.DataBind, (SendOrPostCallback)delegate { MoveItemImpl(e); },e);
            }
        }

        void MoveItemImpl(object[] e)
        {
            if (_dispatcher.CheckAccess())
            {
                MoveItem((int)e[0], (int)e[1]);
            }
            else
            {
                _dispatcher.Invoke(DispatcherPriority.DataBind, (SendOrPostCallback)delegate { MoveItemImpl(e); });
            }
        }

        protected override void RemoveItem(int index)
        {
            if (_dispatcher.CheckAccess())
            {
                if (index >= this.Count)
                    return;

				using (WriterLock)
					base.RemoveItem(index);
            }
            else
            {
                _dispatcher.Invoke(DispatcherPriority.DataBind, (SendOrPostCallback)delegate { RemoveItem(index); }, index);
            }
        }

        protected override void SetItem(int index, T item)
        {
            if (_dispatcher.CheckAccess())
            {
				using (WriterLock)
					base.SetItem(index, item);
            }
            else
            {
                object[] e = new object[] { index, item };
                _dispatcher.Invoke(DispatcherPriority.DataBind, (SendOrPostCallback)delegate { SetItemImpl(e); },e);
            }
        }

        void SetItemImpl(object[] e)
        {
            if (_dispatcher.CheckAccess())
            {
                SetItem((int)e[0],(T)e[1]);
            }
            else
            {
                _dispatcher.Invoke(DispatcherPriority.DataBind, (SendOrPostCallback)delegate { SetItemImpl(e); });
            }
        }

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

		internal WriterLock WriterLock
		{
			get
			{
				return new WriterLock(this);
			}
		}

		#endregion	
	}
}
