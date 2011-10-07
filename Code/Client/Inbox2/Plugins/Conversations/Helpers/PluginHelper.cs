using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Framework.Localization;
using Inbox2.Plugins.Conversations.Controls;

namespace Inbox2.Plugins.Conversations.Helpers
{
	class PluginHelper : IColumnPlugin, IDetailsViewPlugin
	{
		private readonly ConversationsState state;

		public PluginHelper(ConversationsState state)
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
			get { return false; }
		}

		double IColumnPlugin.PreferredWidth
		{
			get { return 1.5; }
		}

		UIElement IColumnPlugin.CreateColumnView()
		{
			return new Column();
		}

		#endregion

		#region IDetailsViewPlugin

		string IDetailsViewPlugin.Header
		{
			get { return Strings.Message; }
		}

		ImageSource IDetailsViewPlugin.Icon
		{
			get { return null; }
		}

		bool IDetailsViewPlugin.ForceSingle
		{
			get { return true; }
		}

		UIElement IViewPlugin.CreateView()
		{
			return new DetailsView();
		}

		#endregion		
	}
}