using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Plugins.Contacts.Controls;

namespace Inbox2.Plugins.Contacts.Helpers
{
	class PluginHelper : IColumnPlugin, IOverviewPlugin
	{
		private ContactsState state;
		
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

		#region IOverviewPlugin

		public string Header
		{
			get { return "Contacts"; }
		}

		public ImageSource Icon
		{
			get { return new BitmapImage(new Uri("/Inbox2.UI.Resources;component/icons/contacts-icon.png", UriKind.Relative)); }
		}

		public WellKnownView WellKnownView
		{
			get { return WellKnownView.Contacts; }
		}

		public UIElement CreateView()
		{
			return new Overview();
		}

		#endregion

		public PluginHelper(ContactsState state)
		{
			this.state = state;
		}
	}
}
