using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Inbox2.Framework.UI
{
	/// <summary>
	/// Fixes a weird and nasty issue where when removing an item from a collectionviewsource with
	/// a filter attached to it, commandbindinds all over the app stop to work.
	/// Resetting the selectedindex seems to fix it, don't ask, just apply this fix :-)
	/// </summary>
	public class ListViewIndexFix : IDisposable
	{
		private readonly ListView listView;
		private readonly int currentIndex;

		public ListViewIndexFix(ListView listView)
		{
			this.listView = listView;

			currentIndex = listView.SelectedIndex;
		}

		public void Dispose()
		{
			listView.SelectedIndex = currentIndex;
		}
	}
}
