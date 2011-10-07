using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Inbox2.Core.Configuration;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Framework.Stats;
using Inbox2.Framework.UI;
using Inbox2.Framework.UI.DragDrop;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Framework.VirtualMailBox.View;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Framework;
using Inbox2.Framework.Localization;
using Inbox2.Platform.Interfaces.Enumerations;
using Label = Inbox2.Framework.VirtualMailBox.Entities.Label;
using LabelsContainer = Inbox2.Framework.VirtualMailBox.Entities.LabelsContainer;

namespace Inbox2.Plugins.Conversations.Controls
{
	/// <summary>
	/// Interaction logic for StreamView.xaml
	/// </summary>
	public partial class StreamView : UserControl, INotifyPropertyChanged
	{
		#region Fields

		public event PropertyChangedEventHandler PropertyChanged;

		private bool isOverviewActive;
		private bool isInitialized;

		private readonly List<Message> selection;
		private readonly VirtualMailBox mailbox = VirtualMailBox.Current;
		private readonly DropHelper dropHelper;
		private readonly ViewFilter viewFilter;
		private readonly Flipper selectionFlipper;

		#endregion

		#region Properties

		public ConversationsPlugin Plugin
		{
			get { return PluginsManager.Current.GetPlugin<ConversationsPlugin>(); }
		}

		public ConversationsState State
		{
			get { return (ConversationsState)Plugin.State; }
		}

		public long Visible
		{
			get
			{
				using (mailbox.Messages.ReaderLock)
					return mailbox.Messages.Count(m => m.IsVisible);
			}
		}

		public bool IsInboxView
		{
			get
			{
				return viewFilter.Filter.CurrentView == ActivityView.MyInbox;
			}
		}

		public bool IsTrashView
		{
			get { return viewFilter.Filter.CurrentView == ActivityView.Trash; }
		}

		#endregion

		#region Constructors

		public StreamView()
		{
			using (new CodeTimer("StreamView/Constructor"))
			{
				InitializeComponent();

				selection = new List<Message>();
				dropHelper = new DropHelper(StreamListView);
				dropHelper.Drop += StreamListView_Drop;
				
				DataContext = this;

				viewFilter = ViewFilter.Current;
				viewFilter.Filter.FilterChanged += State_CurrentViewChanged;

				selectionFlipper = new Flipper(TimeSpan.FromMilliseconds(200),
					() => State.SelectedMessages.ReplaceWithCast(StreamListView.SelectedItems));

				EventBroker.Subscribe(AppEvents.RebuildOverview, 
					() => Thread.CurrentThread.ExecuteOnUIThread(CreateChannelsSelection));

				VirtualMailBox.Current.InboxLoadComplete += delegate
					{
						using (new ThreadFlag())
							StreamListView.SelectedIndex = 0;
					};
				
				EventBroker.Subscribe(AppEvents.TabChanged, delegate(string newTab)
					{
						if (newTab == "DockControl")
						{
							// Switched to overview
							isOverviewActive = true;

							if (selection.Count > 0)
								State.SelectedMessages.Replace(selection);
						}
						else
						{
							// Switched to any other tab
							if (isOverviewActive)
							{
								// Save selection
								selection.Clear();
								selection.AddRange(State.SelectedMessages);
							}

							isOverviewActive = false;
						}
					});                
			}
		}		

		#endregion

		#region Methods	

		void CreateChannelsSelection()
		{
			var index = AccountsComboBox.SelectedIndex;
			if (index < 0) index = 0;

			AccountsComboBox.Items.Clear();
			AccountsComboBox.Items.Add(Strings.ViewAllAccounts);

			foreach (var channel in ChannelsManager.Channels)
				AccountsComboBox.Items.Add(channel);

			if (index > AccountsComboBox.Items.Count - 1)
				index = AccountsComboBox.Items.Count - 1;

			AccountsComboBox.SelectedIndex = index;
		}

		void CreatePluginViews()
		{
			var views =
				PluginsManager.Current.Plugins
					.Where(p => p.StreamViews != null)
					.SelectMany(p => p.StreamViews)
					.ToList();

			foreach (var view in views)
			{
				// Create radio button to switch to this view
				var button = new RadioButton
				{
					GroupName = "StreamViewToggle",
					Content = view.Header,
					Style = (Style)FindResource("SwitcherToggleIconRadio")
				};

				IStreamViewPlugin view1 = view;
				button.Click += delegate
				{
					SettingsManager.ClientSettings.AppConfiguration.DefaultView = view1.Header;
					SettingsManager.Save();

					view1.SwitchToView();
				};
				button.DataContext = view;
				button.SetBinding(IsEnabledProperty, "CanSwitchToView");

				StreamViewsRadioButtons.Children.Add(button);				
			}
		}

		void UpdateUISelection()
		{
			bool found = false;

			// Select the last saved stream view (if any)
			foreach (var checkbox in StreamViewsRadioButtons.Children.OfType<RadioButton>())
			{
				var plugin = (IStreamViewPlugin)checkbox.DataContext;

				if (plugin.Header.Equals(SettingsManager.ClientSettings.AppConfiguration.DefaultView))
				{					
					checkbox.IsChecked = found = true;
					plugin.SwitchToView();
					break;
				}
			}

			// Not found, select first streamview by default
			if (found == false)
			{
				// Select first streamview by default
				StreamViewsRadioButtons.Children.OfType<RadioButton>().First().IsChecked = true;
			}
		}

		internal void DeleteStreamSelection()
		{
			using (new ListViewIndexFix(StreamListView))
			{
				// Outlook style
				if (!SettingsManager.ClientSettings.AppConfiguration.RollUpConversations)
				{
					State.Delete();

					return;
				}

				// Single line view
				if (!viewFilter.Filter.IsActivityViewVisible)
				{
					State.Delete(true);

					return;
				}

				if (State.SelectedMessages.Any(m => m.Conversation.Messages.Count > 1))
				{
					Inbox2MessageBoxResult result = SettingsManager.SettingOrDefault("/Settings/Dialogs/DeleteConversation", Inbox2MessageBoxResult.None);

					if (result == Inbox2MessageBoxResult.None)
					{
						var wrapper = Inbox2MessageBox.Show(Strings.AlsoDeleteConversations, Inbox2MessageBoxButton.Conversation, Strings.DoNotAskAgain);

						if (wrapper.Result == Inbox2MessageBoxResult.Cancel)
							return;

						// Save setting
						if (wrapper.DoNotAskAgainResult)
							ClientState.Current.Context.SaveSetting("/Settings/Dialogs/DeleteConversation", wrapper.Result);

						result = wrapper.Result;
					}

					if (result == Inbox2MessageBoxResult.Conversation)
					{
						State.Delete(true);
						return;
					}
				}

				// No need to ask or just delete single message
				State.Delete();
			}
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region Command handlers

		void EmptyTrash_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Empty trash");

			if (Inbox2MessageBox.Show(Strings.SureYouWantToEmptyTrash, Inbox2MessageBoxButton.YesNo).Result == Inbox2MessageBoxResult.Yes)
			{
				List<Message> trashed;

				using (mailbox.Messages.ReaderLock)
					trashed = mailbox.Messages.Where(m => m.IsTrash).ToList();

				trashed.ForEach(m => m.Purge());

				viewFilter.RebuildCurrentViewAsync();

				EventBroker.Publish(AppEvents.RequestReceive);
			}
		}

		void EmptyTrash_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		#endregion

		#region Event handlers

		void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			if (!isInitialized)
			{
				CreateChannelsSelection();
				CreatePluginViews();
				UpdateUISelection();				

				isInitialized = true;
			}
		}

		void StreamListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			selectionFlipper.Delay();
		}

		void AccountsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.RemovedItems.Count > 0)
			{
				ClientStats.LogEvent("Toggle channel (streamview)");

				var instance = AccountsComboBox.SelectedItem as ChannelInstance;

				foreach (var channel in ChannelsManager.Channels)
					channel.IsVisible = instance == null;

				if (instance != null)
					instance.IsVisible = true;

				viewFilter.RebuildCurrentViewAsync();

				EventBroker.Publish(AppEvents.RequestFocus);
			}
		}

		void StreamListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			ClientStats.LogEvent("View message in stream (doubleclick)");

			var message = (e.OriginalSource as DependencyObject)
				.FindListViewItem<Message>(((ListView)sender).ItemContainerGenerator);

			if (message != null)
			{
				EventBroker.Publish(AppEvents.View, message);
			}
		}

		void StreamListView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
			{
				ClientStats.LogEvent("Delete message in stream (keyboard)");

				DeleteStreamSelection();
			}
			else if (e.Key == Key.Enter)
			{
				ClientStats.LogEvent("View message in stream (keyboard)");

				State.View();
			}
		}

		void StreamListView_Drop(object sender, DropEventArgs e)
		{
			ClientStats.LogEvent("Drop label in stream");

			var message = (Message)StreamListView.ItemContainerGenerator.ItemFromContainer(e.Target);

			if (e.Data as LabelsContainer != null)
			{
				var newLabel = (LabelsContainer) e.Data;				
				message.AddLabel(new Label(newLabel.Labelname));
			}
			
			// For fixed labels
			if (e.Source as RadioButton != null)
			{
				var control = (RadioButton)e.Source;
				var view = FoldersControl.GetActivityView(control);

				switch (view)
				{
					case ActivityView.Todo:
						message.AddLabel(new Label(LabelType.Todo));
						break;
					case ActivityView.WaitingFor:
						message.AddLabel(new Label(LabelType.WaitingFor));
						break;
					case ActivityView.Someday:
						message.AddLabel(new Label(LabelType.Someday));
						break;
				}
			}
		}

		void State_CurrentViewChanged(object sender, EventArgs e)
		{
			SelectedFolderButton.Content = viewFilter.Filter.CurrentView.ToString();

			using (new ThreadFlag())
				StreamListView.SelectedIndex = 0;

			OnPropertyChanged("IsTrashView");

			TodoHelpText.Visibility = Visibility.Collapsed;
			WaitingForHelpText.Visibility = Visibility.Collapsed;
			SomedayHelpText.Visibility = Visibility.Collapsed;

			// count the nr of visible items
			var obj = FindName(viewFilter.Filter.CurrentView + "HelpText");

			if (obj != null && Visible == 0)
				((FrameworkElement)obj).Visibility = Visibility.Visible;
		}

		#endregion        		
	}
}
