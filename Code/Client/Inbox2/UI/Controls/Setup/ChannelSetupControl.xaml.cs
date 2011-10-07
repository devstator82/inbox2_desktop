using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Inbox2.Channels.Imap2;
using Inbox2.Channels.Pop3;
using Inbox2.Core.Threading.Tasks.Users;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Stats;
using Inbox2.Framework.Threading;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Core.Configuration;
using Inbox2.Framework.Localization;
using System.ComponentModel;

namespace Inbox2.UI.Controls.Setup
{
	/// <summary>
	/// Interaction logic for ChannelSetupControl.xaml
	/// </summary>
	public partial class ChannelSetupControl : UserControl, INotifyPropertyChanged
	{
		#region Fields

		protected bool IsChecking;
		protected int FailureCount;
		protected string channelKey;
		protected string token;
		protected string tokenSecret;

		public event PropertyChangedEventHandler PropertyChanged;

		public event EventHandler<EventArgs> OnCancel;
		public event EventHandler<EventArgs> OnValidationFinished;
		public event EventHandler<EventArgs> OnFormLayoutUpdated;

		#endregion

		#region Properties

		public ChannelConfiguration ChannelConfiguration { get; set; }
		public bool InviteFriends { get; set; }
		public bool IsInEditModus { get; set; }
		public bool IsValidated { get; private set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Hostname { get; set; }
		public string ManualEntryIncomingUsername { get; set; }
		public string ManualEntryIncomingPassword { get { return ManualEntryIncomingPasswordTextBox.Password; } }
		public string ManualEntryIncomingServer { get; set; }
		public int ManualEntryIncomingPort { get; set; }
		public bool ManualEntryIncomingSsl { get; set; }
		public string ManualEntryOutgoingUsername { get; set; }
		public string ManualEntryOutgoingPassword { get { return ManualEntryOutgoingPasswordTextBox.Password; } }
		public string ManualEntryOutgoingServer { get; set; }
		public int ManualEntryOutgoingPort { get; set; }
		public bool ManualEntryOutgoingSsl { get; set; }
		public bool IsManuallyCustomized { get; set; }
	
		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ChannelSetupControl"/> class.
		/// </summary>
		/// <param name="channelConfiguration">The channel configuration.</param>
		public ChannelSetupControl(ChannelConfiguration channelConfiguration)
		{
			ChannelConfiguration = channelConfiguration;

			InitializeComponent();

			// Set Header
			TitleTextBlock.Text =
				channelConfiguration.DisplayStyle == DisplayStyle.Other ?
				"Configure your own IMAP or POP3 account" :
				string.Format("Login to {0}", channelConfiguration.DisplayName);

			DataContext = this;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Called when [property changed].
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		/// <summary>
		/// Shows the setup control form.
		/// </summary>
		void ShowSetupControlForm()
		{
			// Show form fields base on display style.
			switch (ChannelConfiguration.DisplayStyle)
			{
				case DisplayStyle.None:
					break;
				case DisplayStyle.Simple:
					UsernameGrid.Visibility = (!IsManuallyCustomized) ? Visibility.Visible : Visibility.Collapsed;
					PasswordGrid.Visibility = (!IsManuallyCustomized) ? Visibility.Visible : Visibility.Collapsed;
					ManualEntryForm.Visibility = IsManuallyCustomized ? Visibility.Visible : Visibility.Collapsed;
					break;
				case DisplayStyle.Advanced:
					UsernameGrid.Visibility = (!IsManuallyCustomized) ? Visibility.Visible : Visibility.Collapsed;
					PasswordGrid.Visibility = (!IsManuallyCustomized) ? Visibility.Visible : Visibility.Collapsed;
					HostnameGrid.Visibility = (!IsManuallyCustomized) ? Visibility.Visible : Visibility.Collapsed;
					ManualEntryForm.Visibility = IsManuallyCustomized ? Visibility.Visible : Visibility.Collapsed;
					break;
				case DisplayStyle.FbConnect:
					FbConnectButtonGrid.Visibility = (!IsManuallyCustomized) ? Visibility.Visible : Visibility.Collapsed;
					CheckCredentialsButton.Visibility = (!IsManuallyCustomized) ? Visibility.Visible : Visibility.Hidden;
					break;
				case DisplayStyle.Redirect:
				case DisplayStyle.RedirectWithPin:
					LoginButtonGrid.Visibility = (!IsManuallyCustomized) ? Visibility.Visible : Visibility.Collapsed;
					break;
				case DisplayStyle.Other:
					ManualEntryForm.Visibility = (!IsManuallyCustomized) ? Visibility.Visible : Visibility.Collapsed;
					break;
			}
		
			// Show invite friend checkbox if the channel configuration is Twitter or Facebook.
			if (ChannelConfiguration.DisplayName.Equals("Twitter", StringComparison.InvariantCultureIgnoreCase) && IsInEditModus == false ||
				ChannelConfiguration.DisplayName.Equals("Facebook", StringComparison.InvariantCultureIgnoreCase) && IsInEditModus == false)
			{
				//InviteFriendGrid.Visibility = Visibility.Visible;

				// Not checked by default
				InviteFriends = false;
			}
			else
			{
				InviteFriendGrid.Visibility = Visibility.Collapsed;
				InviteFriends = false;
			}

			NeedHelpTextBlock.Visibility = Visibility.Hidden;

			var properties = ChannelsManager.GetChannelProperties(ChannelConfiguration.DisplayName);

			if (properties != null)
			{
				if (!String.IsNullOrEmpty(properties.UsernameHint))
					UsernameTextBox.Tag = properties.UsernameHint;

				if (!String.IsNullOrEmpty(properties.HostnameHint))
					HostnameTextBox.Tag = properties.HostnameHint;

				if (!String.IsNullOrEmpty(properties.HelpUrl))
					NeedHelpTextBlock.Visibility = Visibility.Visible;
			}

			OnPropertyChanged("InviteFriends");

			if (OnFormLayoutUpdated != null)
			{
				OnFormLayoutUpdated(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Validates the credentials.
		/// </summary>
		void ValidateChannel()
		{
			switch (ChannelConfiguration.DisplayStyle)
			{
				case DisplayStyle.None:
					break;
				case DisplayStyle.Simple:
				case DisplayStyle.Advanced:
				case DisplayStyle.Other:
					ValidateCredentials();
					break;
				case DisplayStyle.FbConnect:				
				case DisplayStyle.Redirect:
					CreateRedirect();
					break;
				case DisplayStyle.RedirectWithPin:
					if (PinGrid.Visibility == Visibility.Collapsed)
					{
						LoginButtonGrid.Visibility = Visibility.Collapsed;
						PinGrid.Visibility = Visibility.Visible;

						CreateRedirect();
					}
					else
					{
						ValidateCredentials();
					}
					break;
			}
		}

		/// <summary>
		/// Creates the redirect.
		/// </summary>
		void CreateRedirect()
		{
			// Create an instance of the InputChannel and check the provided credentials
			var window = new ChannelSetupRedirectWindow(ChannelConfiguration, false);

			window.ShowDialog();

			if (window.Success.HasValue)
			{
				if (window.Success ?? false)
				{
					token = window.Token;
					tokenSecret = window.TokenSecret;

					// Control is hidden anyway so doesn't matter that we use it for temporarily storing results. 
					// Value of this field if picked up again in the ValidateCredentials method.
					PincodeTextBox.Text = window.LastUri.ToString();

					ValidateCredentials();
				}
				else
				{
					Inbox2MessageBox.Show(String.Format(Strings.AddingAccountFailed, ChannelConfiguration.DisplayName), Inbox2MessageBoxButton.OK);

					FailureCount++;
				}
			}
			else
			{
				// Close setup control if we didn't get a result (user closed popup window)
				if (OnCancel != null)
				{
					OnCancel(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Validates the credentials.
		/// </summary>
		void ValidateCredentials()
		{
			if (!IsInEditModus)
				CheckCanvas.Visibility = Visibility.Collapsed;

			LoaderCanvas.Visibility = Visibility.Visible;
			IsChecking = true;

			#region Set channel authentication

			if (ChannelConfiguration.DisplayStyle == DisplayStyle.Other ||
				ChannelConfiguration.Charasteristics.CanCustomize && IsManuallyCustomized)
			{
				// Incoming Settings
				ChannelConfiguration.InputChannel.Authentication.Username = ManualEntryIncomingUsername;
				ChannelConfiguration.InputChannel.Authentication.Password = ManualEntryIncomingPassword;
				ChannelConfiguration.InputChannel.Hostname = ManualEntryIncomingServer;
				ChannelConfiguration.InputChannel.Port = ManualEntryIncomingPort;
				ChannelConfiguration.InputChannel.IsSecured = ManualEntryIncomingSsl;
				ChannelConfiguration.InputChannel.Type = AccountTypeImapRadioButton.IsChecked == true
															? typeof(Imap2ClientChannel)
															: typeof(Pop3ClientChannel);

				// Outgoing Settings
				ChannelConfiguration.OutputChannel.Authentication.Username = ManualEntryOutgoingUsername;
				ChannelConfiguration.OutputChannel.Authentication.Password = ManualEntryOutgoingPassword;
				ChannelConfiguration.OutputChannel.Hostname = ManualEntryOutgoingServer;
				ChannelConfiguration.OutputChannel.Port = ManualEntryOutgoingPort;
				ChannelConfiguration.OutputChannel.IsSecured = ManualEntryOutgoingSsl;

				ChannelBuilder.SetChannelAuthentication(
					ManualEntryIncomingUsername,
					ManualEntryIncomingPassword,
					ChannelConfiguration.InputChannel,
					ChannelConfiguration.OutputChannel);
			}
			else
			{
				if (ChannelConfiguration.DisplayStyle == DisplayStyle.Advanced)
					ChannelBuilder.SetChannelHostname(
						HostnameTextBox.Text,
						ChannelConfiguration.InputChannel,
						ChannelConfiguration.OutputChannel,
						ChannelConfiguration.ContactsChannel,
						ChannelConfiguration.CalendarChannel,
						ChannelConfiguration.StatusUpdatesChannel);

				ChannelBuilder.SetChannelAuthentication(
					Username,
					Password,
					ChannelConfiguration.InputChannel,
					ChannelConfiguration.OutputChannel,
					ChannelConfiguration.ContactsChannel,
					ChannelConfiguration.CalendarChannel,
					ChannelConfiguration.StatusUpdatesChannel);
			}

			#endregion

			// Start loader animation
			((Storyboard)FindResource("RunLoaderStoryboard")).Begin();

			ValidateLocalCredentials();
		}		

		void ValidateLocalCredentials()
		{
			// Create an instance of the InputChannel and check the provided credentials
			var channel = ChannelBuilder.BuildWithCredentials<IClientInputChannel>(ChannelConfiguration.InputChannel);
			var task = new ValidateLoginTask(ChannelConfiguration, channel) { RedirectResult = PincodeTextBox.Text };			

			task.FinishedSuccess += Task_FinishedSuccess;
			task.FinishedFailure += Task_FinishedFailure;
			task.Finished += Task_Finished;
			task.ExecuteAsync();
		}		

		#region Validate credentials

		void Task_FinishedSuccess(object sender, EventArgs e)
		{
			var task = (IContextTask)sender;

			Thread.CurrentThread.ExecuteOnUIThread(delegate
			{
				// Make sure this channel has not allready been configured (if not in edit mode)
				string username = (ChannelConfiguration.Charasteristics.CanCustomize && IsManuallyCustomized) ? ManualEntryIncomingUsername : Username;
				if (!IsInEditModus && ChannelsManager.Channels.Any(
					c => c.Configuration.DisplayName == ChannelConfiguration.DisplayName && c.Configuration.InputChannel.Authentication.Username == username))
				{
					ClientStats.LogEvent("Check credentials failure - duplicate account (setup)");

					Inbox2MessageBox.Show(Strings.AccountAlreadyAdded, Inbox2MessageBoxButton.OK);
				}
				else
				{
					ClientStats.LogEvent("Check credentials success (setup)");

					IsValidated = true;
					CheckCanvas.Visibility = Visibility.Visible;

					if (IsInEditModus)
					{
						UpdateChannel();
					}
					else
					{
						SaveChannel(task.Values);
					}

					if (OnValidationFinished != null)
					{
						OnValidationFinished(this, EventArgs.Empty);
					}
				}
			});
		}

		void Task_FinishedFailure(object sender, EventArgs e)
		{
			var task = (BackgroundTask)sender;

			Thread.CurrentThread.ExecuteOnUIThread(delegate
			{
				ClientStats.LogEvent("Check credentials failure - general error (setup)");

				Inbox2MessageBox.Show(String.Concat(
					String.Format(Strings.AddingAccountFailed, ChannelConfiguration.DisplayName), 
						", ", task.LastException.Message), Inbox2MessageBoxButton.OK);
				
				FailureCount++;
			});
		}

		void Task_Finished(object sender, EventArgs e)
		{
			var task = (BackgroundTask)sender;

			// Clean up any open connections
			task.Dispose();

			// Clean up event handler roots
			task.FinishedSuccess -= Task_FinishedSuccess;
			task.FinishedFailure -= Task_FinishedFailure;
			task.Finished -= Task_Finished;

			Thread.CurrentThread.ExecuteOnUIThread(delegate
			{
				LoaderCanvas.Visibility = Visibility.Collapsed;
				((Storyboard)FindResource("RunLoaderStoryboard")).Stop();

				IsChecking = false;
			});
		}

		#endregion		

		/// <summary>
		/// Updates the channel.
		/// </summary>
		void UpdateChannel()
		{
			var config = new ChannelConfig
			{
				ChannelConfigId = ChannelConfiguration.ChannelId,
				DisplayName = ChannelConfiguration.DisplayName,
				Hostname = ChannelConfiguration.InputChannel.Hostname,
				Port = ChannelConfiguration.InputChannel.Port,
				Username = ChannelConfiguration.InputChannel.Authentication.Username,
				Password = ChannelConfiguration.InputChannel.Authentication.Password,
				SSL = ChannelConfiguration.InputChannel.IsSecured,
				Type = ChannelConfiguration.InputChannel.TypeSurrogate,
				IsVisible = true,
				IsDefault = ChannelConfiguration.IsDefault,
				IsManuallyCustomized = IsManuallyCustomized,
				DateModified = DateTime.Now
			};

			if (ChannelConfiguration.OutputChannel != null)
			{
				config.OutgoingUsername = ChannelConfiguration.OutputChannel.Authentication.Username;
				config.OutgoingPassword = ChannelConfiguration.OutputChannel.Authentication.Password;
				config.OutgoingHostname = ChannelConfiguration.OutputChannel.Hostname;
				config.OutgoingPort = ChannelConfiguration.OutputChannel.Port;
				config.OutgoingSSL = ChannelConfiguration.OutputChannel.IsSecured;
			}

			ChannelConfiguration.IsCustomized = IsManuallyCustomized;

			ClientState.Current.DataService.Update(config);

			// Ovverride the existing configuration in the channelsmanager
			var instance = ChannelsManager.Channels.First(c => c.Configuration.ChannelId == config.ChannelConfigId);
			instance.OverrideConfiguration(ChannelConfiguration);
		}

		/// <summary>
		/// Saves the channel.
		/// </summary>
		/// <param name="values">The values.</param>
		void SaveChannel(Dictionary<string, object> values)
		{
			var config = new ChannelConfig
			{
				DisplayName = ChannelConfiguration.DisplayName,
				Hostname = ChannelConfiguration.InputChannel.Hostname,
				Port = ChannelConfiguration.InputChannel.Port,
				Username = ChannelConfiguration.InputChannel.Authentication.Username,
				Password = ChannelConfiguration.InputChannel.Authentication.Password,
				SSL = ChannelConfiguration.InputChannel.IsSecured,
				Type = ChannelConfiguration.InputChannel.TypeSurrogate,
				IsVisible = true,
				IsDefault = ChannelConfiguration.IsDefault,
				IsManuallyCustomized = IsManuallyCustomized,
				DateCreated = DateTime.Now
			};

			if (ChannelConfiguration.OutputChannel != null)
			{
				config.OutgoingUsername = ChannelConfiguration.OutputChannel.Authentication.Username;
				config.OutgoingPassword = ChannelConfiguration.OutputChannel.Authentication.Password;
				config.OutgoingHostname = ChannelConfiguration.OutputChannel.Hostname;
				config.OutgoingPort = ChannelConfiguration.OutputChannel.Port;
				config.OutgoingSSL = ChannelConfiguration.OutputChannel.IsSecured;
			}

			config.ChannelConnection = ChannelConnection.Local;

			ClientState.Current.DataService.Save(config);

			// Replace ChannelConfiguration with new channel instance
			ChannelConfiguration = ChannelFactory.Create(config);

			// Save temporary settings to channelcontext
			var context = new ChannelClientContext(ClientState.Current.Context, ChannelConfiguration);
			values.Keys.ForEach(key => context.SaveSetting(key, values[key]));

			var instance = ChannelsManager.Add(ChannelConfiguration);

			if (instance.StatusUpdatesChannel != null)
				EventBroker.Publish(AppEvents.RequestDockChannel, instance);
		}

		#endregion

		#region Event handlers

		/// <summary>
		/// Handles the Loaded event of the UserControl control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			// Set first the IsManuallyCustomized property
			IsManuallyCustomized = ChannelConfiguration.IsCustomized;
			OnPropertyChanged("IsManuallyCustomized");

			// Show Setup Form
			ShowSetupControlForm();

			// Check if we have a pre-channel configured entry
			var preChannels = PreChannels.GetChannels();

			if (preChannels != null)
			{
				var preChannel = preChannels.FirstOrDefault(c => c.Name == ChannelConfiguration.DisplayName);

				if (preChannel != null)
				{
					Username = UsernameTextBox.Text = preChannel.Username;
					Password = PasswordTextBox.Password = preChannel.Password;
					Hostname = HostnameTextBox.Text = preChannel.Hostname;
				}
			}

			// Set Editable Settings
			UsernameTextBox.IsReadOnly = IsInEditModus;
			
			ManualEntryIncomingUsernameTextBox.IsReadOnly = IsInEditModus;
			ManualEntryIncomingServerTextBox.IsReadOnly = IsInEditModus;
			ManualEntryIncomingPortTextBox.IsReadOnly = IsInEditModus;
			ManualEntryIncomingSslCheckBox.IsEnabled = !IsInEditModus;

			ManualEntryOutgoingUsernameTextBox.IsReadOnly = IsInEditModus;
			ManualEntryOutgoingServerTextBox.IsReadOnly = IsInEditModus;
			ManualEntryOutgoingPortTextBox.IsReadOnly = IsInEditModus;
			ManualEntryOutgoingSslCheckBox.IsEnabled = !IsInEditModus;
			
			AccountTypeImapRadioButton.IsEnabled = !IsInEditModus;

			// Bind Other Channels Fields Values
			if (ChannelConfiguration.DisplayStyle == DisplayStyle.Other ||
				ChannelConfiguration.Charasteristics.CanCustomize)
			{
				ManualEntryIncomingUsername = ChannelConfiguration.InputChannel.Authentication.Username;
				ManualEntryIncomingServer = ChannelConfiguration.InputChannel.Hostname;
				ManualEntryIncomingPort = ChannelConfiguration.InputChannel.Port == 0 ? 143 : ChannelConfiguration.InputChannel.Port;
				ManualEntryIncomingSsl = ChannelConfiguration.InputChannel.IsSecured;

				if (ChannelConfiguration.InputChannel.TypeSurrogate != null)
				{
					AccountTypeImapRadioButton.IsChecked = (ChannelConfiguration.InputChannel.Type ==
															typeof(Imap2ClientChannel));
					AccountTypePop3RadioButton.IsChecked = (ChannelConfiguration.InputChannel.Type ==
															typeof(Pop3ClientChannel));
				}

				OnPropertyChanged("ManualEntryIncomingUsername");
				OnPropertyChanged("ManualEntryIncomingServer");
				OnPropertyChanged("ManualEntryIncomingPort");
				OnPropertyChanged("ManualEntryIncomingSsl");

				ManualEntryOutgoingUsername = ChannelConfiguration.OutputChannel.Authentication.Username;
				ManualEntryOutgoingServer = ChannelConfiguration.OutputChannel.Hostname;
				ManualEntryOutgoingPort = ChannelConfiguration.OutputChannel.Port == 0 ? 25 : ChannelConfiguration.OutputChannel.Port;
				ManualEntryOutgoingSsl = ChannelConfiguration.OutputChannel.IsSecured;

				OnPropertyChanged("ManualEntryOutgoingUsername");
				OnPropertyChanged("ManualEntryOutgoingServer");
				OnPropertyChanged("ManualEntryOutgoingPort");
				OnPropertyChanged("ManualEntryOutgoingSsl");
			}
		}

		/// <summary>
		/// Handles the Click event of the Help control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		void Help_Click(object sender, RoutedEventArgs e)
		{
			ClientStats.LogEvent("Check credentials help (setup)");

			var properties = ChannelsManager.GetChannelProperties(ChannelConfiguration.DisplayName);

			new Process { StartInfo = new ProcessStartInfo(properties.HelpUrl) }.Start();
		}

		/// <summary>
		/// Handles the LostFocus event of the UsernameTextBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		void UsernameTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			var usernameTextBox = e.Source as TextBox;

			// See if we need to append the defaultDomain to the current entry
			if (String.IsNullOrEmpty(ChannelConfiguration.DefaultDomain)
				|| String.IsNullOrEmpty(usernameTextBox.Text))
			{
				return;
			}

			if (SourceAddress.IsValidEmail(usernameTextBox.Text))
			{
				return;
			}

			if (usernameTextBox.Text.Contains("@"))
			{
				return;
			}

			usernameTextBox.Text = String.Format("{0}@{1}", usernameTextBox.Text, ChannelConfiguration.DefaultDomain);
		}

		/// <summary>
		/// Handles the PasswordChanged event of the PasswordTextBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			Password = PasswordTextBox.Password;
		}

		/// <summary>
		/// Handles the Click event of the AccountTypeRadioButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		void AccountTypeRadioButton_Click(object sender, RoutedEventArgs e)
		{
			if (AccountTypeImapRadioButton.IsChecked ?? false)
				ManualEntryIncomingPort = ManualEntryIncomingPort == 0 ? 143 : ManualEntryIncomingPort;

			if (AccountTypePop3RadioButton.IsChecked ?? false)
				ManualEntryOutgoingPort = ManualEntryOutgoingPort == 0 ? 25 : ManualEntryOutgoingPort;

			OnPropertyChanged("ManualEntryIncomingPort");
		}

		/// <summary>
		/// Handles the Click event of the CancelButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			ClientStats.LogEvent("Check credentials cancel (setup)");

			if (IsInEditModus)
			{
				if (OnCancel != null)
				{
					OnCancel(this, EventArgs.Empty);
				}
			}
			else
			{
				if (OnCancel != null)
				{
					OnCancel(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Handles the Click event of the ToggleManualSettingsHyperlink control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		void ToggleManualSettingsHyperlink_Click(object sender, RoutedEventArgs e)
		{
			if (!IsManuallyCustomized)
				ClientStats.LogEvent("Toggle manual settings (setup)");

			// Set manually customization
			IsManuallyCustomized = !IsManuallyCustomized;
			OnPropertyChanged("IsManuallyCustomized");

			// Update the setup control form.
			ShowSetupControlForm();
		}

		/// <summary>
		/// Handles the KeyDown event of the CheckCredentials control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
		void CheckCredentials_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				CheckUserInput();
			}
		}

		/// <summary>
		/// Handles the Click event of the CheckCredentialsButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		void CheckCredentialsButton_Click(object sender, RoutedEventArgs e)
		{
			CheckUserInput();
		}

		/// <summary>
		/// Checks the user input.
		/// </summary>
		void CheckUserInput()
		{
			ClientStats.LogEventWithSegment("Check user credentials (setup)", ChannelConfiguration.DisplayName);

			bool isValid = true;

			if (IsValidated && !IsInEditModus)
			{
				return;
			}

			if (IsChecking)
			{
				Inbox2MessageBox.Show("Credentials are being validated.", Inbox2MessageBoxButton.OK);
				return;
			}

			if (ChannelConfiguration == null)
			{
				Inbox2MessageBox.Show("No credentials configured.", Inbox2MessageBoxButton.OK);
				return;
			}

			if (ChannelConfiguration.DisplayStyle == DisplayStyle.Other ||
				(ChannelConfiguration.Charasteristics.CanCustomize && IsManuallyCustomized))
			{
				isValid =
					!(String.IsNullOrEmpty(ManualEntryIncomingUsernameTextBox.Text) ||
					  String.IsNullOrEmpty(ManualEntryIncomingPasswordTextBox.Password) ||
					  String.IsNullOrEmpty(ManualEntryIncomingServerTextBox.Text) ||
					  String.IsNullOrEmpty(ManualEntryIncomingPortTextBox.Text) ||
					  String.IsNullOrEmpty(ManualEntryOutgoingUsernameTextBox.Text) ||
					  String.IsNullOrEmpty(ManualEntryOutgoingPasswordTextBox.Password) ||
					  String.IsNullOrEmpty(ManualEntryOutgoingServerTextBox.Text) ||
					  String.IsNullOrEmpty(ManualEntryOutgoingPortTextBox.Text));
			}
			else if (ChannelConfiguration.DisplayStyle == DisplayStyle.Advanced)
			{
				isValid =
					!(String.IsNullOrEmpty(UsernameTextBox.Text) ||
					  String.IsNullOrEmpty(PasswordTextBox.Password) ||
					  String.IsNullOrEmpty(HostnameTextBox.Text));
			}
			else if (ChannelConfiguration.DisplayStyle == DisplayStyle.Simple)
			{
				isValid =
					!(String.IsNullOrEmpty(UsernameTextBox.Text) ||
					  String.IsNullOrEmpty(PasswordTextBox.Password));
			}
			else if (ChannelConfiguration.DisplayStyle == DisplayStyle.RedirectWithPin)
			{
				isValid =
					(PinGrid.Visibility == Visibility.Visible) ?
					!String.IsNullOrEmpty(PincodeTextBox.Text) :
					true;
			}

			if (!isValid)
			{
				Inbox2MessageBox.Show(Strings.PleaseFillConfigurationCorrectly, Inbox2MessageBoxButton.OK);
			}
			else
			{
				ValidateChannel();
			}
		}

		#endregion
	}
}