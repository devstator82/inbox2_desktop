using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Plugins.StatusUpdates.Controls;

namespace Inbox2.Plugins.StatusUpdates.Helpers
{
	public class PluginHelper : IColumnPlugin
	{
		bool IColumnPlugin.ShowInOverview
		{
			get { return true; }
		}

		bool IColumnPlugin.ShowInSearch
		{
			get { return false; }
		}

		double IColumnPlugin.PreferredWidth
		{
			get { return 200; }
		}

		UIElement IColumnPlugin.CreateColumnView()
		{
			return new OverviewColumn();
		}
	}
}
