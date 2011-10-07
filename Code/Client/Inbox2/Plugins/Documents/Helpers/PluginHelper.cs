using System;
using System.Windows;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Plugins.Documents.Controls;

namespace Inbox2.Plugins.Documents.Helpers
{
	class PluginHelper : IColumnPlugin
	{
		private readonly DocumentsState state;

		public PluginHelper(DocumentsState state)
		{
			this.state = state;
		}

		#region IColumnPlugin

		public bool ShowInOverview
		{
			get { return false; }
		}

		public bool ShowInSearch
		{
			get { return true; }
		}

		double IColumnPlugin.PreferredWidth
		{
			get { return 1; }
		}

		UIElement IColumnPlugin.CreateColumnView()
		{
			return new Column();
		}

		#endregion
	}
}