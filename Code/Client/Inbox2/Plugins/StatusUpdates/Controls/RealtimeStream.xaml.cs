using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Stats;
using Inbox2.Framework.Threading;
using Inbox2.Framework.Threading.AsyncUpdate;
using Inbox2.Framework.UI;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Plugins.StatusUpdates.Helpers;

namespace Inbox2.Plugins.StatusUpdates.Controls
{
	/// <summary>
	/// Interaction logic for RealtimeStream.xaml
	/// </summary>
	public partial class RealtimeStream : UserControl, INotifyPropertyChanged, IFocusChild
	{
		#region Fields

		public static RoutedCommand ViewAttachmentCommand = new RoutedCommand();

		public event PropertyChangedEventHandler PropertyChanged;

		public Action AfterUndock;
		public event EventHandler<StatusUpdateEventArgs> StatusUpdated;
		
		private readonly VirtualMailBox mailbox;
		private readonly DispatcherTimer markreadtimer;

		private int count;
		private int mentionsCount;

		#endregion

		#region Properties

		public CollectionViewSource StreamStatusUpdatesSource { get; private set; }
		public CollectionViewSource MentionsStatusUpdatesSource { get; private set; }		

		public ChannelInstance Channel { get; private set; }
		public string Keyword { get; private set; }

		public bool IsColumnView { get; set; }

		public UIElement FocusElement
		{
			get { return SupportsMentions ? Stream1ListView : Stream2ListView; }
		}

		public StatusUpdatesState State
		{
			get { return PluginsManager.Current.GetState<StatusUpdatesState>(); }
		}

		public string ChannelKeyword
		{
			get { return String.Format("{0}|{1}", Channel.Configuration.DisplayName, Keyword); }
		}

		public bool IsInSearchMode
		{
			get { return !String.IsNullOrEmpty(Keyword); }
		}

		public bool SupportsMentions
		{
			get { return Channel == null || (!IsInSearchMode && Channel.Configuration.Charasteristics.SupportsStatusUpdateMentions); }
		}

		public bool CanUndock
		{
			get
			{
				return IsColumnView && (Channel != null || IsInSearchMode);
			}
		}

		public long Count
		{
			get { return count; }
		}

		public long MentionsCount
		{
			get { return mentionsCount; }
		}

		#endregion

		#region Construction

		public RealtimeStream(ChannelInstance channel, string keyword)
		{
			this.Channel = channel;
			this.Keyword = keyword;
			this.mailbox = VirtualMailBox.Current;

			this.markreadtimer = new DispatcherTimer(DispatcherPriority.Normal, Dispatcher) { Interval = TimeSpan.FromMilliseconds(400), IsEnabled = false };
			this.markreadtimer.Tick += MarkReadTimerTick;

			InitializeComponent();

			StreamStatusUpdatesSource = new CollectionViewSource { Source = State.StatusUpdates };
			MentionsStatusUpdatesSource = new CollectionViewSource { Source = State.StatusUpdates };

			StreamStatusUpdatesSource.SortDescriptions.Add(new SortDescription("ParentSortDate", ListSortDirection.Descending));
			StreamStatusUpdatesSource.SortDescriptions.Add(new SortDescription("SortDate", ListSortDirection.Ascending));
			StreamStatusUpdatesSource.View.Filter = StreamStatusUpdatesSourceFilter;
			
			MentionsStatusUpdatesSource.SortDescriptions.Add(new SortDescription("ParentSortDate", ListSortDirection.Descending));
			MentionsStatusUpdatesSource.SortDescriptions.Add(new SortDescription("SortDate", ListSortDirection.Ascending));
			MentionsStatusUpdatesSource.View.Filter = MentionsStatusUpdatesSourceFilter;			

			DataContext = this;

			mailbox.InboxLoadComplete += delegate
				{
					StreamStatusUpdatesSource.View.Refresh();
					MentionsStatusUpdatesSource.View.Refresh();
				};

			EventBroker.Subscribe(AppEvents.SyncStatusUpdatesFinished, () => ThreadPool.QueueUserWorkItem(UpdateCountAsync));

			Responder.SetIsFirstResponder(SupportsMentions ? Stream2ListView : Stream1ListView, true);

			ThreadPool.QueueUserWorkItem(UpdateCountAsync);
		}

		#endregion

		#region Methods

		internal void MarkReadAsync()
		{
			markreadtimer.IsEnabled = true;

			// Update unread counts
			new BackgroundActionTask(delegate
			{
				var sb = new StringBuilder();

				if (Channel == null)
				{
					sb.Append("Update UserStatus Set IsRead='True' where IsRead='False'");
				}
				else
				{
					sb.Append("Update UserStatus Set IsRead='True' where ");
					sb.Append(IsInSearchMode ?
						String.Format("SearchKeyword='{0}'", ChannelKeyword) :
						String.Format("SourceChannelId={0}", Channel.Configuration.ChannelId)
					);	
				}				
				
				ClientState.Current.DataService.ExecuteNonQuery(sb.ToString());
			}).ExecuteAsync();
		}

		void UpdateCountAsync(object state)
		{
			if (Channel == null)
			{
				using (mailbox.StatusUpdates.ReaderLock)
					count = State.StatusUpdates.Count(u => u.IsRead == false);
			}
			else
			{
				using (mailbox.StatusUpdates.ReaderLock)
					count = IsInSearchMode ?
						State.StatusUpdates.Count(u => u.SearchKeyword == ChannelKeyword && u.IsRead == false) :
						State.StatusUpdates.Count(u => u.SourceChannelId == Channel.Configuration.ChannelId && u.IsRead == false);
			}

			if (Channel == null)
			{
				using (mailbox.StatusUpdates.ReaderLock)
					mentionsCount = State.StatusUpdates.Count(u => u.StatusType == StatusTypes.Mention && u.IsRead == false);
			}
			else
			{
				using (mailbox.StatusUpdates.ReaderLock)
					mentionsCount = State.StatusUpdates.Count(u => u.SourceChannelId == Channel.Configuration.ChannelId
						&& u.StatusType == StatusTypes.Mention && u.IsRead == false);
			}

			Thread.CurrentThread.ExecuteOnUIThread(delegate
				{
					OnPropertyChanged("Count");
					OnPropertyChanged("MentionsCount");
				});
		}

		bool StreamStatusUpdatesSourceFilter(object sender)
		{
			var status = (UserStatus)sender;

			if (status.SourceChannel == null)
				return false;

			if (Channel == null)
			{
				// All docked channels column
				return status.StatusType == StatusTypes.SearchUpdate || status.StatusType == StatusTypes.FriendUpdate;
			}
			else
			{
				if (IsInSearchMode)
					return (status.SearchKeyword == ChannelKeyword && status.StatusType == StatusTypes.SearchUpdate);
				else
					return (status.SourceChannelId == Channel.Configuration.ChannelId && status.StatusType == StatusTypes.FriendUpdate);	
			}			
		}

		bool MentionsStatusUpdatesSourceFilter(object sender)
		{
			var status = (UserStatus)sender;

			if (Channel == null)
			{
				// All docked channels column
				return status.StatusType == StatusTypes.Mention;
			}
			else
			{
				return (status.SourceChannelId == Channel.Configuration.ChannelId && status.StatusType == StatusTypes.Mention);
			}
		}

		void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		void OnStatusUpdated(UserStatus status, string statusText, long channelId, StatusUpdateAction action)
		{
			if (StatusUpdated != null)
			{
				StatusUpdated(this, new StatusUpdateEventArgs(status, statusText, channelId, action));
			}
		}

		internal void UpdateDockState()
		{
			OnPropertyChanged("CanUndock");
		}

		#endregion

		#region Event handlers

		#region Command handlers

		void Send_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Send in realtime stream");

			var tb = (TextBox)e.Parameter;

			if (!String.IsNullOrEmpty(tb.Text.Trim()))
				CommandHelper.SendExecute(Channel, e);
		}

		void ShortenUrls_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("ShortenUrl in realtime stream");

			var tb = (TextBox)e.Parameter;

			if (!String.IsNullOrEmpty(tb.Text.Trim()))
				CommandHelper.ShortenUrlExecute(e);
		}

		void Forward_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Forward in realtime stream");

			var status = (UserStatus)e.Parameter;

			StringBuilder rt = new StringBuilder();
			rt.Append("RT @");

			// Search results or twitter updates
			if (status.SourceChannel == null || status.SourceChannel.DisplayName == "Twitter")
				rt.AppendFormat("{0} ", status.From.Address);
			else
				rt.AppendFormat("{0} ({1}) ", status.From.DisplayName, status.SourceChannel.DisplayName);

			rt.Append(status.Status);

			OnStatusUpdated(status, rt.ToString(), status.SourceChannelId, StatusUpdateAction.Share);
		}

		void Reply_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Reply in realtime stream");

			var status = (UserStatus)e.Parameter;
			string statusText = String.Empty;

			if (status.SourceChannel == null || status.SourceChannel.DisplayName == "Twitter")
				statusText = String.Format("@{0} ", status.From.Address);

			OnStatusUpdated(status, statusText, status.SourceChannelId, StatusUpdateAction.Reply);
		}

		void ViewAttachment_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("View attachment in realtime stream");

			var attachment = (UserStatusAttachment)e.Parameter;

			new Process { StartInfo = new ProcessStartInfo(attachment.TargetUrl) }.Start();
		}

		void CanAlwaysExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}
		
		#endregion

		void MarkReadTimerTick(object sender, EventArgs e)
		{
			using (mailbox.StatusUpdates.ReaderLock)
			{
				if (Channel == null)
				{
					State.StatusUpdates.ForEach(u => u.MarkRead());
				}
				else
				{
					var q = State.StatusUpdates.AsQueryable();

					if (ViewTabControl.SelectedIndex < 1)
					{
						q = q.Where(u => u.SourceChannelId == Channel.Configuration.ChannelId && u.StatusType == StatusTypes.Mention);
					}
					else
					{
						q = IsInSearchMode ?
							q.Where(u => u.SearchKeyword == ChannelKeyword && u.StatusType == StatusTypes.SearchUpdate) :
							q.Where(u => u.SourceChannelId == Channel.Configuration.ChannelId && u.StatusType == StatusTypes.FriendUpdate);
					}

					q.ForEach(u => u.MarkRead());
				}				
			}

			markreadtimer.IsEnabled = false;

			// Fake a sync status upupdates finished event to force count updates
			EventBroker.Publish(AppEvents.SyncStatusUpdatesFinished);
		}

		void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ClientStats.LogEvent("Change realtime stream selection");

			var lv = (ListView) sender;

			if (lv.SelectedItem != null)
			{
				var status = (UserStatus) lv.SelectedItem;

				if (!status.IsRead)
				{
					status.MarkRead();

					AsyncUpdateQueue.Enqueue(status);

					// Forces update of unread counts
					EventBroker.Publish(AppEvents.SyncStatusUpdatesFinished);
				}
			}
		}

		void MarkRead_Click(object sender, RoutedEventArgs e)
		{
			if (Channel == null)
				ClientStats.LogEvent("Mark everything in realtime stream as read");
			else
				ClientStats.LogEvent("Mark realtime stream as read");

			MarkReadAsync();
		}

		void UnpinButton_Click(object sender, RoutedEventArgs e)
		{
			ClientStats.LogEvent("Unpin search in realtime stream");

			// Remove keyword
			VirtualMailBox.Current.StreamSearchKeywords.Remove(ChannelKeyword);

			// Update toolbar
			EventBroker.Publish(AppEvents.RebuildToolbar);
		}

		#endregion		
	}
}
