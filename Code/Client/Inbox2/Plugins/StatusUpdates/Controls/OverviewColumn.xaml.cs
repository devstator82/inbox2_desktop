using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Inbox2.Core.Configuration;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Plugins.SharedControls;
using Inbox2.Framework.Stats;
using Inbox2.Framework.UI;
using Inbox2.Framework.UI.Controls;
using Inbox2.Framework.Utils.Viral;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Plugins.StatusUpdates.Helpers;
using Inbox2.UI.Resources;
using System.Globalization;
using System.Windows.Data;
using Inbox2.Framework.Localization;
using TabItem = Inbox2.Framework.UI.Controls.TabItem;

namespace Inbox2.Plugins.StatusUpdates.Controls
{
	/// <summary>
	/// Interaction logic for OverviewColumn.xaml
	/// </summary>
    public partial class OverviewColumn : UserControl, IValueConverter, INotifyPropertyChanged
	{
		#region Fields

		public event PropertyChangedEventHandler PropertyChanged;

        public static RoutedCommand ToggleChannel = new RoutedCommand();

		#endregion

		#region Properties

        public List<long> SelectedChannels { get; private set; }

		public UserStatus ReplyTo { get; private set; }

		public ChannelInstance SelectedChannel
		{
			get
			{
				var ti = StreamsTab.SelectedItem as OverviewColumnTabItem;

				return ti == null ? null : ti.Channel;
			}
		}

        public IEnumerable<ChannelInstance> AllStatusChannels
        {
            get
            {
                return ChannelsManager.GetStatusChannels();
            }
        }

		public IEnumerable<ChannelConfiguration> UniqueStatusChannels
		{
			get
			{
				return ChannelsManager.GetUniqueStatusChannels().Select(s => s.Configuration);
			}
		}

		public bool HasStatusChannels
		{
			get { return ChannelsManager.GetStatusChannels().Any(); }
		}

		public bool HasTwitter
		{
			get
			{
				return SelectedChannels
					.Select(c => ChannelsManager.GetChannelObject(c))
					.Where(c => c != null)
					.Any(c => c.Configuration.DisplayName == "Twitter");
			}
		}

		public bool ShowViral { get; private set; }		

		#endregion

		#region Construction

		public OverviewColumn()
		{
            SelectedChannels = new List<long>();

            Resources.Add("IsCheckedConverter", this);

			InitializeComponent();

			DataContext = this;			

			EventBroker.Subscribe(AppEvents.RebuildToolbar, () => Thread.CurrentThread.ExecuteOnUIThread(BuildTabs));
		}

		#endregion

		#region Methods

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Action)BuildTabs);

			LoadSavedSelection();

			if (SettingsManager.ClientSettings.AppConfiguration.IsFirstStatusUpdate)
			{
				ShowViral = true;

				PART_StatusUpdateTextbox.Text = Messages.Next();
			}
		}

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            var channel = (ChannelInstance)value;

            return SelectedChannels.Contains(channel.Configuration.ChannelId);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

		void ToggleChanneIndex(int index)
		{
			var channels = AllStatusChannels.ToList();

			if (index <= channels.Count)
			{
				var channel = channels[index - 1];

				ToggleChannelInstance(channel);
			}
		}

		void ToggleChannelInstance(ChannelInstance channel)
		{
			if (SelectedChannels.Contains(channel.Configuration.ChannelId))
			{
				SelectedChannels.Remove(channel.Configuration.ChannelId);
			}
			else
			{
				SelectedChannels.Add(channel.Configuration.ChannelId);
			}

			// Forces a propertychanged event, which resets our toggle button
			channel.IsVisible = channel.IsVisible;

			OnPropertyChanged("HasTwitter");
		}

        void UpdateStatusUpdaterVisibility(bool gotfocus)
        {
            var visibility = gotfocus || PART_StatusUpdateTextbox.Text.Length > 0 || ReplyTo != null ? Visibility.Visible : Visibility.Collapsed;

            wp.Visibility = visibility;
            sp.Visibility = visibility;
        }

		void BuildTabs()
		{
			OnPropertyChanged("HasStatusChannels");
			OnPropertyChanged("HasDockedChannels");

			StreamsTab.Items.Clear();

			var allItemsTabItem = new OverviewColumnTabItem(null, String.Empty)
				{
					Style = (Style)FindResource("StatusUpdatesColumnAllItemsTabItem"),
					Icon = new BitmapImage(new Uri("pack://application:,,,/Inbox2.UI.Resources;component/icons/icon-inbox2.png"))
				};

			StreamsTab.Items.Add(allItemsTabItem);

			foreach (var channel in ChannelsManager.GetStatusChannels())
			{
				if (channel.IsVisible)
				{
					var tabItem = new OverviewColumnTabItem(channel, String.Empty)
						{
							Style = (Style)FindResource("StatusUpdatesColumnTabItem"),
							Icon = (BitmapSource)new ChannelIconConverter()
								.Convert(channel.Configuration, typeof(BitmapSource), 13, Thread.CurrentThread.CurrentCulture)
						};

					StreamsTab.Items.Add(tabItem);
				}
			}

			if (SearchKeywordsHelper.HasSearchChannel())
			{
				var channel = SearchKeywordsHelper.GetSearchChannel();

				foreach (var keyword in VirtualMailBox.Current.StreamSearchKeywords.GetKeyWords())
				{
					var tabItem = new OverviewColumnTabItem(channel, keyword.Split('|')[1])
					{
						Style = (Style)FindResource("StatusUpdatesColumnTabItem"),
						Icon = (BitmapSource)new ChannelIconConverter()
							.Convert(channel.Configuration, typeof(BitmapSource), 13, Thread.CurrentThread.CurrentCulture)
					};

					StreamsTab.Items.Add(tabItem);
				}
			}
		}

		void LoadSavedSelection()
		{
			SelectedChannels.Clear();
			SelectedChannels.AddRange(StatusUpdateControl.GetSavedSelection());

			OnPropertyChanged("HasTwitter");
		}

		void SaveChannelSelection()
		{
			// Save current selection
			ClientState.Current.Context.SaveSetting("/Settings/StatusUpdate/ChannelsSelection",
				String.Join(",", SelectedChannels.Select(i => i.ToString()).ToArray()));
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion		

		#region Event handler

		#region Command handlers

		void Send_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Send in realtime streams overview column");

			if (String.IsNullOrEmpty(PART_StatusUpdateTextbox.Text.Trim()) || SelectedChannels.Count == 0)
				return;

			CommandHelper.Send(SelectedChannels.Select(ChannelsManager.GetChannelObject), 
				ReplyTo == null ? null : ReplyTo.ChannelStatusKey, e);

			SaveChannelSelection();

			// Reset fields
			if (ShowViral)
			{
				ShowViral = false;
				OnPropertyChanged("ShowViral");

				SettingsManager.ClientSettings.AppConfiguration.IsFirstStatusUpdate = false;
				SettingsManager.Save();
			}
			else
			{
				ReplyTo = null;
				OnPropertyChanged("ReplyTo");
			}

			PART_StatusUpdateTextbox.Text = String.Empty;

            // Put focus back on listview
            FocusHelper.Focus(((TabItem)StreamsTab.SelectedItem));
		}

		void ShortenUrls_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Shorten url in realtime streams overview column");

			CommandHelper.ShortenUrlExecute(e);
		}		

		void ToggleChannel1_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Toggle channel in realtime streams overview column");

			ToggleChanneIndex(1);
		}

		void ToggleChannel2_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Toggle channel in realtime streams overview column");

			ToggleChanneIndex(2);
		}

		void ToggleChannel3_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Toggle channel in realtime streams overview column");

			ToggleChanneIndex(3);
		}

		void ToggleChannel4_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Toggle channel in realtime streams overview column");

			ToggleChanneIndex(4);
		}

		void ToggleChannel5_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Toggle channel in realtime streams overview column");

			ToggleChanneIndex(5);
		}

		void ToggleChannel6_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Toggle channel in realtime streams overview column");

			ToggleChanneIndex(6);
		}

		void ToggleChannel7_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Toggle channel in realtime streams overview column");

			ToggleChanneIndex(7);
		}

		void ToggleChannel8_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Toggle channel in realtime streams overview column");

			ToggleChanneIndex(8);
		}

		void ToggleChannel9_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Toggle channel in realtime streams overview column");

			ToggleChanneIndex(9);
		}

		#region CanExecute

		void CanAlwaysExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		void Send_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = CommandHelper.CanSendExecute(e) && SelectedChannels.Count > 0;
		}

		void ShortenUrls_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = CommandHelper.CanShortenUrlExecute(e);
		}

		void ToggleChannel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		#endregion

		#endregion

		void Close_Click(object sender, RoutedEventArgs e)
		{
			ClientStats.LogEvent("Close realtime streams overview column");
    
            if (ChannelsManager.GetStatusChannels().Count() > 0)
            {
				if (Inbox2MessageBox.Show(Strings.AreYouSureYouWantToCloseColumn, Inbox2MessageBoxButton.YesNo).Result == Inbox2MessageBoxResult.No)
                    return;                

                EventBroker.Publish(AppEvents.RebuildToolbar);
            }

			SettingsManager.ClientSettings.AppConfiguration.ShowStreamColumn = false;

			EventBroker.Publish(AppEvents.RebuildOverview);            
		}
        
		void StreamsTab_TabItemSelected(object sender, TabItemEventArgs e)
		{
			var ti = (OverviewColumnTabItem)e.TabItem;
			var stream = ti.Content as RealtimeStream;

			if (stream == null)
			{
				if (ti.Channel == null && String.IsNullOrEmpty(ti.Keyword))
				{
					ClientStats.LogEventWithSegment("Change tab in realtime streams overview column", "All streams");

					// All docked accounts tab
					stream = new RealtimeStream(ti.Channel, ti.Keyword) { IsColumnView = true };
				}
				else
				{
					ClientStats.LogEventWithSegment("Change tab in realtime streams overview column", ti.Channel.Configuration.DisplayName);

					stream = ControlCache.Get(ti.Channel, ti.Keyword, true);
					stream.UpdateDockState();

					// Disconnect from any parent that might hold the visual tree for our element
					if (stream.Parent != null)
						((ContentControl)stream.Parent).Content = null;

					stream.AfterUndock = delegate
					{
						stream.StatusUpdated -= Stream_StatusUpdated;
						stream.AfterUndock = null;
					};
				}

				stream.StatusUpdated += Stream_StatusUpdated;				

				ti.Content = stream;
			}

			// Updates visual appearance of tab control depending of wether we have a sub-tab or not
			StreamsTab.Tag = stream.SupportsMentions ? "StreamColumnWithMentions" : "StreamColumn";

			OnPropertyChanged("Channel");
		}

		void Stream_StatusUpdated(object sender, StatusUpdateEventArgs e)
		{
			ShowViral = false;
			ReplyTo = null;

			if (e.Action == StatusUpdateAction.Reply)
				ReplyTo = e.Status;

			SelectedChannels.Clear();
			SelectedChannels.Add(e.ChannelId);

			PART_StatusUpdateTextbox.Text = e.StatusText;
			
			FocusHelper.Focus(PART_StatusUpdateTextbox);

			// Move caret to end of textbox
			PART_StatusUpdateTextbox.SelectionStart = PART_StatusUpdateTextbox.Text.Length;

			OnPropertyChanged("AllStatusChannels");
			OnPropertyChanged("ReplyTo");
			OnPropertyChanged("ShowViral");
			OnPropertyChanged("HasTwitter");
		}

		void ToggleChannel_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            var channel = (ChannelInstance)e.Parameter;

            ToggleChannelInstance(channel);
        }

		void Cancel_Click(object sender, RoutedEventArgs e)
		{
			ClientStats.LogEvent("Cancel status update in realtime streams overview column");

			if (ShowViral)
			{
				ShowViral = false;
				OnPropertyChanged("ShowViral");

				SettingsManager.ClientSettings.AppConfiguration.IsFirstStatusUpdate = false;
				SettingsManager.Save();
			}
			else
			{
				ReplyTo = null;
				OnPropertyChanged("ReplyTo");
			}
			
			PART_StatusUpdateTextbox.Text = String.Empty;

            // Put focus back on listview
            FocusHelper.Focus(((TabItem)StreamsTab.SelectedItem));
		}

		void PART_StatusUpdateTextbox_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				Cancel_Click(sender, e);
			}
		}

        void StatusUpdater_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            UpdateStatusUpdaterVisibility(true);
        }

        void StatusUpdater_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            UpdateStatusUpdaterVisibility(false);
        }

		#endregion
	}
}