using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Plugins.Conversations.Controls;

namespace Inbox2.Plugins.Conversations.Helpers
{
	class NewItemsPluginHelper : INewItemViewPlugin
	{
		private readonly ConversationsState state;

		public NewItemsPluginHelper(ConversationsState state)
		{
			this.state = state;
		}	

		#region INewItemViewPlugin

		ImageSource INewItemViewPlugin.Icon
		{
			get { return new BitmapImage(new Uri("pack://application:,,,/Inbox2.UI.Resources;component/icons/newmessage-icon.png")); }
		}

		string INewItemViewPlugin.Header
		{
			get { return "Message"; }
		}

		public bool ForceSingle
		{
			get { return true; }
		}

		UIElement IViewPlugin.CreateView()
		{
			return new NewItemView();
		}

		#endregion
	}
}
