using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Inbox2.Framework.Localization;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Plugins.ColumnSearch.Controls;

namespace Inbox2.Plugins.ColumnSearch.Helpers
{
	class PluginHelper : IDetailsViewPlugin
	{
		private readonly ColumnSearchState state;

		public PluginHelper(ColumnSearchState state)
		{
			this.state = state;
		}

		#region IDetailsViewPlugin

		ImageSource IDetailsViewPlugin.Icon
		{
			get { return null; }
		}

		string IDetailsViewPlugin.Header
		{
			get { return Strings.Search; }
		}

		bool IDetailsViewPlugin.ForceSingle
		{
			get { return false; }
		}

		UIElement IViewPlugin.CreateView()
		{
			state.LastDetailsView = new DetailsView();

			return state.LastDetailsView;
		}

		#endregion
	}
}
