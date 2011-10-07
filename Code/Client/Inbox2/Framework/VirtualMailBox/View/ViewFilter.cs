using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Inbox2.Core.Configuration;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Platform.Logging;

namespace Inbox2.Framework.VirtualMailBox.View
{
	public class ViewFilter
	{
		#region Singleton pattern implementation

		private static ViewFilter _current;

		public static ViewFilter Current
		{
			get
			{
				if (_current == null)
					_current = new ViewFilter();

				return _current;
			}
		}

		#endregion

		private readonly VirtualMailBox mailbox;
		private readonly Thread workerThread;
		private readonly AutoResetEvent signal;

		private volatile bool refresh;

		public Filter Filter { get; private set; }
		public ThreadSafeCollection<Message> Messages { get; private set; }		

		public ViewFilter()
		{
			Filter = new Filter();
			Filter.FilterChanged += delegate { RebuildCurrentViewAsync(); };

			Messages = new ThreadSafeCollection<Message>();

			mailbox = VirtualMailBox.Current;
			mailbox.InboxLoadComplete += delegate
			    {
					// Perform a lock-free update on the UI-Thread.
					// We should be safe because at this point there should be no async tasks
					// running and all inserts should happen on the UI thread anyway.
					Messages.Replace(GetMessagesInView(
						mailbox.Messages.Where(IsMessageVisible).AsQueryable()).ToList());
			    };

			signal = new AutoResetEvent(false);
			workerThread = new Thread(RenderView)
			    {
			        Name = "View rendering thread",
			        IsBackground = true,
			        Priority = ThreadPriority.Normal
			    };

			workerThread.Start();
		}

		public void RebuildCurrentViewAsync()
		{
			refresh = true;
			signal.Set();
		}

		public void UpdateCurrentViewAsync()
		{
			signal.Set();
		}

		void RenderView()
		{
			while (true)
			{
				// Wait for the signal to be set
				signal.WaitOne();

				try
				{
					if (refresh)
						RebuildCurrentView();
					else
						UpdateCurrentView();
				}
				catch (Exception ex)
				{
					Logger.Error("An error has occured while trying to update view. Exception = {0}", LogSource.UI, ex);
				}
			}
		}

		internal void UpdateCurrentView()
		{
			List<Message> newView;

			using (mailbox.Messages.ReaderLock)
				newView = GetMessagesInView(
					mailbox.Messages.Where(IsMessageVisible)
						.AsQueryable()).ToList();

			// Remove items that have disappeared from view
			for (int i = Messages.Count - 1; i >= 0; i--)
			{
				if (!newView.Contains(Messages[i]))
					Messages.RemoveAt(i);
			}

			// Add items that have appeared in view
			for (int i = 0; i < newView.Count; i++)
			{
				if (!Messages.Contains(newView[i]))
					Messages.Insert(i, newView[i]);
			}
		}

		internal void RebuildCurrentView()
		{
			using (mailbox.Messages.WriterLock)
				mailbox.Messages.ForEach(m => m.IsVisible = IsMessageVisible(m));

			List<Message> messages;

			using (mailbox.Messages.ReaderLock)
				messages = GetMessagesInView(
					mailbox.Messages.Where(m => m.IsVisible).AsQueryable()).ToList();

			Messages.Replace(messages);

			refresh = false;
		}

		IEnumerable<Message> GetMessagesInView(IQueryable<Message> query)
		{
			// If conversations are enabled, first sort on conversation.sortdate
			if (SettingsManager.ClientSettings.AppConfiguration.RollUpConversations
				&& Filter.CurrentView == ActivityView.MyInbox)
				query = query.OrderByDescending(m => m.Conversation.SortDate);

			return query.OrderByDescending(m => m.SortDate);
		}

		bool IsMessageVisible(Message message)
		{
			if (FilterMessage(message))
			{
				if (Filter.IsActivityViewVisible)
				{
					if (SettingsManager.ClientSettings.AppConfiguration.RollUpConversations)
					{
						// Default behavior for my inbox
						if (Filter.CurrentView == ActivityView.MyInbox || Filter.CurrentView == ActivityView.Archive)
							return message.IsLast;
					}					
				}
				else
				{
					if (Filter.CurrentView == ActivityView.MyInbox || Filter.CurrentView == ActivityView.Archive)
						return message.IsLast;
				}

				return true;
			}

			return false;
		}

		bool FilterMessage(Message message)
		{
			if (message == null)
				return false;

			// For our fake message work-around
			if (message.MessageId == -1)
				return true;

			// This is a expunged message
			if (message.ConversationIdentifier == "-1")
				return false;

			// Break out if source/target channel is not visible
			if (!message.IsChannelVisible)
				return false;

			if (Filter.CurrentView == ActivityView.MyInbox && message.Conversation.Messages.Count(m => m.MessageFolder != Folders.SentItems) == 0)
				return false;

			if (Filter.CurrentView == ActivityView.MyInbox &&
				(message.MessageFolder == Folders.Trash
					|| message.MessageFolder == Folders.Archive
					|| message.MessageFolder == Folders.Drafts
					|| message.MessageFolder == Folders.Spam))
				return false;

			if (Filter.CurrentView == ActivityView.Archive && message.MessageFolder != Folders.Archive)
				return false;

			if (Filter.CurrentView == ActivityView.Received && message.MessageFolder != Folders.Inbox)
				return false;

			if (Filter.CurrentView == ActivityView.Sent && message.MessageFolder != Folders.SentItems)
				return false;

			if (Filter.CurrentView == ActivityView.Drafts && message.MessageFolder != Folders.Drafts)
				return false;

			if (Filter.CurrentView == ActivityView.Trash && message.MessageFolder != Folders.Trash)
				return false;

			if (Filter.CurrentView == ActivityView.Starred && message.IsStarred == false)
				return false;

			if (Filter.CurrentView == ActivityView.Unread && (message.IsRead || 
				message.MessageFolder == Folders.Trash || message.MessageFolder == Folders.Spam))
				return false;

			if (Filter.CurrentView == ActivityView.Todo && message.IsTodo == false)
				return false;

			if (Filter.CurrentView == ActivityView.WaitingFor && message.IsWaitingFor == false)
				return false;

			if (Filter.CurrentView == ActivityView.Someday && message.IsSomeday == false)
				return false;

			if (Filter.CurrentView == ActivityView.Label && (message.IsTrash || message.HasLabel(Filter.Label) == false))
				return false;

			return true;
		}
	}
}
