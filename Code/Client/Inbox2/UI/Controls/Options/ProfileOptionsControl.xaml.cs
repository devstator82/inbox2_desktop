using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Inbox2.Core.Configuration;
using Inbox2.Core.Threading.Tasks;
using Inbox2.Core.Threading.Tasks.Application;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interop.Windows;
using Inbox2.Framework.Localization;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Logging;

namespace Inbox2.UI.Controls.Options
{
	/// <summary>
	/// Interaction logic for ProfileOptionsControl.xaml
	/// </summary>
	public partial class ProfileOptionsControl : UserControl
	{
		#region Properties

		public IEnumerable<ChannelConfiguration> ChannelConfigurations
		{
			get
			{
				return ChannelsManager.Channels.Select(s => s.Configuration)
					.Where(c => c.Charasteristics.SupportsEmail);
			}
		}

		public bool HasMailChannelConfigured
		{
			get { return ChannelConfigurations.Any(); }
		}

		#endregion

		#region Construction

		public ProfileOptionsControl()
		{
			InitializeComponent();

			DataContext = this;
		}

		#endregion

		#region Methods

		public bool TrySave()
		{
			bool isValid = true;

			// Save Fullname
			if (!String.IsNullOrEmpty(DisplayNameTextBox.Text.Trim()))
			{
				SettingsManager.ClientSettings.AppConfiguration.Fullname = DisplayNameTextBox.Text;
			}
			else
			{
				Inbox2MessageBox.Show("Please enter a display name", Inbox2MessageBoxButton.OK);

				isValid = false;
			}

			// Save Signature
			if (isValid)
			{
				SettingsManager.ClientSettings.AppConfiguration.Signature = SignatureTextBox.Text;
                SettingsManager.ClientSettings.AppConfiguration.IsStatsDisabled = IsStatsDisabledCheckBox.IsChecked ?? false;
				SettingsManager.ClientSettings.AppConfiguration.IsDefaultMailClientCheckEnabled = IsDefaultEmailClientCheckBox.IsChecked ?? false;
			}

			if (isValid)
			{
				if (HasMailChannelConfigured && DefaultEmailAddressComboBox.HasItems)
				{
					// If no default e-mail address has been selected, select the first one
					if (DefaultEmailAddressComboBox.SelectedItem == null)
						DefaultEmailAddressComboBox.SelectedIndex = 0;

					// Get the selected default e-mail address
					var defaultChannel = (ChannelConfiguration)DefaultEmailAddressComboBox.SelectedItem;

					// First set all channels to false status
					ChannelsManager.Channels.Select(c => c.Configuration).ForEach(c => c.IsDefault = false);

					// Set the channel to default
					defaultChannel.IsDefault = true;

					// Update database
					ClientState.Current.DataService.ExecuteNonQuery(String.Format("update ChannelConfigs set IsDefault='{0}'", false));
					ClientState.Current.DataService.ExecuteNonQuery(
						String.Format("update ChannelConfigs set IsDefault='{0}' where ChannelConfigId={1}",
							defaultChannel.IsDefault, defaultChannel.ChannelId));
				}
			}

			return isValid;
		}

		static void RunReceive()
		{
			EventBroker.Publish(AppEvents.RequestReceive, Int32.MaxValue);

			EventBroker.Subscribe(AppEvents.ReceiveFinished, () => Thread.CurrentThread.ExecuteOnUIThread(delegate
              	{
              		if (SettingsManager.ClientSettings.AppConfiguration.IsLoadingAllMessages)
              		{
              			SettingsManager.ClientSettings.AppConfiguration.IsLoadingAllMessages = false;
              			SettingsManager.Save();
              		}
              	}));
		}

		#endregion

		#region Event handlers

		void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			DisplayNameTextBox.Text = SettingsManager.ClientSettings.AppConfiguration.Fullname;
			SignatureTextBox.Text = SettingsManager.ClientSettings.AppConfiguration.Signature;
            IsStatsDisabledCheckBox.IsChecked = SettingsManager.ClientSettings.AppConfiguration.IsStatsDisabled;
			IsDefaultEmailClientCheckBox.IsChecked = SettingsManager.ClientSettings.AppConfiguration.IsDefaultMailClientCheckEnabled;

			if (HasMailChannelConfigured)
				DefaultEmailAddressComboBox.SelectedItem = ChannelsManager.GetDefaultChannel().Configuration;
		}

		/// <summary>
		/// User clicks on the load all messages button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void LoadAllMessages_Click(object sender, RoutedEventArgs e)
		{
			if (Inbox2MessageBox.Show(Strings.ThisCouldTakeALongTime, Inbox2MessageBoxButton.YesNo).Result == Inbox2MessageBoxResult.Yes)
			{
				// todo mwa
				// Cancel any running receive tasks
				//if (((TaskQueue)ClientState.Current.TaskQueue).ProcessingPool.HasRunning<ReceiveTask>())
				//{
				//    // Let current send/receive finish
				//    IEventReg subscription = null;

				//    subscription = EventBroker.Subscribe(AppEvents.ReceiveFinished, delegate
				//        {
				//            EventBroker.Unregister(subscription);

				//            RunReceive();
				//        });
				//}

				RunReceive();

				SettingsManager.ClientSettings.AppConfiguration.IsLoadingAllMessages = true;
				SettingsManager.Save();

				LoadAllMessagesButton.IsEnabled = false;
			}
		}

		void CheckNow_Click(object sender, RoutedEventArgs e)
		{
			// Check if Inbox2 is the default mail client
			var handler = new DefaultMailClientHandler();

			if (!handler.IsDefaultMailClient())
			{
				var result = Inbox2MessageBox.Show(Strings.NotYourDefaultMailClient, Inbox2MessageBoxButton.YesNo);

				if (result.Result == Inbox2MessageBoxResult.Yes)
				{
					var executable = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Inbox2DefaultMailClient.exe");

					try
					{
						// Start set default mail client process as administrator if needed
						new Process { StartInfo = new ProcessStartInfo(executable) { UseShellExecute = true, Verb = "runas" } }.Start();
					}
					catch (Exception ex)
					{
						Logger.Error("An error has occured while trying to start process. Exception = {0}", LogSource.UI, ex);
					}
				}
			}
			else
			{
				Inbox2MessageBox.Show(Strings.AlreadyDefaultMailClient, Inbox2MessageBoxButton.OK);
			}
		}

		void CheckForUpdateButton_Click(object sender, RoutedEventArgs e)
		{
			CheckForUpdateButton.IsEnabled = false;
			CheckForUpdateTextBlock.Text = Strings.CheckingForUpdate;

			ThreadPool.QueueUserWorkItem(delegate
             	{
             		var task = new CheckForUpdateTask();

             		string versionString;

             		if (task.CheckNeedsUpgrade(out versionString))
             		{
             			Thread.CurrentThread.ExecuteOnUIThread(delegate
                           	{
                           		CheckForUpdateTextBlock.Text = Strings.DownloadingUpdate;
                           	});

             			task.FinishedSuccess += delegate
                        	{
                        		Thread.CurrentThread.ExecuteOnUIThread(delegate
                               	{
                               		CheckForUpdateTextBlock.Text = Strings.UpdateDownloadFinished;
                               	});
                        	};

             			task.FinishedFailure += delegate
                        	{
                        		Thread.CurrentThread.ExecuteOnUIThread(delegate
                               	{
                               		CheckForUpdateTextBlock.Text = Strings.UpdateDownloadError;
                               		CheckForUpdateButton.IsEnabled = true;
                               	});
                        	};

             			task.ExecuteAsync();
             		}
             		else
             		{
             			Thread.CurrentThread.ExecuteOnUIThread(delegate
                           	{
                           		CheckForUpdateTextBlock.Text = Strings.NoUpdateAvailable;
                           		CheckForUpdateButton.IsEnabled = true;
                           	});
             		}
             	});
		}

		#endregion		
	}
}
