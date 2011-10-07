using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Plugins.Notes.Controls;

namespace Inbox2.Plugins.Notes.Helpers
{
	class PluginHelper : IColumnPlugin, INewItemViewPlugin
	{
		private NotesState state;

		public PluginHelper(NotesState state)
		{
			this.state = state;
		}

		#region Implementation of IColumnPlugin

		double IColumnPlugin.PreferredWidth
		{
			get { return 1; }
		}

		UIElement IColumnPlugin.CreateColumnView()
		{
			return new Column();
		}

		#endregion

		#region Implementation of INewItemViewPlugin

		ImageSource INewItemViewPlugin.Icon
		{
			get { return new BitmapImage(new Uri("pack://application:,,,/Inbox2.UI.Resources;component/icons/icon_forlater.png")); }
		}

		string INewItemViewPlugin.Header
		{
			get { return "Note"; }
		}

		UIElement INewItemViewPlugin.CreateNewItemView()
		{
			return null;
		}

		#endregion
	}
}
