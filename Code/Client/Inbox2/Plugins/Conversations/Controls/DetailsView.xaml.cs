using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Inbox2.Framework;
using Inbox2.Framework.Stats;
using Inbox2.Framework.UI;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Framework.Localization;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Framework.Collections;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Plugins.Conversations.Helpers;
using Label = Inbox2.Framework.VirtualMailBox.Entities.Label;

namespace Inbox2.Plugins.Conversations.Controls
{
	/// <summary>
	/// Interaction logic for DetailsView.xaml
	/// </summary>
	public partial class DetailsView : UserControl, INotifyPropertyChanged, IPersistableTab, IDisposable
	{
		#region Fields

		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler<EventArgs> RequestCloseTab;
		public event EventHandler<EventArgs> AfterAction;
	    
        private readonly VirtualMailBox mailbox = VirtualMailBox.Current;
        private ListView list;

		#endregion

		#region Properties

		public Message SelectedMessage { get; private set; }

		public Conversation SelectedConversation { get; private set; }

		public string Title
		{
			get
			{
				if (SelectedMessage == null || String.IsNullOrEmpty(SelectedMessage.Context))
					return Strings.NoSubject;

				return SelectedMessage.Context.ToClearSubject();
			}
		}

		public Control CustomHeaderContent
		{
			get { return null; }
		}

		public PluginPackage Plugin
		{
			get { return PluginsManager.Current.GetPlugin<ConversationsPlugin>(); }
		}

		public ViewType ViewType
		{
			get { return ViewType.DetailsView; }
		}

		public ConversationsState State
		{
			get { return (ConversationsState)Plugin.State; }
		}

		public AdvancedObservableCollection<Message> Messages { get; private set; }
		public CollectionViewSource MessagesViewSource { get; private set; }

		#endregion

		#region Construction

		public DetailsView()
		{
			InitializeComponent();

			Messages = new AdvancedObservableCollection<Message>();
			MessagesViewSource = new CollectionViewSource { Source = Messages };
			MessagesViewSource.SortDescriptions.Add(new SortDescription("SortDate", ListSortDirection.Ascending));
			MessagesViewSource.View.Filter = MessageViewSourceFilter;

			DataContext = this;
		}

		#endregion

		#region Methods

		public void LoadData(object dataInstance)
		{
			var helper = ((OverviewDataHelper) dataInstance);

			using (mailbox.Messages.ReaderLock)
				SelectedMessage =
					mailbox.Messages.FirstOrDefault(
						c => c.MessageId == helper.MessageId);

			if (SelectedMessage == null)
				throw new ApplicationException(
					String.Format("Message was not found. MessageId = {0}", ((OverviewDataHelper)dataInstance).MessageId));

			// Mark all messages as read
			SelectedMessage.MarkRead();

			OnPropertyChanged("SelectedMessage");
			OnPropertyChanged("Title");

			if (SelectedConversation == null || SelectedMessage.Conversation != SelectedConversation)
			{
				SelectedConversation = SelectedMessage.Conversation;
				OnPropertyChanged("SelectedConversation");

				Messages.Replace(SelectedConversation.Messages);

				if (list != null)
				{
					list.SelectedItem = SelectedMessage;

					Dispatcher.BeginInvoke(DispatcherPriority.Input, (Action) (() => list.ScrollIntoView(SelectedMessage)));									
				}				
			}
			
			MessageActionsBox.Show(SelectedMessage);
			ThreadView.Show(SelectedMessage);
			UserProfileControl.SourceAddress = SelectedMessage.From;

            FocusHelper.Focus(TabSink);
		}

		public object SaveData()
		{
			return new OverviewDataHelper { MessageId = SelectedMessage.MessageId.Value };
		}

		public void Dispose()
		{
			ThreadView.Dispose();
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		bool MessageViewSourceFilter(object sender)
		{
			Message message = (Message)sender;

			return message.MessageFolder != Folders.Trash;
		}      

		#endregion

		#region Command handlers
		
		void Reply_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Reply in message details view");

			Plugin.State.Reply();
		}

		void ReplyAll_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Reply all in message details view");

			Plugin.State.ReplyAll();
		}

		void Forward_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Forward in message details view");

			Plugin.State.Forward();
		}

		void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Delete in message details view");

			Plugin.State.Delete();
		}

		void MarkRead_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("MarkRead in message details view");

			Plugin.State.MarkRead();
		}

		void MarkUnread_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("MarkUnread in message details view");

			Plugin.State.MarkUnread();
		}

		void Todo_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Todo in message details view");

			SelectedMessage.AddLabel(new Label(LabelType.Todo));
		}

		void WaitingFor_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("WaitingFor in message details view");

			SelectedMessage.AddLabel(new Label(LabelType.WaitingFor));
		}

		void Someday_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Someday in message details view");

			SelectedMessage.AddLabel(new Label(LabelType.Someday));
		}

		void OpenDocument_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Open document in message details view");

			EventBroker.Publish(AppEvents.View, (Document)e.Parameter);
		}

		void SaveDocument_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Save document in message details view");

			EventBroker.Publish(AppEvents.Save, (Document)e.Parameter);
		}

        void NewerConversation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
			ClientStats.LogEvent("Newer conversation in message details view");

            List<Conversation> conversations;

            using (mailbox.Conversations.ReaderLock)
                conversations = mailbox.Conversations
                    .OrderByDescending(c => c.SortDate)
                    .ToList();

            var index = conversations.IndexOf(SelectedMessage.Conversation);

            if (index > -1)
            {
                var newerConversation = conversations[index +1];

                LoadData(new OverviewDataHelper { MessageId = newerConversation.Last.MessageId.Value });
            }
        }

        void OlderConversation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
			ClientStats.LogEvent("Older conversation in message details view");

            List<Conversation> conversations;

			using (mailbox.Conversations.ReaderLock)
                conversations = mailbox.Conversations
                    .OrderByDescending(c => c.SortDate)
                    .ToList();

            var index = conversations.IndexOf(SelectedMessage.Conversation);

            if (index > 0 && index < conversations.Count -1)
            {
                var newerConversation = conversations[index -1];

                LoadData(new OverviewDataHelper { MessageId = newerConversation.Last.MessageId.Value });
            }
        }

        void NewerMessage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
			ClientStats.LogEvent("Newer message in message details view");

            if (list.SelectedIndex < list.Items.Count - 1)
            {
                list.SelectedIndex++;

                list.ScrollIntoView(list.SelectedItem);
            }
        }

        void OlderMessage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
			ClientStats.LogEvent("Older message in message details view");

            if (list.SelectedIndex > 0)
            {
                list.SelectedIndex--;

                list.ScrollIntoView(list.SelectedItem);
            }
        }

        void NextImportant_Executed(object sender, ExecutedRoutedEventArgs e)
        {
			ClientStats.LogEvent("Next important in message details view");
        }

        void Archive_Executed(object sender, ExecutedRoutedEventArgs e)
        {
			ClientStats.LogEvent("Archive in message details view");

            SelectedMessage.Archive();
        }

        void Star_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectedMessage.IsStarred)
            {
                ClientStats.LogEvent("Star in message details view");

                SelectedMessage.SetUnstarred();
            }
            else
            {
                ClientStats.LogEvent("Unstar in message details view");

                SelectedMessage.SetStarred();
            }
        }

        void InlineReply_Executed(object sender, ExecutedRoutedEventArgs e)
        {
			ClientStats.LogEvent("Inline reply in message details view");

            FocusHelper.Focus(ThreadView.QuickReplyAll);
        }

        void ForwardInline_Executed(object sender, ExecutedRoutedEventArgs e)
        {
			ClientStats.LogEvent("Inline forward in message details view");

            State.Forward();
        }

        void AddLabel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
			ClientStats.LogEvent("Add label in message details view");

            FocusHelper.Focus(ThreadView.LabelsEditor);
        }

	    #region CanExecute

	    void CanAlwaysExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        e.CanExecute = true;
	    }

	    void Reply_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        e.CanExecute = true;
	    }

	    void ReplyAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        e.CanExecute = true;
	    }

	    void Forward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        e.CanExecute = true;
	    }

	    void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        e.CanExecute = true;
	    }

	    void MarkRead_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
	        e.CanExecute = SelectedMessage != null && !SelectedMessage.IsRead;
	    }

	    void MarkUnread_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
            e.CanExecute = SelectedMessage != null && SelectedMessage.IsRead;
	    }

	    void Todo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
            e.CanExecute = SelectedMessage != null && !(SelectedMessage.IsTodo || SelectedMessage.IsWaitingFor || SelectedMessage.IsSomeday);
	    }

	    void WaitingFor_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
            e.CanExecute = SelectedMessage != null && !(SelectedMessage.IsTodo || SelectedMessage.IsWaitingFor || SelectedMessage.IsSomeday);
	    }

	    void Someday_CanExecute(object sender, CanExecuteRoutedEventArgs e)
	    {
            e.CanExecute = SelectedMessage != null && !(SelectedMessage.IsTodo || SelectedMessage.IsWaitingFor || SelectedMessage.IsSomeday);
	    }

	    #endregion

		#endregion

		#region Event handlers

        void List_OnLoaded(object sender, RoutedEventArgs e)
        {
            list = (ListView) sender;
			list.ScrollIntoView(list.SelectedItem);
        }

		void MessagesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.RemovedItems.Count > 0)
			{
				var message = ((ListView) e.OriginalSource).SelectedItem as Message;

				if (message != null)
				{
					LoadData(new OverviewDataHelper {MessageId = message.MessageId.Value});

					State.SelectedMessages.Replace(new[] {message});
				}
			}
		}

		void MessagesListView_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				ClientStats.LogEvent("View message in message details view (keyboard)");

				State.SelectedMessages.ReplaceWithCast(new[] { ((ListView)e.Source).SelectedItem });
				State.View();
			}
			else if (e.Key == Key.Delete)
			{
				ClientStats.LogEvent("Delete message in message details view (keyboard)");

				State.SelectedMessages.ReplaceWithCast(new[] { ((ListView)e.Source).SelectedItem });
				State.Delete();
			}
		}

		void MessagesListView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			ClientStats.LogEvent("View message in message details view (mouse)");

			State.SelectedMessages.ReplaceWithCast(new[] { ((ListView)e.Source).SelectedItem });
		}    

		#endregion	    
	}
}