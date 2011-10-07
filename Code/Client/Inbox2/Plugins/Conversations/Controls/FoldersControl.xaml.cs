using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Inbox2.Core.Configuration;
using Inbox2.Framework;
using Inbox2.Framework.Collections;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Stats;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Framework.VirtualMailBox.View;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Framework.Collections;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Platform.Logging;
using Label = Inbox2.Framework.VirtualMailBox.Entities.Label;

namespace Inbox2.Plugins.Conversations.Controls
{
	/// <summary>
	/// Interaction logic for FoldersControl.xaml
	/// </summary>
	public partial class FoldersControl : UserControl, INotifyPropertyChanged
	{
		#region DependencyProperties

		public static readonly DependencyProperty ActivityViewProperty = DependencyProperty.RegisterAttached("ActivityView", typeof(ActivityView), typeof(RadioButton), new UIPropertyMetadata(ActivityView.MyInbox));
	
		public static ActivityView GetActivityView(DependencyObject obj)
		{
			return (ActivityView)obj.GetValue(ActivityViewProperty);
		}

		public static void SetActivityView(DependencyObject obj, ActivityView value)
		{
			obj.SetValue(ActivityViewProperty, value);
		}

		#endregion

		#region Fields

		public event PropertyChangedEventHandler PropertyChanged;

		private readonly VirtualMailBox mailbox;
		private readonly ViewFilter viewFilter;
		private readonly ThreadSafeCollection<LabelsContainer> labels;
		private readonly Thread workerThread;
		private readonly AutoResetEvent signal;

		public CollectionViewSource LabelsViewSource { get; private set; }

		private long unread;
		private long starred;
		private long todo;
		private long waitingFor;
		private long someday;
		private long drafts;

		#endregion

		#region Properties

		public AdvancedObservableCollection<LabelsContainer> Labels
		{
			get { return labels; }
		}

		public ConversationsPlugin Plugin
		{
			get { return PluginsManager.Current.GetPlugin<ConversationsPlugin>(); }
		}

		public ConversationsState State
		{
			get { return (ConversationsState)Plugin.State; }
		}

		public long Unread
		{
			get { return unread; }
		}

		public long Starred
		{
			get { return starred; }
		}

		public long Todo
		{
			get { return todo; }
		}

		public long WaitingFor
		{
			get { return waitingFor; }
		}

		public long Someday
		{
			get { return someday; }
		}

		public long Drafts
		{
			get { return drafts; }
		}

		#endregion

		public FoldersControl()
		{
			mailbox = VirtualMailBox.Current;
			viewFilter = ViewFilter.Current;
			labels = new ThreadSafeCollection<LabelsContainer>();

			LabelsViewSource = new CollectionViewSource { Source = labels };
			LabelsViewSource.SortDescriptions.Add(new SortDescription("Count", ListSortDirection.Descending));
			LabelsViewSource.View.Filter = IsLabelVisible;

			InitializeComponent();

			signal = new AutoResetEvent(false);
			workerThread = new Thread(UpdateCounters)
			{
				Name = "Counter update thread",
				IsBackground = true,
				Priority = ThreadPriority.BelowNormal
			};

			workerThread.Start();

			DataContext = this;

			VirtualMailBox.Current.LoadComplete += delegate { UpdateCountersAsync(); };

			EventBroker.Subscribe(AppEvents.RebuildOverview, UpdateCountersAsync);
			EventBroker.Subscribe(AppEvents.ReceiveMessagesFinished, UpdateCountersAsync);
			EventBroker.Subscribe(AppEvents.MessageUpdated, UpdateCountersAsync);	
		
			EventBroker.Subscribe(AppEvents.View, delegate(ActivityView view)
				{
					ClientState.Current.ViewController.MoveTo(WellKnownView.Overview);

					// Find the radio-button with the requested view
					LogicalTreeWalker.Walk(this, delegate(RadioButton rb)
						{
							if (GetActivityView(rb) == view)
								rb.IsChecked = true;
						});

					viewFilter.Filter.CurrentView = view;
					viewFilter.RebuildCurrentViewAsync();

                    EventBroker.Publish(AppEvents.RequestFocus);
				});

			EventBroker.Subscribe(AppEvents.View, delegate(Label label)
				{
					EventBroker.Publish(AppEvents.View, ActivityView.Label);

					viewFilter.Filter.Label = label.Labelname;
					viewFilter.RebuildCurrentViewAsync();

                    EventBroker.Publish(AppEvents.RequestFocus);
				});

            EventBroker.Subscribe(AppEvents.ShuttingDown, () => Thread.CurrentThread.ExecuteOnUIThread(delegate
                {
                    // Save settings during shutdown
                    SettingsManager.ClientSettings.AppConfiguration.ShowProductivityColumn = ProductivityExpander.IsExpanded;
					SettingsManager.ClientSettings.AppConfiguration.ShowLabelsColumn = LabelsExpander.IsExpanded;
                    SettingsManager.Save();
                }));

			EventBroker.Subscribe(AppEvents.LabelCreated, (string label) => labels.Add(new LabelsContainer(label)));
		}

		bool IsLabelVisible(object obj)
		{
			return ((LabelsContainer) obj).Count > 0;
		}

		void UpdateCountersAsync()
		{
			signal.Set();
		}

		void UpdateCounters(object state)
		{
			while (true)
			{
				// Wait for the signal to be set
				signal.WaitOne();

				try
				{
					using (mailbox.Messages.ReaderLock)
					{
                        unread = mailbox.Messages.Count(m => m.IsRead == false && m.MessageFolder != Folders.Trash && m.MessageFolder != Folders.None && m.MessageFolder != Folders.Spam);
                        starred = mailbox.Messages.Count(m => m.IsStarred && m.MessageFolder != Folders.Trash && m.MessageFolder != Folders.None && m.MessageFolder != Folders.Spam);
                        todo = mailbox.Messages.Count(m => m.IsTodo && m.MessageFolder != Folders.Trash && m.MessageFolder != Folders.None && m.MessageFolder != Folders.Spam);
                        waitingFor = mailbox.Messages.Count(m => m.IsWaitingFor && m.MessageFolder != Folders.Trash && m.MessageFolder != Folders.None && m.MessageFolder != Folders.Spam);
                        someday = mailbox.Messages.Count(m => m.IsSomeday && m.MessageFolder != Folders.Trash && m.MessageFolder != Folders.None && m.MessageFolder != Folders.Spam);
                        drafts = mailbox.Messages.Count(m => m.IsDraft && m.MessageFolder != Folders.Trash && m.MessageFolder != Folders.None && m.MessageFolder != Folders.Spam);
					}
				    
					Thread.CurrentThread.ExecuteOnUIThread(delegate
					{
						using (labels.ReaderLock)
							labels.ForEach(l => l.Refresh());

						OnPropertyChanged("Unread");
						OnPropertyChanged("Starred");
						OnPropertyChanged("Todo");
						OnPropertyChanged("WaitingFor");
						OnPropertyChanged("Someday");

						LabelsViewSource.View.Refresh();
					});
				}
				catch (Exception ex)
				{
					Logger.Error("An error has occured while trying to update counters. Exception = {0}", LogSource.UI, ex);
				}
			}			
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			ProductivityExpander.IsExpanded = SettingsManager.ClientSettings.AppConfiguration.ShowProductivityColumn;
			LabelsExpander.IsExpanded = SettingsManager.ClientSettings.AppConfiguration.ShowLabelsColumn;
		}
        
		void RadioButton_Click(object sender, RoutedEventArgs e)
		{
			viewFilter.Filter.CurrentView = GetActivityView((RadioButton)e.OriginalSource);
			viewFilter.Filter.Label = viewFilter.Filter.CurrentView == ActivityView.Label
				? ((LabelsContainer) ((RadioButton) e.OriginalSource).Tag).Labelname : String.Empty;

			viewFilter.RebuildCurrentViewAsync();

			if (viewFilter.Filter.CurrentView == ActivityView.Label)
				ClientStats.LogEvent("Goto label (mouse)");
			else
				ClientStats.LogEventWithSegment("Goto folder (mouse)", viewFilter.Filter.CurrentView.ToString().ToLower());

            EventBroker.Publish(AppEvents.RequestFocus);
		}   
	}
}


