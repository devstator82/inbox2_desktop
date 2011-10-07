using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Inbox2.Platform.Framework.Collections;

namespace Inbox2.Framework.Collections
{
	public class CustomCompositeCollection<T> : List<T>
	{
		private readonly AdvancedObservableCollection<T>[] lists;

		public event EventHandler<EventArgs> CollectionChanged;

		public CustomCompositeCollection(params AdvancedObservableCollection<T>[] lists)
		{
			this.lists = lists;
			foreach (var list in lists)
			{
				AddSource(list);
			}
		}

		public void AddSource(AdvancedObservableCollection<T> list)
		{
			AddRange(list);

			list.CollectionChanged += Inner_CollectionChanged;
		}

		protected virtual void OnItemAdded(T addedItem)
		{
		}

		protected virtual void OnItemRemoved(T removedItem)
		{
		}

		void Inner_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null && e.NewItems.Count > 0)
			{
				foreach (T item in e.NewItems)
				{
					Add(item);

					OnItemAdded(item);
				}
			}

			if (e.OldItems != null && e.OldItems.Count > 0)
			{
				foreach (T item in e.OldItems)
				{
					Remove(item);

					OnItemRemoved(item);
				}
			}

			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				Clear();

				foreach (var list in lists)
					AddRange(list);
			}

			if (CollectionChanged != null)
				CollectionChanged(this, EventArgs.Empty);
		}
	}
}
