using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Inbox2.Platform.Framework.Collections;

namespace Inbox2.Framework.Collections
{
	public abstract class WrappedCollection<S, T, K> : AdvancedObservableCollection<T> where S : AdvancedObservableCollection<K> where T : new()
	{
		protected S source;
		protected Dictionary<K, T> map;

		protected WrappedCollection(S source)
		{
			this.source = source;
			this.source.CollectionChanged += SourceCollectionChanged;
			this.map = new Dictionary<K, T>();

			PopulateCollection();
		}

		void PopulateCollection()
		{
			Clear();

			foreach (K s in source)
			{
				T obj = CreateWrappedObject(s);

				Add(obj);
			}
		}		

		protected T CreateWrappedObject(K objectToWrap)
		{
			T wrappedObject = new T();

			GenerateWrappedItem(objectToWrap, wrappedObject);

			map.Add(objectToWrap, wrappedObject);

			return wrappedObject;
		}

		protected abstract void GenerateWrappedItem(K objectToWrap, T wrappedObject);


		void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				foreach (K item in e.NewItems)
				{
					var newObject = CreateWrappedObject(item);
					
					Add(newObject);
				}
			}

			if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				foreach (K item in e.OldItems)
				{
					var wrappedObject = map[item];

					Remove(wrappedObject);
				}
			}

			// ToDo: support NotifyCollectionChangedAction.Move and NotifyCollectionChangedAction.Replace

			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				PopulateCollection();
			}
		}
	}
}
