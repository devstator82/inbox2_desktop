using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Threading;

namespace Inbox2.Platform.Framework.Collections
{
	/// <summary>
	/// ObservableCollection with additional helper methods.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[CollectionDataContract]
	public class AdvancedObservableCollection<T> : ObservableCollection<T>
	{
		/// <summary>
		/// Occurs when an item is added, removed, changed, moved, or the entire list is refreshed.
		/// </summary>
		public override event NotifyCollectionChangedEventHandler CollectionChanged;

		public virtual void Replace(IEnumerable<T> list)
		{
			this.Clear();

			AddRange(list);
		}

		public virtual void ReplaceWithCast(IEnumerable list)
		{
			this.Clear();

			foreach (var o in list)
			{
				Add((T)o);
			}
		}

		public virtual void AddRange(IEnumerable<T> range)
		{
			foreach (T t in range)
			{
				this.Add(t);
			}
		}

		public virtual void RemoveAll(Func<T, bool> t)
		{
			for (int i = this.Count -1; i >= 0; i--)
			{
				if (Count > 0)
				{
					if (t(this[i]))
						Remove(this[i]);
				}
			}			
		}

		/// <summary>
		/// Overriden for speed improvements, see: http://shevaspace.spaces.live.com/Blog/cns!FD9A0F1F8DD06954!547.entry
		/// </summary>
		/// <param name="e"></param>
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (this.CollectionChanged != null)
			{
				using (IDisposable disposable = this.BlockReentrancy())
				{
					foreach (Delegate del in this.CollectionChanged.GetInvocationList())
					{
						NotifyCollectionChangedEventHandler handler = (NotifyCollectionChangedEventHandler)del;
						DispatcherObject dispatcherInvoker = del.Target as DispatcherObject;
						ISynchronizeInvoke syncInvoker = del.Target as ISynchronizeInvoke;
						if (dispatcherInvoker != null)
						{
							// We are running inside DispatcherSynchronizationContext,
							// so we should invoke the event handler in the correct dispatcher.
							dispatcherInvoker.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
							{
								try
								{
									handler(this, e);
								}
								catch (Exception)
								{
									// MWA: Swallow the exception here because it usually is the result of something else
								}
							}));
						}
						else if (syncInvoker != null)
						{
							// We are running inside WindowsFormsSynchronizationContext,
							// so we should invoke the event handler in the correct context.
							syncInvoker.Invoke(del, new Object[] { this, e });
						}
						else
						{
							// We are running in free threaded context, so just directly invoke the event handler.
							handler(this, e);
						}
					}
				}
			}

			OnPropertyChanged(new PropertyChangedEventArgs("Count"));
		}
	}
}