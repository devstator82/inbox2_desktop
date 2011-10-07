using System;
using System.Windows;
using System.Windows.Controls;
using Inbox2.Core.Configuration;

namespace Inbox2.UI.Controls.Options
{
	/// <summary>
	/// Interaction logic for MessagesOptionsControl.xaml
	/// </summary>
	public partial class MessagesOptionsControl : UserControl
	{
		public MessagesOptionsControl()
		{
			InitializeComponent();

			DataContext = this;
		}

		public bool TrySave()
		{
			var config = SettingsManager.ClientSettings.AppConfiguration;

			if (!String.IsNullOrEmpty(ReceiveEmailsTextBox.Text.Trim()))
				config.ReceiveConfiguration.ReceiveInterval = Int16.Parse(ReceiveEmailsTextBox.Text);

			if (!String.IsNullOrEmpty(SyncStatusUpdatesTextBox.Text.Trim()))
				config.ReceiveConfiguration.SyncStatusUpdatesInterval = Int16.Parse(SyncStatusUpdatesTextBox.Text);

			if (!String.IsNullOrEmpty(SyncSearchStreamTextBox.Text.Trim()))
				config.ReceiveConfiguration.SyncSearchStreamInterval = Int16.Parse(SyncSearchStreamTextBox.Text);

			if (!String.IsNullOrEmpty(SyncContactsTextBox.Text.Trim()))
				config.ReceiveConfiguration.SyncContactsInterval = Int16.Parse(SyncContactsTextBox.Text);

			config.MarkReadWhenViewing = MarkMessagesReadWhenViewing.IsChecked ?? false;
			config.MarkReadWhenViewingAfter = null;

			if (MarkMessagesReadAfter.IsChecked ?? false)
			{
				if (!String.IsNullOrEmpty(MarkMessagesReadAfterSecondsTextBox.Text.Trim()))
					config.MarkReadWhenViewingAfter = Int16.Parse(MarkMessagesReadAfterSecondsTextBox.Text);
			}

			config.ShowNotificationsPopup = ShowNotificationsPopupCheckBox.IsChecked ?? false;
			config.ShowNotificationsPopupFor = Int16.Parse(ShowNotificationsPopupForTextBox.Text);
			config.PlayNotificationsSound = PlayNotificationsSoundCheckBox.IsChecked ?? false;

			SettingsManager.Save();

			return true;
		}

		void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			var config = SettingsManager.ClientSettings.AppConfiguration;

			ReceiveEmailsTextBox.Text = config.ReceiveConfiguration.ReceiveInterval.ToString();
			SyncStatusUpdatesTextBox.Text = config.ReceiveConfiguration.SyncStatusUpdatesInterval.ToString();
			SyncSearchStreamTextBox.Text = config.ReceiveConfiguration.SyncSearchStreamInterval.ToString();
			SyncContactsTextBox.Text = config.ReceiveConfiguration.SyncContactsInterval.ToString();
			MarkMessagesReadWhenViewing.IsChecked = config.MarkReadWhenViewing;

			if (config.MarkReadWhenViewingAfter.HasValue)
			{
				MarkMessagesReadAfterSecondsTextBox.Text = config.MarkReadWhenViewingAfter.Value.ToString();
				MarkMessagesReadAfter.IsChecked = true;
			}

			ShowNotificationsPopupCheckBox.IsChecked = config.ShowNotificationsPopup;
			ShowNotificationsPopupForTextBox.Text = config.ShowNotificationsPopupFor.ToString();
			PlayNotificationsSoundCheckBox.IsChecked = config.PlayNotificationsSound;
		}

		void MarkMessagesReadWhenViewing_Checked(object sender, RoutedEventArgs e)
		{
			MarkMessagesReadAfter.IsChecked = false;
		}

		void MarkMessagesReadAfter_Checked(object sender, RoutedEventArgs e)
		{
			MarkMessagesReadWhenViewing.IsChecked = false;
		}		
	}
}
