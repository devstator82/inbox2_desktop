using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Inbox2.Framework.Interfaces.Plugins
{
	public interface IControllableTab
	{
		event EventHandler<EventArgs> RequestCloseTab;		

		string Title { get; }

		Control CustomHeaderContent { get; }
	}
}
