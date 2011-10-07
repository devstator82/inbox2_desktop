using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.UI.Controls
{
	public class TabItemEventArgs : EventArgs
	{
		TabItem _item;

		public TabItem TabItem
		{
			get { return _item; }
		}

		public TabItemEventArgs(TabItem item)
		{
			_item = item;
		}
	}

	public class TabItemCancelEventArgs : CancelEventArgs
	{
		TabItem _item;

		public TabItem TabItem
		{
			get { return _item; }
		}

		public TabItemCancelEventArgs(TabItem item)
			: base()
		{
			_item = item;
		}
	}
}
