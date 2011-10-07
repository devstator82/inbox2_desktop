using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Inbox2.Framework.Interfaces.Plugins
{
	public interface IColumnPlugin
	{
		bool ShowInOverview { get; }

		bool ShowInSearch { get; }

		double PreferredWidth { get; }		

		UIElement CreateColumnView();
	}
}