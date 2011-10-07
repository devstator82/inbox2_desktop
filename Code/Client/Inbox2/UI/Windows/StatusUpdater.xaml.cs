using System;
using System.Collections.Generic;
using System.Windows;
using Inbox2.Framework.Plugins.SharedControls;
using Inbox2.Framework.UI;
using Inbox2.Framework.VirtualMailBox.Entities;

namespace Inbox2.UI.Windows
{
    /// <summary>
    /// Interaction logic for StatusUpdater.xaml
    /// </summary>
    public partial class StatusUpdater : Window
    {
		private readonly bool fromSettings;

		public StatusUpdater()
			: this(StatusUpdateControl.GetSavedSelection())
		{
			fromSettings = true;
		}

		public StatusUpdater(IEnumerable<long> channels)
		{
			InitializeComponent();

			StatusUpdateControl.SelectedChannels.AddRange(channels);
		}

		public void SetText(string text)
		{
			StatusUpdateControl.Status = text;
		}

		public void SetReplyTo(UserStatus replyTo)
		{
			StatusUpdateControl.ReplyTo = replyTo;
		}

		void Window_Loaded(object sender, RoutedEventArgs e)
		{
			StatusUpdateControl.FocusFirstResponder();
		}

		void Window_Unloaded(object sender, RoutedEventArgs e)
		{
			if (fromSettings)
			{
				StatusUpdateControl.SaveChannelSelection();
			}
		}

    	void StatusUpdateControl_OnRequestClose(object sender, EventArgs e)
    	{
    		Close();
    	}
    }
}
