using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using FluidKit.Controls;
using Inbox2.Core.Configuration;
using Inbox2.Core.Configuration.Channels;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Threading;
using Inbox2.Framework.UI.Animation;
using Inbox2.Framework.Utils.Viral;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.UI.Controls.Setup
{
	/// <summary>
	/// Interaction logic for ChannelSetupPageControl.xaml
	/// </summary>
	public partial class ChannelSetupPageControl : UserControl, INotifyPropertyChanged
	{
		#region Fields

		private bool IsPreAnimated { get; set; }
		private bool ChannelAddWrapAnimated { get; set; }    	

		#endregion

		#region Properties

		public string FullName { get; set; }
		public event PropertyChangedEventHandler PropertyChanged;
		public static RoutedCommand CheckFinish = new RoutedCommand();
		public bool HasConfiguredChannels
		{
			get { return (ChannelsManager.Channels.Count > 0); }
		}
		public IEnumerable<ChannelConfiguration> ChannelConfiguration
		{
			get
			{
				if (ChannelsManager.Channels.Select(s => s.Configuration).Where(c => c.Charasteristics.SupportsEmail).Count() > 0)
				{
					return ChannelsManager.Channels.Select(s => s.Configuration).Where(c => c.Charasteristics.SupportsEmail);
				}
				else
				{
					return ChannelsManager.Channels.Select(s => s.Configuration);
				}
			}
		}

		#endregion

		#region Constructor

		public ChannelSetupPageControl()
		{
			InitializeComponent();

			DataContext = this;
		}

		#endregion

		#region Methods

		public void FadeIn()
		{
			var sb = (Storyboard)FindResource("sb");

			sb.Begin(this);
		}

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
		/// Pres the animate.
		/// </summary>
		/// <remarks>This is workaround to get the genie effect to work properly.</remarks>
		void PreAnimate()
		{
			if (!IsPreAnimated)
			{
				// Empty Control
				Control emptyControl1 = new Control();
				emptyControl1.HorizontalAlignment = HorizontalAlignment.Stretch;
				transitionContainer.Items.Add(emptyControl1);

				// Empty Control
				Control emptyControl2 = new Control();
				emptyControl2.HorizontalAlignment = HorizontalAlignment.Stretch;

				TimeSpan duration = new TimeSpan(0, 0, 0);
				transitionContainer.RestDuration = new Duration(duration);
				transitionContainer.Items.Add(emptyControl2);

				transitionContainer.ApplyTransition(emptyControl1, emptyControl2);
			}
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
			// Create ChannelAdd Buttons
			foreach (var channel in new ChannelLoadHelper().AvailableChannels)
			{
				// Insert each item at index 0 of the wrap panel
				ChannelAddControl control = new ChannelAddControl();

				control.ChannelConfiguration = channel;

				ChannelAddWrapPannel.Children.Insert(0, control);
			}

			GenieTransition transition = Resources["GenieTransition"] as GenieTransition;
			transitionContainer.Transition = transition;
			transitionContainer.TransitionCompleted += TransitionCompleted_Handler;

			if (HasConfiguredChannels)
			{
				AnimateChannelAddWrapPanel(null);
			}

			OnPropertyChanged("HasConfiguredChannels");
		}

		/// <summary>
		/// Handles the Click event of the ChannelAddWrapPannel control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		void ChannelAddWrapPannel_Click(object sender, RoutedEventArgs e)
		{
			ChannelAddControl channelAddControl = e.Source as ChannelAddControl;

			if (!ChannelAddWrapAnimated)
			{
				AnimateChannelAddWrapPanel(channelAddControl);
			}
			else
			{
				OnChannelAddAnimationComplete(channelAddControl);
			}

			e.Handled = true;
		}

		private void AnimateChannelAddWrapPanel(ChannelAddControl channelAddControl)
		{
			PennerDoubleAnimation.Equations equation = PennerDoubleAnimation.Equations.QuintEaseOut;
			double from = Canvas.GetLeft(ChannelConfigurationAddStackPanel);
			double to = 0;
			int durationMS = 1000;

			Animator.AnimatePenner(ChannelConfigurationAddStackPanel, Canvas.LeftProperty, equation, from, to, durationMS, 
				delegate 
				{
				   ChannelAddWrapAnimated = true;
				   if (channelAddControl != null)
				   {
					   OnChannelAddAnimationComplete(channelAddControl);
				   }
				   AnimateOpenedLogo();
			   });
		}

		/// <summary>
		/// Called when [channel add animation complete].
		/// </summary>
		/// <param name="channelAddControl">The channel add control.</param>
		private void OnChannelAddAnimationComplete(ChannelAddControl channelAddControl)
		{
			if (transitionContainer.Items.Count == 0)
			{
				// Set Canvas Top
				if (channelAddControl.ChannelConfiguration.DisplayStyle == DisplayStyle.Other)
				{
					Canvas.SetTop(ChannelSetupStackPanel, 0);
				}
				else
				{
					Canvas.SetTop(ChannelSetupStackPanel, 110);
				}

				// Tranisition Settings
				transitionContainer.Opacity = 0;

				// Setup Control
				ChannelSetupControl setupControl = new ChannelSetupControl(channelAddControl.ChannelConfiguration.Clone());
				setupControl.IsInEditModus = false;
				setupControl.OnValidationFinished += OnValidationFinishedHandler;
				setupControl.OnCancel += OnCancelHandler;
				setupControl.RenderTransform = new TranslateTransform(0, 0);
				setupControl.OnFormLayoutUpdated += 
					delegate
					{
						// Set Canvas Top
						if (setupControl.ChannelConfiguration.DisplayStyle == DisplayStyle.Other || 
							setupControl.IsManuallyCustomized)
						{
							Canvas.SetTop(ChannelSetupStackPanel, 0);
						}
						else
						{
							Canvas.SetTop(ChannelSetupStackPanel, 110);
						}
					};

				transitionContainer.Items.Add(setupControl);

				// Empty Control
				Control emptyControl = new Control();
				emptyControl.HorizontalAlignment = HorizontalAlignment.Stretch;
				emptyControl.VerticalAlignment = VerticalAlignment.Bottom;
				emptyControl.RenderTransform = new TranslateTransform(0, 0);

				transitionContainer.Items.Add(emptyControl);

				// NOTE: This is a workaround to get the focus 
				// correctly on the SetupControl. This way the command binding 
				// on the CheckCredentials button is triggerd correctly.
				emptyControl.Focus();
				setupControl.Focus();

				// Animate Opacity Tween
				PennerDoubleAnimation.Equations equation = PennerDoubleAnimation.Equations.QuintEaseOut;
				int durationMS = 750;
				Animator.AnimatePenner(transitionContainer, OpacityProperty, equation, transitionContainer.Opacity, 1, durationMS, delegate { });
			}

			OnPropertyChanged("HasConfiguredChannels");
		}

		/// <summary>
		/// Animates the logo.
		/// </summary>
		private void AnimateOpenedLogo()
		{
			PennerDoubleAnimation.Equations equation = PennerDoubleAnimation.Equations.QuintEaseOut;

			double from = Canvas.GetTop(Inbox2SetupOpenedLogoCanvas);
			double to = Canvas.GetTop(Inbox2SetupOpenedLogoCanvas) - 320;
			int durationMS = 1200;

			Animator.AnimatePenner(Inbox2SetupOpenedLogoCanvas, OpacityProperty, equation, Inbox2SetupOpenedLogoCanvas.Opacity, 1, durationMS, delegate { });
			Animator.AnimatePenner(Inbox2SetupOpenedLogoCanvas, Canvas.TopProperty, equation, from, to, durationMS, delegate { });
		}

		/// <summary>
		/// Handles the Click event of the ChannelSetupPageNextButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		void ChannelSetupPageNextButton_Click(object sender, RoutedEventArgs e)
		{
			AnimateClosedLogo();
		}

		/// <summary>
		/// Animates the closed logo.
		/// </summary>
		private void AnimateClosedLogo()
		{
			Canvas.SetTop(Inbox2SetupClosedLogoCanvas, Canvas.GetTop(Inbox2SetupOpenedLogoCanvas));

			PennerDoubleAnimation.Equations equation = PennerDoubleAnimation.Equations.QuintEaseOut;
			int durationMS = 1000;

			// Hide Opened Logo
			Animator.AnimatePenner(Inbox2SetupOpenedLogoCanvas, OpacityProperty, equation, Inbox2SetupOpenedLogoCanvas.Opacity, 0, durationMS, delegate { Inbox2SetupOpenedLogoCanvas.Visibility = Visibility.Hidden; });

			// Show Closed Logo
			Animator.AnimatePenner(Inbox2SetupClosedLogoCanvas, OpacityProperty, equation, 0, 1, durationMS, delegate
			{
				Animator.AnimatePenner(Inbox2SetupClosedImage, WidthProperty, equation, Inbox2SetupClosedImage.Width, Inbox2SetupClosedImage.Width / 2, durationMS, delegate { });
				Animator.AnimatePenner(Inbox2SetupClosedImage, HeightProperty, equation, Inbox2SetupClosedImage.Height, Inbox2SetupClosedImage.Height / 2, durationMS,
					delegate
					{
						Animator.AnimatePenner(ChannelConfigurationAddStackPanel, OpacityProperty, equation, null, 0, durationMS, delegate { ChannelConfigurationAddStackPanel.Visibility = Visibility.Hidden; });
						Animator.AnimatePenner(Inbox2SetupClosedLogoCanvas, Canvas.LeftProperty, equation, null, 0, durationMS, delegate { ChannelSetupFinishStackPanel.Visibility = System.Windows.Visibility.Visible; });
					});
			});

			OnPropertyChanged("ChannelConfiguration");

			// Setup form to complete setup.
			ChannelsManager.Channels[0].Configuration.IsDefault = true;
			DefaultEmailAddressComboBox.SelectedIndex = 0;
			if (ChannelsManager.Channels.Count <= 1 ||
				ChannelsManager.Channels.Select(s => s.Configuration).Where(c => c.Charasteristics.SupportsEmail).Count() == 0)
			{
				DefaultEmailAddressTextBlock.Visibility = Visibility.Collapsed;
				DefaultEmailAddressComboBox.Visibility = Visibility.Collapsed;
			}

			var address = ChannelsManager.GetDefaultSourceAddress();

			if (address.ToString().Contains("@"))
			{
				string[] parts = address.ToString().Split('@');

				DisplayNameTextBox.Text = char.ToUpper(parts[0][0]) + parts[0].Substring(1);
			}
			else
			{
				DisplayNameTextBox.Text = address.DisplayName;
			}
			DisplayNameTextBox.Focus();
		}

		/// <summary>
		/// Handles the Click event of the ChannelSetupFinishButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		void ChannelSetupFinishButton_Click(object sender, RoutedEventArgs e)
		{
			bool isValid = true;

			if (String.IsNullOrEmpty(DisplayNameTextBox.Text.Trim()))
			{
				isValid = false;
				Inbox2MessageBox.Show("Please enter your full name", Inbox2MessageBoxButton.OK);
			}

			if (HasConfiguredChannels && isValid)
			{
				// Save Default Channel
				var defaultChannel = DefaultEmailAddressComboBox.SelectedItem as ChannelConfiguration;
				defaultChannel.IsDefault = true;
				ClientState.Current.DataService.ExecuteNonQuery(
							String.Format("update ChannelConfigs set IsDefault='{0}' where ChannelConfigId={1}",
								defaultChannel.IsDefault, defaultChannel.ChannelId));

				// Do nothing if none of the channels has been validated
				SettingsManager.ClientSettings.AppConfiguration.Fullname = DisplayNameTextBox.Text;
				SettingsManager.ClientSettings.AppConfiguration.IsChannelSetupFinished = true;
				SettingsManager.Save();

				// Start receiving data on our newly initialized channels
				EventBroker.Publish(AppEvents.RequestReceive);
				EventBroker.Publish(AppEvents.RequestSync);

				// Run startup logic again
				((ViewController)ClientState.Current.ViewController).Startup();
			}
		}

		/// <summary>
		/// Handles the KeyDown event of the DisplayNameTextBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
		private void DisplayNameTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				ChannelSetupFinishButton_Click(null, null);
			}
		}

		/// <summary>
		/// Called when [validation finished handler].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void OnValidationFinishedHandler(object sender, EventArgs e)
		{
			ChannelSetupControl control = sender as ChannelSetupControl;

			if (control.IsValidated)
			{
				if (control.InviteFriends)
				{
					// Send out viral
					var message = Messages.Next();
					var config = control.ChannelConfiguration;

					var status = new UserStatus
					{
						Status = message,
						StatusType = StatusTypes.MyUpdate,
						SortDate = DateTime.Now,
						DateCreated = DateTime.Now,
						TargetChannelId = config.ChannelId.ToString()
					};

					new BackgroundActionTask(delegate
						{
							var channel = ChannelsManager.GetChannelObject(config.ChannelId);

							ChannelContext.Current.ClientContext =
								new ChannelClientContext(ClientState.Current.Context, config);

							channel.StatusUpdatesChannel.UpdateMyStatus(status.DuckCopy<ChannelStatusUpdate>());

						}).ExecuteAsync();
				}

				var firstItem = (FrameworkElement)transitionContainer.Items[0];
				var secondItem = (FrameworkElement)transitionContainer.Items[1];

				TimeSpan duration = new TimeSpan(0, 0, 0);
				transitionContainer.RestDuration = new Duration(duration);
				transitionContainer.ApplyTransition(firstItem, secondItem);
			}

			OnPropertyChanged("HasConfiguredChannels");
		}

		/// <summary>
		/// Called when [cancel handler].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void OnCancelHandler(object sender, EventArgs e)
		{
			transitionContainer.Items.Clear();

			OnPropertyChanged("HasConfiguredChannels");
		}

		/// <summary>
		/// Handles the Handler event of the TransitionCompleted control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void TransitionCompleted_Handler(object sender, EventArgs e)
		{
			IsPreAnimated = true;

			transitionContainer.Items.Clear();

			OnPropertyChanged("HasConfiguredChannels");
		}

		/// <summary>
		/// Handles the MouseMove event of the UserControl control.
		/// </summary>
		/// <remarks>This is workaround to get the genie effect to work properly.</remarks>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
		void UserControl_MouseMove(object sender, MouseEventArgs e)
		{
			PreAnimate();
		}

		/// <summary>
		/// Handles the KeyDown event of the UserControl control.
		/// </summary>
		/// <remarks>This is workaround to get the genie effect to work properly.</remarks>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
		void UserControl_KeyDown(object sender, KeyEventArgs e)
		{
			PreAnimate();
		}

		#endregion
	}
}
