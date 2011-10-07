using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.ValueTypes;
using Inbox2.Framework.Threading.AsyncUpdate;
using Inbox2.Framework.UI;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Framework.VirtualMailBox.View;
using Inbox2.Platform.Framework.Collections;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Plugins.Conversations.Helpers;
using Inbox2.Plugins.Conversations.Utils;
using Inbox2.Framework.Localization;

namespace Inbox2.Plugins.Conversations
{
	public class ConversationsState : PluginStateBase
	{
		#region Fields

		private readonly Flipper flipper;
		private readonly ViewFilter viewFilter;

		#endregion

		#region Properties
		
		/// <summary>
		/// Gets or sets the messages view source.
		/// </summary>
		public CollectionViewSource ActivityViewSource { get; private set; }

		/// <summary>
		/// Gets or sets the selected messages.
		/// </summary>
		public AdvancedObservableCollection<Message> SelectedMessages { get; private set; }

		/// <summary>
		/// Gets the first selected message.
		/// </summary>
		public Message SelectedMessage
		{
			get { return SelectedMessages.FirstOrDefault(); }
		}		

		#endregion

		#region Properties

		public override bool CanView
		{
			get { return SelectedMessage != null; }
		}

		public override bool CanReply
		{
			get { return true; }
		}

		public override bool CanReplyAll
		{
			get { return true; }
		}

		public override bool CanForward
		{
			get { return true; }
		}

		public override bool CanDelete
		{
			get { return true; }
		}

		public override bool CanStar
		{
			get { return true; }
		}

		public override bool CanMarkRead
		{
			get { return SelectedMessage == null ? false : !SelectedMessage.IsRead; }
		}

		public override bool CanMarkUnread
		{
			get { return SelectedMessage == null ? false : SelectedMessage.IsRead; }
		}

		#endregion

		#region Constructors

		public ConversationsState()
		{
			viewFilter = ViewFilter.Current;

			ActivityViewSource = new CollectionViewSource { Source = viewFilter.Messages };
			SelectedMessages = new AdvancedObservableCollection<Message>();

			SelectedMessages.CollectionChanged += delegate
			{
				OnPropertyChanged("SelectedMessage");

				OnSelectionChanged();
			};

			flipper = new Flipper(TimeSpan.FromSeconds(25), delegate
				{
					// Todo run readstates syncornization if no receive task is currently running
				});
		}

		#endregion

		#region Methods

		public void Star()
		{
			StarCore();
		}

		protected override void NewCore()
		{
			ClientState.Current.ViewController.MoveTo(
				PluginsManager.Current.GetPlugin<ConversationsPlugin>().NewItemView,
					new NewMessageDataHelper { Body = MessageBodyGenerator.CreateBodyText() });
		}

		protected override void ViewCore()
		{
			ClientState.Current.ViewController.MoveTo(
				PluginsManager.Current.GetPlugin<ConversationsPlugin>().DetailsView,
					new OverviewDataHelper { MessageId = SelectedMessage.MessageId.Value });
		}

		protected override void ReplyCore()
		{
			if (CanChannelReply() == false)
				return;

			ActionHelper.Reply(SelectedMessage);
		}		

		protected override void ReplyAllCore()
		{
			if (CanChannelReply() == false)
				return;

			ActionHelper.ReplyAll(SelectedMessage);
		}

		protected override void ForwardCore()
		{
			ActionHelper.Forward(SelectedMessage);
		}

		protected override void DeleteCore()
		{
			Delete(false);
		}

		public void Delete(bool deleteConversations)
		{
			// Contains the same references as in SelectedMessages,
			// these references can change when un-doing so keep a snapshot around
			var previousSelection = deleteConversations ? 
					SelectedMessages.SelectMany(m => m.Conversation.Messages)
						.Distinct() // Select all messages from selected conversations
						.ToList() 
				: new List<Message>(SelectedMessages);

			// Contains instance copies of messages, this will be the old data
			// before the do is applied.
			var messagesCopy = previousSelection
				.Select(m => m.DuckCopy<Message>())
				.ToList();

			#region Do action

			Action doAction = delegate
			{
				foreach (var message in previousSelection)
				{
					message.MarkDeleted();					
				}

				viewFilter.UpdateCurrentViewAsync();

				flipper.Delay();
			};

			#endregion

			#region Undo action

			Action undoAction = delegate
			{
				foreach (var message in previousSelection)
				{
					// Get old message from copied data
					Message message1 = message;

					var oldMessage = messagesCopy.Single(m => m.MessageId == message1.MessageId);

					message.IsRead = oldMessage.IsRead;
					message.TargetMessageState = oldMessage.TargetMessageState;
					message.MessageFolder = oldMessage.MessageFolder;

					AsyncUpdateQueue.Enqueue(message);
				}				

				// We cannot use the IEditableObject appraoch here because the conversation in question
				// probably might not be in view anymore. So instead we will refresh the whole view.
				viewFilter.RebuildCurrentViewAsync();
			};

			#endregion

			ClientState.Current.UndoManager.Execute(new HistoryAction(doAction, undoAction));
		}

		public void Undelete()
		{
			// Contains the same references as in SelectedMessages,
			// these references can change when un-doing so keep a snapshot around
			var previousSelection = new List<Message>(SelectedMessages);

			// Contains instance copies of messages, this will be the old data
			// before the do is applied.
			var messagesCopy = SelectedMessages
				.Select(m => m.DuckCopy<Message>())
				.ToList();

			#region Do action

			Action doAction = delegate
			{
				foreach (var message in SelectedMessages.ToList())
				{
					message.MarkUndeleted();					
				}

				viewFilter.UpdateCurrentViewAsync();

				flipper.Delay();
			};

			#endregion

			#region Undo action

			Action undoAction = delegate
			{
				foreach (var message in previousSelection)
				{
					// Get old message from copied data
					Message message1 = message;

					var oldMessage = messagesCopy.Single(m => m.MessageId == message1.MessageId);

					message.IsRead = oldMessage.IsRead;
					message.TargetMessageState = oldMessage.TargetMessageState;
					message.MessageFolder = oldMessage.MessageFolder;

					AsyncUpdateQueue.Enqueue(message);
				}

				// We cannot use the IEditableObject appraoch here because the conversation in question
				// probably might not be in view anymore. So instead we will refresh the whole view.
				viewFilter.RebuildCurrentViewAsync();
			};

			#endregion

			ClientState.Current.UndoManager.Execute(new HistoryAction(doAction, undoAction));
		}
		
		protected void StarCore()
		{
			var previousSelection = new List<Message>(SelectedMessages);

			// Contains instance copies of messages, this will be the old data
			// before the do is applied.
			var messagesCopy = SelectedMessages
				.Select(m => m.DuckCopy<Message>())
				.ToList();

			#region Do action

			Action doAction = delegate
				{
					foreach (var message in SelectedMessages.ToList())
					{
						if (SelectedMessage.IsStarred)
							SelectedMessage.SetUnstarred();
						else
							SelectedMessage.SetStarred();
					}

					viewFilter.UpdateCurrentViewAsync();

					flipper.Delay();
				};

			#endregion

			#region Undo action

			Action undoAction = delegate
				{
					foreach (var message in previousSelection)
					{
						// Get old message from copied data
						Message message1 = message;

						var oldMessage = messagesCopy.Single(m => m.MessageId == message1.MessageId);

						// Reverts the previous action
						message.IsStarred = oldMessage.IsStarred;
						message.TargetMessageState = oldMessage.TargetMessageState;

						AsyncUpdateQueue.Enqueue(message);
					}

					// We cannot use the IEditableObject appraoch here because the conversation in question
					// probably might not be in view anymore. So instead we will refresh the whole view.
					viewFilter.RebuildCurrentViewAsync();
				};

			#endregion

			ClientState.Current.UndoManager.Execute(new HistoryAction(doAction, undoAction));
		}

		protected override void MarkReadCore()
		{
			var previousSelection = new List<Message>(SelectedMessages);

			// Contains instance copies of messages, this will be the old data
			// before the do is applied.
			var messagesCopy = SelectedMessages
				.Select(m => m.DuckCopy<Message>())
				.ToList();

			#region Do action

			Action doAction = delegate
			{
				foreach (var message in SelectedMessages.ToList())
				{
					message.MarkRead();					
				}

				viewFilter.UpdateCurrentViewAsync();

				flipper.Delay();
			};

			#endregion

			#region Undo action

			Action undoAction = delegate
			{
				foreach (var message in previousSelection)
				{
					// Get old message from copied data
					Message message1 = message;

					var oldMessage = messagesCopy.Single(m => m.MessageId == message1.MessageId);

					message.IsRead = oldMessage.IsRead;
					message.TargetMessageState = oldMessage.TargetMessageState;
					message.UpdateProperty("IsRead");

					AsyncUpdateQueue.Enqueue(message);
				}

				// We cannot use the IEditableObject appraoch here because the conversation in question
				// probably might not be in view anymore. So instead we will refresh the whole view.
				viewFilter.RebuildCurrentViewAsync();
			};

			#endregion

			ClientState.Current.UndoManager.Execute(new HistoryAction(doAction, undoAction));
		}

		protected override void MarkUnreadCore()
		{
			var previousSelection = new List<Message>(SelectedMessages);

			// Contains instance copies of messages, this will be the old data
			// before the do is applied.
			var messagesCopy = SelectedMessages
				.Select(m => m.DuckCopy<Message>())
				.ToList();

			#region Do action

			Action doAction = delegate
				{
					foreach (var message in SelectedMessages.ToList())
					{
						message.MarkUnread();						
					}

					viewFilter.UpdateCurrentViewAsync();

					flipper.Delay();
				};

			#endregion

			#region Undo action

			Action undoAction = delegate
				{
					foreach (var message in previousSelection)
					{
						// Get old message from copied data
						var oldMessage = messagesCopy.Single(m => m.MessageId == message.MessageId);

						message.IsRead = oldMessage.IsRead;
						message.TargetMessageState = oldMessage.TargetMessageState;
						message.UpdateProperty("IsRead");

						AsyncUpdateQueue.Enqueue(message);
					}

					// We cannot use the IEditableObject appraoch here because the conversation in question
					// probably might not be in view anymore. So instead we will refresh the whole view.
					viewFilter.RebuildCurrentViewAsync();
				};

			#endregion

			ClientState.Current.UndoManager.Execute(new HistoryAction(doAction, undoAction));
		}

		public void Archive()
		{
			var previousSelection = new List<Message>(
				SelectedMessages.SelectMany(m => m.Conversation.Messages)
								.Distinct());

			// Contains instance copies of messages, this will be the old data
			// before the do is applied.
			var messagesCopy = previousSelection
				.Select(m => m.DuckCopy<Message>())
				.ToList();

			#region Do action

			Action doAction = delegate
			{
				foreach (var message in SelectedMessages.ToList())
				{
					message.Archive();					
				}

				viewFilter.UpdateCurrentViewAsync();

				flipper.Delay();
			};

			#endregion

			#region Undo action

			Action undoAction = delegate
			{
				foreach (var message in previousSelection)
				{
					// Get old message from copied data
					Message message1 = message;

					var oldMessage = messagesCopy.Single(m => m.MessageId == message1.MessageId);

					// Reverts the previous action
					message.MessageFolder = oldMessage.MessageFolder;
					message.TargetMessageState = oldMessage.TargetMessageState;

					AsyncUpdateQueue.Enqueue(message);
				}

				// We cannot use the IEditableObject appraoch here because the conversation in question
				// probably might not be in view anymore. So instead we will refresh the whole view.
				viewFilter.RebuildCurrentViewAsync();
			};

			#endregion

			ClientState.Current.UndoManager.Execute(new HistoryAction(doAction, undoAction));
		}

		public void Unarchive()
		{
			var previousSelection = new List<Message>(
				SelectedMessages.SelectMany(m => m.Conversation.Messages)
								.Distinct());

			// Contains instance copies of messages, this will be the old data
			// before the do is applied.
			var messagesCopy = previousSelection
				.Select(m => m.DuckCopy<Message>())
				.ToList();

			#region Do action

			Action doAction = delegate
			{
				foreach (var message in SelectedMessages.ToList())
				{
					message.Unarchive();					
				}

				viewFilter.UpdateCurrentViewAsync();

				flipper.Delay();
			};

			#endregion

			#region Undo action

			Action undoAction = delegate
			{
				foreach (var message in previousSelection)
				{
					// Get old message from copied data
					Message message1 = message;

					var oldMessage = messagesCopy.Single(m => m.MessageId == message1.MessageId);

					// Reverts the previous action
					message.MessageFolder = oldMessage.MessageFolder;
					message.TargetMessageState = oldMessage.TargetMessageState;

					AsyncUpdateQueue.Enqueue(message);
				}

				// We cannot use the IEditableObject appraoch here because the conversation in question
				// probably might not be in view anymore. So instead we will refresh the whole view.
				viewFilter.RebuildCurrentViewAsync();
			};

			#endregion

			ClientState.Current.UndoManager.Execute(new HistoryAction(doAction, undoAction));
		}

		public void MoveToFolder(int folder)
		{
			var previousSelection = new List<Message>(
				SelectedMessages.SelectMany(m => m.Conversation.Messages)
								.Distinct());

			// Contains instance copies of messages, this will be the old data
			// before the do is applied.
			var messagesCopy = previousSelection
				.Select(m => m.DuckCopy<Message>())
				.ToList();

			#region Do action

			Action doAction = delegate
			{
				foreach (var message in SelectedMessages.ToList())
				{
					message.MoveToFolder(folder);					
				}

				viewFilter.UpdateCurrentViewAsync();

				flipper.Delay();
			};

			#endregion

			#region Undo action

			Action undoAction = delegate
			{
				foreach (var message in previousSelection)
				{
					// Get old message from copied data
					Message message1 = message;

					var oldMessage = messagesCopy.Single(m => m.MessageId == message1.MessageId);

					// Reverts the previous action
					message.MessageFolder = oldMessage.MessageFolder;

					AsyncUpdateQueue.Enqueue(message);
				}

				// We cannot use the IEditableObject appraoch here because the conversation in question
				// probably might not be in view anymore. So instead we will refresh the whole view.
				viewFilter.RebuildCurrentViewAsync();
			};

			#endregion

			ClientState.Current.UndoManager.Execute(new HistoryAction(doAction, undoAction));
		}

		public void AddLabel(Label label)
		{
			var previousSelection = new List<Message>(
				SelectedMessages.SelectMany(m => m.Conversation.Messages)
								.Distinct());

			// Contains instance copies of messages, this will be the old data
			// before the do is applied.
			var messagesCopy = previousSelection
				.Select(m => m.DuckCopy<Message>())
				.ToList();

			#region Do action

			Action doAction = delegate
			{
				foreach (var message in SelectedMessages.ToList())
				{
					message.AddLabel(label);					
				}

				viewFilter.UpdateCurrentViewAsync();

				flipper.Delay();
			};

			#endregion

			#region Undo action

			Action undoAction = delegate
			{
				foreach (var message in previousSelection)
				{
					// Get old message from copied data
					Message message1 = message;

					var oldMessage = messagesCopy.Single(m => m.MessageId == message1.MessageId);

					// Reverts the previous action
					message.Labels = oldMessage.Labels;
					message.LabelsList.Replace(oldMessage.LabelsList);
					message.SendLabels = oldMessage.SendLabels;

					AsyncUpdateQueue.Enqueue(message);
				}

				// We cannot use the IEditableObject appraoch here because the conversation in question
				// probably might not be in view anymore. So instead we will refresh the whole view.
				viewFilter.RebuildCurrentViewAsync();
			};

			#endregion

			ClientState.Current.UndoManager.Execute(new HistoryAction(doAction, undoAction));

			EventBroker.Publish(AppEvents.MessageLabelsUpdated, this);
		}

		public void RemoveLabel(Label label)
		{
			var previousSelection = new List<Message>(
				SelectedMessages.SelectMany(m => m.Conversation.Messages)
								.Distinct());

			// Contains instance copies of messages, this will be the old data
			// before the do is applied.
			var messagesCopy = previousSelection
				.Select(m => m.DuckCopy<Message>())
				.ToList();

			#region Do action

			Action doAction = delegate
			{
				foreach (var message in SelectedMessages.ToList())
				{
					message.RemoveLabel(label);					
				}

				viewFilter.UpdateCurrentViewAsync();

				flipper.Delay();
			};

			#endregion

			#region Undo action

			Action undoAction = delegate
			{
				foreach (var message in previousSelection)
				{
					// Get old message from copied data
					Message message1 = message;

					var oldMessage = messagesCopy.Single(m => m.MessageId == message1.MessageId);

					// Reverts the previous action
					message.Labels = oldMessage.Labels;
					message.LabelsList.Replace(oldMessage.LabelsList);
					message.SendLabels = oldMessage.SendLabels;

					AsyncUpdateQueue.Enqueue(message);
				}

				// We cannot use the IEditableObject appraoch here because the conversation in question
				// probably might not be in view anymore. So instead we will refresh the whole view.
				viewFilter.RebuildCurrentViewAsync();
			};

			#endregion

			ClientState.Current.UndoManager.Execute(new HistoryAction(doAction, undoAction));

			EventBroker.Publish(AppEvents.MessageLabelsUpdated, this);
		}

		public void ClearActions()
		{
			var previousSelection = new List<Message>(
				SelectedMessages.SelectMany(m => m.Conversation.Messages)
								.Distinct());

			// Contains instance copies of messages, this will be the old data
			// before the do is applied.
			var messagesCopy = previousSelection
				.Select(m => m.DuckCopy<Message>())
				.ToList();

			#region Do action

			Action doAction = delegate
			{
				foreach (var message in SelectedMessages.ToList())
				{
					if (message.IsTodo)
						message.RemoveLabel(message.LabelsList.First(l => l.LabelType == LabelType.Todo));

					if (message.IsWaitingFor)
						message.RemoveLabel(message.LabelsList.First(l => l.LabelType == LabelType.WaitingFor));

					if (message.IsSomeday)
						message.RemoveLabel(message.LabelsList.First(l => l.LabelType == LabelType.Someday));					
				}

				viewFilter.UpdateCurrentViewAsync();

				flipper.Delay();
			};

			#endregion

			#region Undo action

			Action undoAction = delegate
			{
				foreach (var message in previousSelection)
				{
					// Get old message from copied data
					Message message1 = message;

					var oldMessage = messagesCopy.Single(m => m.MessageId == message1.MessageId);

					// Reverts the previous action
					message.Labels = oldMessage.Labels;
					message.LabelsList.Replace(oldMessage.LabelsList);
					message.SendLabels = oldMessage.SendLabels;

					AsyncUpdateQueue.Enqueue(message);
				}

				// We cannot use the IEditableObject appraoch here because the conversation in question
				// probably might not be in view anymore. So instead we will refresh the whole view.
				viewFilter.RebuildCurrentViewAsync();
			};

			#endregion

			ClientState.Current.UndoManager.Execute(new HistoryAction(doAction, undoAction));

			EventBroker.Publish(AppEvents.MessageLabelsUpdated, this);
		}

		bool CanChannelReply()
		{
			var channel = SelectedMessage.SourceChannel;

			if (channel != null)
			{
				if (!channel.Charasteristics.CanReply)
				{
					var sb = new StringBuilder();
					var url = channel.ProfileInfoBuilder.InboxUrl;

					sb.AppendFormat(Strings.ChannelDoesNotSupportReply, channel.DisplayName);

					if (!String.IsNullOrEmpty(url))
					{
						sb.AppendFormat(" " + Strings.ClickOkToReplyOn, channel.DisplayName);

						if (Inbox2MessageBox.Show(sb.ToString(), Inbox2MessageBoxButton.OKCancel).Result == Inbox2MessageBoxResult.OK)
						{
							new Process { StartInfo = new ProcessStartInfo(url) }.Start();
						}
					}
					else
					{
						Inbox2MessageBox.Show(sb.ToString(), Inbox2MessageBoxButton.OK);
					}

					return false;
				}
			}

			return true;
		}

		#endregion
	}
}