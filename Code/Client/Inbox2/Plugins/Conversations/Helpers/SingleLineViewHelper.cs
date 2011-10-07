using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Framework.VirtualMailBox.View;
using Inbox2.Plugins.Conversations.Controls;

namespace Inbox2.Plugins.Conversations.Helpers
{
	public class SingleLineViewHelper : IStreamViewPlugin
	{
		private readonly ConversationsState state;
		private readonly StreamView view;

		public SingleLineViewHelper(ConversationsState state, StreamView view)
		{
			this.state = state;
			this.view = view;
		}

		#region IStreamViewPlugin

		public ImageSource StreamIcon
		{
			get { return new BitmapImage(new Uri("/Inbox2.Plugins.Conversations;component/resources/icon-view-one-line.gif", UriKind.Relative)); }
		}

		public string Header
		{
			get { return "Single line view"; }
		}

		public UIElement CreateStreamView()
		{
			return view;
		}

		public bool CanSwitchToView
		{
			get { return true; }
		}

		public void SwitchToView()
		{
			ViewFilter.Current.Filter.IsActivityViewVisible = false;

			if (ClientState.StartupSuccess)
				state.ActivityViewSource.View.Refresh();
		}

		#endregion
	}
}
