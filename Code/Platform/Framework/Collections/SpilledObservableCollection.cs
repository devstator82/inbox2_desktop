using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Framework.Collections
{
	public class SpilledObservableCollection<T> : AdvancedObservableCollection<T>
	{
		private readonly int spill;

		public SpilledObservableCollection(int spill)
		{
			this.spill = spill;
		}

		protected override void InsertItem(int index, T item)
		{
			base.InsertItem(index, item);

			if (Count > spill)
				RemoveAt(0);
		}
	}
}
