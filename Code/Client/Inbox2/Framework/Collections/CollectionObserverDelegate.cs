using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.Collections
{	
	public class CollectionObserverDelegate<T>
	{
		protected ObservableCollection<T> source;
		private readonly Action<T> added;
		private readonly Action<T> deleted;

		public CollectionObserverDelegate(ObservableCollection<T> source, Action<T> added, Action<T> deleted)
		{
			this.source = source;
			this.added = added;
			this.deleted = deleted;

			foreach (var item in this.source)
			{
				added(item);
			}

			this.source.CollectionChanged += source_CollectionChanged;
		}

		void source_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				if (added != null)
				{
					foreach (T item in e.NewItems)
					{
						added(item);
					}
				}
			} 
			else if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				if (deleted != null)
				{
					foreach (T item in e.OldItems)
					{
						deleted(item);
					}
				}
			}
		}
	}
}
