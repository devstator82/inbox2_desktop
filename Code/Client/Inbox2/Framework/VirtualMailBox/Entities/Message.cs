using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using Inbox2.Core.Configuration;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Persistance;
using Inbox2.Framework.Threading.AsyncUpdate;
using Inbox2.Framework.VirtualMailBox.Mappers;
using Inbox2.Platform.Channels.Attributes;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Framework.CloudApi.Enumerations;
using Inbox2.Platform.Framework.Collections;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Framework.VirtualMailBox.Entities
{
	[PersistableClass]
	[Serializable]
	[ContentMapper(typeof(MessageContentMapper))]
	public class Message : IEntityBase, IDataServiceHooks
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private ChannelConfiguration sourceChannelConfig;
		private ChannelConfiguration targetChannelConfig;
		private bool configLoaded;

		private Conversation conversation;
		private bool bodyPreviewLoaded;
		private string bodyPreview;
		

		[IndexAndStore]
		[PrimaryKey] public long? MessageId { get; set; }

		[Persist] public string MessageKey { get; set; }

		[Persist] public string MessageNumber { get; set; }
		[Persist] public string MessageIdentifier { get; set; }
		[Persist] public string InReplyTo { get; set; }
		[Persist] public string SourceFolder { get; set; }
		
		[Persist] public long SourceChannelId { get; set; }
		[Persist] public long TargetChannelId { get; set; }

		[IndexTokenizeAndStore]
		[Persist] public string Context { get; set; }
		[Persist] public string OriginalContext { get; set; }		

		[IndexTokenizeAndStore]
		[Persist] public string BodyPreview
		{
			get
			{
				if (!bodyPreviewLoaded)
				{
					ThreadPool.QueueUserWorkItem(EnsureBodyPreview);

					return String.Empty;
				}

				return bodyPreview;
			}
			set
			{
				bodyPreviewLoaded = true;
				bodyPreview = value;
			}
		}

		[Persist] public Guid? BodyTextStreamName { get; set; }
		[Persist] public Guid? BodyHtmlStreamName { get; set; }
	
		[Persist] public long Size { get; set; }

		[Persist] public SourceAddress From { get; set; }
		[Persist] public SourceAddress ReturnTo { get; set; }
		[Persist] public SourceAddressCollection To { get; set; }
		[Persist] public SourceAddressCollection CC { get; set; }
		[Persist] public SourceAddressCollection BCC { get; set; }

		[Persist] public bool IsRead { get; set; }        
		[Persist] public EntityStates? TargetMessageState { get; set; }
		
		[Persist] public DateTime? DateRead { get; set; }
		[Persist] public DateTime? DateAction { get; set; }
		[Persist] public DateTime? DateReply { get; set; }
		[Persist] public DateTime? DateReceived { get; set; }
		[Persist] public DateTime? DateSent { get; set; }

		public ChannelMetadata Metadata { get; set; }

		[Persist] public string ConversationIdentifier { get; set; }			
		[Persist] public int MessageFolder { get; set; }

		[Persist] public bool IsStarred { get; set; }
		[Persist] public string Labels { get; set; }
		[Persist] public string SendLabels { get; set; }

		[Persist] public bool ContentSynced { get; set; }
		
		[Persist] public DateTime DateCreated { get; set; }
		
		public bool IsNew { get; set; }
		public bool IsVisible { get; set; }

		public Profile Profile { get; set; }

		public Message Self
		{
			get { return this; }
		}

		public Conversation Conversation
		{
			get
			{
				if (conversation == null)
				{
					// Return a fake conversation for this message alone
					var result = new Conversation { Context = ClearContext };
					
					result.Add(this);

					return result;
				}

				return conversation;
			}
			set { conversation = value; }
		}

		public List<Label> ReceiveLabels { get; private set; }
		public List<Label> PostLabels { get; private set; }

		public AdvancedObservableCollection<Document> Documents { get; private set; }
		
		public AdvancedObservableCollection<Label> LabelsList { get; private set; }

		public ChannelConfiguration SourceChannel
		{
			get
			{
				LoadChannelConfigs();

				return sourceChannelConfig;
			}
		}

		public ChannelConfiguration TargetChannel
		{
			get
			{
				LoadChannelConfigs();

				return targetChannelConfig;
			}
		}

		public Person Person
		{
			get
			{
				if (Profile == null || Profile.Person == null)
				{
					var fake = new Person { Name = From.DisplayName ?? String.Empty };
					fake.Profiles.Add(new Profile { SourceAddress = From, Address = From.Address, ScreenName = From.DisplayName });

					return fake;
				}

				return Profile.Person;
			}
		}

		public string UniqueId
		{
			get { return EntityKeyPrefixes.Message + MessageId; }
		}

		public DateTime SortDate
		{
			get { return DateReceived ?? DateSent.Value; }
		}

		public bool IsHtml
		{
			get { return BodyHtmlStreamName.HasValue; }
		}		

		/// <summary>
		/// Returns true if this message is the last message in the conversation.
		/// </summary>
		public bool IsLast
		{
			get
			{
				// Outlook style messages view
				if (!SettingsManager.ClientSettings.AppConfiguration.RollUpConversations)
					return true;

				return Conversation.Last == this;
			}
		}		
		
		public bool HasLabel(string label)
		{
			return LabelsList.Any(l => l.Labelname.ToLower() == label);
		}

		public IEnumerable<Document> Attachments
		{
			get
			{
				return Documents.Where(d => d.ContentType != ContentType.Inline);
			}
		}

		public IEnumerable<Document> ImagesFiltered
		{
			get
			{
				return Documents.Where(d => d.IsImage && d.ContentType == ContentType.Attachment);
			}
		}

		public IEnumerable<Document> DocumentsFiltered
		{
			get
			{
				return Documents.Where(d => !d.IsImage && d.ContentType == ContentType.Attachment);
			}
		}

		public bool IsArchived
		{
			get { return MessageFolder == Folders.Archive; }
		}

		public bool IsDraft
		{
			get { return MessageFolder == Folders.Drafts; }
		}

		public bool IsSent
		{
			get { return MessageFolder == Folders.SentItems; }
		}

		public bool IsTrash
		{
			get { return MessageFolder == Folders.Trash; }
		}

		public bool IsTodo
		{
			get { return LabelsList.Any(l => l.LabelType == LabelType.Todo); }
		}

		public bool IsWaitingFor
		{
			get { return LabelsList.Any(l => l.LabelType == LabelType.WaitingFor); }
		}

		public bool IsSomeday
		{
			get { return LabelsList.Any(l => l.LabelType == LabelType.Someday); }
		}

		public bool IsChannelVisible
		{
			get
			{
				if (SourceChannel == null)
				{
					if (TargetChannel == null)
						return false;

					return TargetChannel.DisplayEnabled;
				}

				return SourceChannel.DisplayEnabled;
			}
		}

		public string ClearContext
		{
			get { return Context.ToClearSubject(); }
		}

		public Message()
		{
			MessageKey = Guid.NewGuid().GetHash();
			Metadata = new ChannelMetadata();
			DateCreated = DateTime.Now;
			
			To = new SourceAddressCollection();
			CC = new SourceAddressCollection();
			BCC = new SourceAddressCollection();

			SourceChannelId = 0;
			TargetChannelId = 0;

			Documents = new AdvancedObservableCollection<Document>();
			LabelsList = new AdvancedObservableCollection<Label>();

			ReceiveLabels = new List<Label>();
			PostLabels = new List<Label>();
			ContentSynced = true;
		}
	
		public Message(IDataReader reader) : this()
		{
			MessageId = reader.GetInt64(0);

			if (!reader.IsDBNull(1))
				MessageKey = reader.GetString(1);

			if (!reader.IsDBNull(2))
				MessageNumber = reader.GetString(2);

			if (!reader.IsDBNull(3))
				MessageIdentifier = reader.GetString(3);

			if (!reader.IsDBNull(4))
				InReplyTo = reader.GetString(4);

			if (!reader.IsDBNull(5))
				SourceFolder = reader.GetString(5);

			if (!reader.IsDBNull(6))
				Size = reader.GetInt64(6);

			if (!reader.IsDBNull(7))
				ConversationIdentifier = reader.GetString(7);

			if (!reader.IsDBNull(8))
				SourceChannelId = reader.GetInt64(8);

			if (!reader.IsDBNull(9))
				TargetChannelId = reader.GetInt64(9);

			if (!reader.IsDBNull(10))
				MessageFolder = reader.GetInt32(10);

			if (!reader.IsDBNull(11))
				From = new SourceAddress(reader.GetString(11));

			if (!reader.IsDBNull(12))
				ReturnTo = new SourceAddress(reader.GetString(12));

			if (!reader.IsDBNull(13))
				To = new SourceAddressCollection(reader.GetString(13));

			if (!reader.IsDBNull(14))
				CC = new SourceAddressCollection(reader.GetString(14));

			if (!reader.IsDBNull(15))
				BCC = new SourceAddressCollection(reader.GetString(15));

			if (!reader.IsDBNull(16))
				Context = reader.GetString(16);

			if (!reader.IsDBNull(17))
				BodyTextStreamName = reader.ReadGuidOrNull(17);

			if (!reader.IsDBNull(18))
				BodyHtmlStreamName = reader.ReadGuidOrNull(18);

			if (!reader.IsDBNull(19))
				Labels = reader.GetString(19);

			if (!reader.IsDBNull(20))
				SendLabels = reader.GetString(20);

			if (!reader.IsDBNull(21))
				IsRead = reader.ReadBoolean(21);

			if (!reader.IsDBNull(22))
				IsStarred = reader.ReadBoolean(22);

			if (!reader.IsDBNull(23))
			{
				var str = reader.GetString(23);

				if (!String.IsNullOrEmpty(str))
					TargetMessageState = (EntityStates)Enum.Parse(typeof(EntityStates), str);
			}

			if (!reader.IsDBNull(24))
				DateRead = reader.ReadDateTimeOrNull(24);

			if (!reader.IsDBNull(25))
				DateAction = reader.ReadDateTimeOrNull(25);

			if (!reader.IsDBNull(26))
				DateReply = reader.ReadDateTimeOrNull(26);

			if (!reader.IsDBNull(27))
				DateReceived = reader.ReadDateTimeOrNull(27);

			if (!reader.IsDBNull(28))
				DateSent = reader.ReadDateTimeOrNull(28);
		}

		void EnsureBodyPreview(object state)
		{
			using (var reader = ClientState.Current.DataService.ExecuteReader("select BodyPreview from Messages where MessageId=" + MessageId.Value))
			{
				if (reader.Read())
				{
					bodyPreview = reader.GetString(0);
					bodyPreviewLoaded = true;
				}
			}

			// Executes on the UI thread
			OnPropertyChanged("Self");
		}

		public void MarkDeleted()
		{
			MessageFolder = Folders.Trash;

			TrackAction(ActionType.Action, false);

			UpdateProperty("MessageFolder");

			TargetMessageState = EntityStates.Deleted;

			AsyncUpdateQueue.Enqueue(this);

			EnqueueCloudNotification(ModifyAction.Delete, true);

			EventBroker.Publish(AppEvents.MessageUpdated);			
		}

		public void MarkUndeleted()
		{
			if (IsTrash)
			{
				MessageFolder = Folders.Inbox;

				TrackAction(ActionType.Action, false);

				UpdateProperty("MessageFolder");

				TargetMessageState = EntityStates.Read;

				AsyncUpdateQueue.Enqueue(this);

				EnqueueCloudNotification(ModifyAction.Delete, false);

				EventBroker.Publish(AppEvents.MessageUpdated);				
			}
		}

		public void Purge()
		{
			MessageFolder = Folders.None;
			UpdateProperty("MessageFolder");

			TargetMessageState = EntityStates.Purged;
			
			BodyPreview = String.Empty;
			Context = String.Empty;
			ConversationIdentifier = "-1";

			if (BodyTextStreamName.HasValue)
				ClientState.Current.Storage.Delete("m", BodyTextStreamName.ToString());

			if (BodyHtmlStreamName.HasValue)
				ClientState.Current.Storage.Delete("m", BodyHtmlStreamName.ToString());

			AsyncUpdateQueue.Enqueue(this);

			ClientState.Current.Search.Delete(this);

			EventBroker.Publish(AppEvents.MessageUpdated);

			if (Conversation != null)
			{
				var mailbox = VirtualMailBox.Current;

				Conversation.Messages.Remove(this);

				// If this was the last message in the conversation, also delete the conversation
				if (Conversation.Messages.Count == 0)
				{
					using (mailbox.Conversations.WriterLock)
						mailbox.Conversations.Remove(Conversation);

					ClientState.Current.DataService.Delete(Conversation);
				}
			}
		}

		public void MarkRead()
		{
			MarkRead(true);
		}

		public void MarkRead(bool post)
		{
			if (!IsRead)
			{
				IsRead = true;

				UpdateProperty("IsRead");

				if (post)
					TargetMessageState = EntityStates.Read;

				AsyncUpdateQueue.Enqueue(this);

				EnqueueCloudNotification(ModifyAction.IsRead, true);

				EventBroker.Publish(AppEvents.MessageUpdated);
			}
		}

		public void MarkUnread()
		{
			MarkUnread(true);
		}

		public void MarkUnread(bool post)
		{
			if (IsRead)
			{
				IsRead = false;
				
				TrackAction(ActionType.Action, false);

				UpdateProperty("IsRead");

				if (post)
					TargetMessageState = EntityStates.Unread;

				AsyncUpdateQueue.Enqueue(this);

				EnqueueCloudNotification(ModifyAction.IsRead, false);

				EventBroker.Publish(AppEvents.MessageUpdated);
			}
		}

		public void SetStarred()
		{
			SetStarred(true);
		}

		public void SetUnstarred()
		{
			SetUnstarred(true);
		}

		public void SetStarred(bool post)
		{
			if (!IsStarred)
			{
				IsStarred = true;

				TrackAction(ActionType.Action, false);

				OnPropertyChanged("IsStarred");

				if (post)
					TargetMessageState = EntityStates.Starred;

				AsyncUpdateQueue.Enqueue(this);

				EnqueueCloudNotification(ModifyAction.Star, true);

				EventBroker.Publish(AppEvents.MessageUpdated);
			}
		}

		public void SetUnstarred(bool post)
		{
			if (!IsStarred)
			{
				IsStarred = false;

				TrackAction(ActionType.Action, false);

				OnPropertyChanged("IsStarred");

				if (post)
					TargetMessageState = EntityStates.Unstarred;

				AsyncUpdateQueue.Enqueue(this);

				EnqueueCloudNotification(ModifyAction.Star, false);

				EventBroker.Publish(AppEvents.MessageUpdated);
			}
		}

		public void Archive()
		{
			if (MessageFolder != Folders.Archive)
			{
				MessageFolder = Folders.Archive;

				TrackAction(ActionType.Action, false);

				TargetMessageState = EntityStates.Archived;

				AsyncUpdateQueue.Enqueue(this);

				EnqueueCloudNotification(ModifyAction.Archive, true);
	
				EventBroker.Publish(AppEvents.MessageUpdated);
			}
		}

		public void Unarchive()
		{
			if (MessageFolder == Folders.Archive)
			{
				MessageFolder = Folders.Inbox;
				
				TrackAction(ActionType.Action, false);

				OnPropertyChanged("MessageFolder");
				OnPropertyChanged("IsArchived");

				TargetMessageState = EntityStates.Unarchived;

				AsyncUpdateQueue.Enqueue(this);

				EnqueueCloudNotification(ModifyAction.Archive, false);

				EventBroker.Publish(AppEvents.MessageUpdated);
			}
		}

		public void MoveToFolder(int folder)
		{
			MessageFolder = folder;

			TrackAction(ActionType.Action, false);

			OnPropertyChanged("MessageFolder");

			AsyncUpdateQueue.Enqueue(this);

			EnqueueCloudNotification(ModifyAction.Folder, folder);

			EventBroker.Publish(AppEvents.MessageUpdated);
		}

		bool IsManaged()
		{
			if (SourceChannel == null)
				return false;

			return SourceChannel.IsConnected;
		}

		void EnqueueCloudNotification(ModifyAction action, object value)
		{
		}

		public void TrackAction(ActionType action)
		{
			TrackAction(action, DateTime.Now, true);
		}

		public void TrackAction(ActionType action, DateTime date)
		{
			TrackAction(action, date, true);
		}

		void TrackAction(ActionType action, bool save)
		{
			TrackAction(action, DateTime.Now, save);
		}

		void TrackAction(ActionType action, DateTime date, bool save)
		{
			bool changed = false;

			switch (action)
			{
				case ActionType.Read:
					if (!DateRead.HasValue)
					{
						changed = true;
						DateRead = date;
					}

					break;

				case ActionType.Action:
					if (!DateAction.HasValue)
					{
						DateAction = date;
						changed = true;
					}

					break;

				case ActionType.ReplyForward:
					if (!DateReply.HasValue)
					{
						DateReply = date;
						changed = true;
					}

					break;
			}

			if (changed && save)
				AsyncUpdateQueue.Enqueue(this);
		}

		public void UpdateProperty(string propertyName)
		{
			OnPropertyChanged(propertyName);
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				Thread.CurrentThread.RaiseUIPropertyChanged(this, propertyName, PropertyChanged);
			}
		}		

		public void Add(Document document)
		{
			Documents.Add(document);

			document.Message = this;
		}

		public void SetLabels(string labels)
		{
			LabelsList.Replace(
				labels.SmartSplit("|")
					.Select(l => new Label(l)));

			EventBroker.Publish(AppEvents.MessageLabelsUpdated, this);
		}

		public void AddLabel(Label label)
		{
			AddLabel(label, true);
		}

		public void AddLabel(Label label, bool post)
		{
			var mailbox = VirtualMailBox.Current;

			using (mailbox.Labels.ReaderLock)
			{
				string key = label.Labelname.ToLower();

				if (!mailbox.Labels.ContainsKey(key))
				{
					mailbox.Labels.Add(key, new List<Label>());

					if (label.LabelType == LabelType.Custom)
						EventBroker.Publish(AppEvents.LabelCreated, key);
				}

				if (LabelsList.Any(l => l.Equals(label)))
					return;

				label.Messages.Add(this);

				mailbox.Labels[key].Add(label);

				LabelsList.Add(label);

				if (post)
				{
					PostLabels.Add(label);

					SendLabels = String.Join("|", PostLabels.Select(s => s.ToString()).ToArray());

					EnqueueCloudNotification(ModifyAction.Label, String.Concat("%2B", label.Labelname)); // + (url encoded)
				}
			}
		
			// Recreate and save labels list
			Labels = String.Join("|", LabelsList.Select(s => s.ToString()).ToArray());

			AsyncUpdateQueue.Enqueue(this);

			OnPropertyChanged("IsTodo");
			OnPropertyChanged("IsWaitingFor");
			OnPropertyChanged("IsSomeday");

			EventBroker.Publish(AppEvents.MessageUpdated);
			EventBroker.Publish(AppEvents.MessageLabelsUpdated, this);
		}

		public void RemoveLabel(Label label)
		{
			RemoveLabel(label, true);
		}

		public void RemoveLabel(Label label, bool post)
		{
			LabelsList.Remove(label);

			label.Messages.Remove(this);

			if (post)
			{
				// If label allready is in post but has 
				// not been posted yet, remove it
				if (PostLabels.Contains(label))
					PostLabels.Remove(label);
				else
					PostLabels.Add(label);

				SendLabels = String.Join("|", PostLabels.Select(s => s.ToString()).ToArray());

				EnqueueCloudNotification(ModifyAction.Label, String.Concat("-", label.Labelname));
			}

			// Recreate and save labels list
			Labels = String.Join("|", LabelsList.Select(s => s.ToString()).ToArray());

			AsyncUpdateQueue.Enqueue(this);

			OnPropertyChanged("IsTodo");
			OnPropertyChanged("IsWaitingFor");
			OnPropertyChanged("IsSomeday");

			EventBroker.Publish(AppEvents.MessageUpdated);
			EventBroker.Publish(AppEvents.MessageLabelsUpdated, this);
		}


		void LoadChannelConfigs()
		{
			if (!configLoaded)
			{
				if (SourceChannelId > 0)
				{
					var channel = ChannelsManager.GetChannelObject(SourceChannelId);

					if (channel != null)
						sourceChannelConfig = channel.Configuration;
				}

				if (TargetChannelId > 0)
				{
					var channel = ChannelsManager.GetChannelObject(TargetChannelId);

					if (channel != null)
						targetChannelConfig = channel.Configuration;
				}

				configLoaded = true;
			}
		}

		public override string ToString()
		{
			return String.Format("[{0} {1}]", MessageNumber, Context);
		}

		#region Implementation of IDataServiceHooks

		public void BeforeSave()
		{
		}

		public void AfterSave()
		{
		}

		public void BeforeUpdate()
		{
			EnsureBodyPreview(null);
		}

		public void AfterUpdate()
		{
		}

		public void BeforeDelete()
		{
		}

		public void AfterDelete()
		{
		}

		#endregion
	}
}