using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FluidKit.Controls;
using Inbox2.Core.Configuration.Channels;
using Inbox2.Core.Threading.Tasks.Application;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Threading;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.UI.Controls.Setup;
using Inbox2.Framework.Localization;

namespace Inbox2.UI.Controls.Options
{
	/// <summary>
	/// Interaction logic for AccountOptionsControl.xaml
	/// </summary>
	public partial class AccountOptionsControl : UserControl, INotifyPropertyChanged
	{
		#region Fields

		public event PropertyChangedEventHandler PropertyChanged;

		private readonly List<ChannelConfiguration> channelsToDelete = new List<ChannelConfiguration>();
		private readonly VirtualMailBox mailbox = VirtualMailBox.Current;

		#endregion

		#region Properties

		public IEnumerable<ChannelConfiguration> ChannelConfigurations
		{
			get
			{
				return ChannelsManager.Channels.Select(s => s.Configuration);
			}
		}

		#endregion

		#region Construction

		public AccountOptionsControl()
		{
			InitializeComponent();

			DataContext = this;
		}

		#endregion

		#region Methods

		public bool TrySave()
		{
			bool failure = false;

			foreach (var config in channelsToDelete)
			{
				var task = new RemoveChannelTask(config);

				task.FinishedFailure += delegate { failure = true; };
				task.Finished += delegate { Thread.CurrentThread.ExecuteOnUIThread(() => ClientState.Current.ViewController.HidePopup()); };
				task.ExecuteAsync();

				ClientState.Current.ViewController.ShowPopup(new ModalWaitControl { Message = String.Format(Strings.RemovingAccount, config.InputChannel.Authentication.Username) });

				if (failure)
				{
					Inbox2MessageBox.Show(
						String.Format(Strings.AnErrorHasOccuredWhileDeletingAccount, config.InputChannel.Authentication.Username),
						Inbox2MessageBoxButton.OK);

					return false;
				}							
			}

			OnPropertyChanged("ChannelConfigurations");

			return true;
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			foreach (var channel in new ChannelLoadHelper().AvailableChannels)
			{
				// Insert each item at index 0 of the wrap panel
				ChannelAddControl control = new ChannelAddControl();

				control.ChannelConfiguration = channel;

				ChannelAddWrapPannel.Children.Insert(0, control);
			}
		}

		/// <summary>
		/// Edits the channel.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		void EditChannel(ChannelConfiguration configuration)
		{
			if (configuration != null)
			{
				ChannelSetupControlPanel.Children.Clear();

				// Setup Control
				ChannelSetupControl setupControl = new ChannelSetupControl(configuration.Clone());
				setupControl.IsInEditModus = true;
				setupControl.Username = configuration.InputChannel.Authentication.Username;
				setupControl.HorizontalAlignment = HorizontalAlignment.Center;
				setupControl.VerticalAlignment = VerticalAlignment.Center;
				setupControl.OnValidationFinished += OnValidationFinishedHandler;
				setupControl.OnCancel += OnCancelHandler;
				setupControl.RenderTransform = new TranslateTransform(0, 0);

				ChannelSetupControlPanel.Children.Add(setupControl);

				// Play Animation
				var firstItem = (FrameworkElement)transitionContainer.Items[0];
				var secondItem = (FrameworkElement)transitionContainer.Items[1];

				FlipTransition transition = Resources["FlipTransition"] as FlipTransition;
				transition.Rotation = Direction.RightToLeft;
				transitionContainer.Transition = transition;
				transitionContainer.TransitionCompleted -= OnTransitionCompleted;
				transitionContainer.ApplyTransition(firstItem, secondItem);

				OnPropertyChanged("AllChannelConfigurations");
				OnPropertyChanged("ChannelConfiguration");
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
				if (!control.IsInEditModus)
				{
					ChannelConfigurationsListView.ItemsSource = ChannelConfigurations;
				}

				// Play Animation
				var frontItem = (FrameworkElement)transitionContainer.Items[1];
				var backItem = (FrameworkElement)transitionContainer.Items[0];

				FlipTransition transition = Resources["FlipTransition"] as FlipTransition;
				transition.Rotation = Direction.RightToLeft;
				transitionContainer.Transition = transition;
				transitionContainer.TransitionCompleted += OnTransitionCompleted;
				transitionContainer.ApplyTransition(frontItem, backItem);

				// Start recieve with default page size
				EventBroker.Publish(AppEvents.RequestReceive, ChannelsManager.Channels.First(c => c.Configuration.ChannelId == control.ChannelConfiguration.ChannelId));
			}

			OnPropertyChanged("AllChannelConfigurations");
			OnPropertyChanged("ChannelConfiguration");
		}

		/// <summary>
		/// Called when [cancel handler].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void OnCancelHandler(object sender, EventArgs e)
		{
			var frontItem = (FrameworkElement)transitionContainer.Items[1];
			var backItem = (FrameworkElement)transitionContainer.Items[0];

			FlipTransition transition = Resources["FlipTransition"] as FlipTransition;
			transition.Rotation = Direction.RightToLeft;
			transitionContainer.Transition = transition;
			transitionContainer.TransitionCompleted += OnTransitionCompleted;
			transitionContainer.ApplyTransition(frontItem, backItem);

			OnPropertyChanged("AllChannelConfigurations");
			OnPropertyChanged("ChannelConfiguration");
		}

		/// <summary>
		/// Called when [transition completed].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void OnTransitionCompleted(object sender, EventArgs e)
		{
			ChannelSetupControlPanel.Children.Clear();
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

		#endregion

		#region Event handlers

		/// <summary>
		/// Handles the MouseLeftButtonUp event of the ChannelConfigurationsListView control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
		void ChannelConfigurationsListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			var configuration =
				(e.OriginalSource as DependencyObject).FindListViewItem<ChannelConfiguration>(
					ChannelConfigurationsListView.ItemContainerGenerator);

			EditChannel(configuration);
		}

		/// <summary>
		/// Handles the MouseDoubleClick event of the ChannelConfigurationsListView control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
		void ChannelConfigurationsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var configuration =
				(e.OriginalSource as DependencyObject).FindListViewItem<ChannelConfiguration>(
					ChannelConfigurationsListView.ItemContainerGenerator);

			EditChannel(configuration);
		}

		/// <summary>
		/// Handles the Click event of the ChannelIcon control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		void ChannelIcon_Click(object sender, RoutedEventArgs e)
		{
			var configuration =
				(e.OriginalSource as DependencyObject).FindListViewItem<ChannelConfiguration>(
					ChannelConfigurationsListView.ItemContainerGenerator);

			EditChannel(configuration);
		}

		/// <summary>
		/// Handles the Click event of the ChannelDeleteButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		void ChannelDeleteButton_Click(object sender, RoutedEventArgs e)
		{
			var configuration =
				(e.OriginalSource as DependencyObject).FindListViewItem<ChannelConfiguration>(
					ChannelConfigurationsListView.ItemContainerGenerator);

			Inbox2MessageBoxResultWrapper result;

			if (configuration.IsConnected)
			{
				// Cloud
				result = Inbox2MessageBox.Show(String.Concat(Strings.AreYouSureYouWantToRemoveThisAccount, Strings.AccountWillAlsoBeRemovedFromCloud), Inbox2MessageBoxButton.YesNo);				
			}
			else
			{
				// Local
				result = Inbox2MessageBox.Show(Strings.AreYouSureYouWantToRemoveThisAccount, Inbox2MessageBoxButton.YesNo);
			}

			if (result.Result == Inbox2MessageBoxResult.Yes)
			{
				channelsToDelete.Add(configuration);

				var configurations = ChannelConfigurationsListView.ItemsSource as IEnumerable<ChannelConfiguration>;
				configurations = configurations.Where(c => c.ChannelId != configuration.ChannelId);
				ChannelConfigurationsListView.ItemsSource = configurations;

				OnPropertyChanged("AllChannelConfigurations");
				OnPropertyChanged("ChannelConfiguration");
			}
		}

		/// <summary>
		/// Handles the Click event of the ChannelAddWrapPannel control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		void ChannelAddWrapPannel_Click(object sender, RoutedEventArgs e)
		{
			if (ChannelSetupControlPanel.Children.Count == 0)
			{
				ChannelAddControl channelAddControl = e.Source as ChannelAddControl;

				// Setup Control
				ChannelSetupControl setupControl = new ChannelSetupControl(channelAddControl.ChannelConfiguration.Clone());
				setupControl.IsInEditModus = false;
				setupControl.HorizontalAlignment = HorizontalAlignment.Center;
				setupControl.VerticalAlignment = VerticalAlignment.Center;
				setupControl.OnValidationFinished += OnValidationFinishedHandler;
				setupControl.OnCancel += OnCancelHandler;
				setupControl.RenderTransform = new TranslateTransform(0, 0);

				ChannelSetupControlPanel.Children.Add(setupControl);

				// Play Animation
				var firstItem = (FrameworkElement)transitionContainer.Items[0];
				var secondItem = (FrameworkElement)transitionContainer.Items[1];

				FlipTransition transition = Resources["FlipTransition"] as FlipTransition;
				transition.Rotation = Direction.RightToLeft;
				transitionContainer.Transition = transition;
				transitionContainer.TransitionCompleted -= OnTransitionCompleted;
				transitionContainer.ApplyTransition(firstItem, secondItem);
			}

			OnPropertyChanged("AllChannelConfigurations");
			OnPropertyChanged("ChannelConfiguration");

			e.Handled = true;
		}

		#endregion
	}
}
