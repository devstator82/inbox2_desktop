using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Persistance;
using Inbox2.Framework.Threading.AsyncUpdate;
using Inbox2.Framework.UI.AsyncImage;
using Inbox2.Platform.Channels.Attributes;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.CloudApi.Enumerations;
using Inbox2.Platform.Framework.Collections;
using Inbox2.Platform.Framework.Extensions;

namespace Inbox2.Framework.VirtualMailBox.Entities
{
	[PersistableClass(TableName = "UserStatus")]
	[Serializable]
	public class UserStatus : IEntityBase
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private AsyncHttpImage loader;
		private ChannelConfiguration config;
		private bool configLoaded;

		[IndexAndStore]
		[PrimaryKey] public long StatusId { get; set; }
		[Persist] public string StatusKey { get; set; }
		[Persist] public string ParentKey { get; set; }
		[Persist] public string ChannelStatusKey { get; set; }

		[Persist] public long ProfileId { get; set; }

		[Persist] public long SourceChannelId { get; set; }
		[Persist] public string TargetChannelId { get; set; }		

		[Persist] public SourceAddress From { get; set; }
		[Persist] public SourceAddress To { get; set; }

		[IndexTokenizeAndStore]
		[Persist] public string Status { get; set; }

		[Persist] public string InReplyTo { get; set; }
		[Persist] public int StatusType { get; set; }
		[Persist] public string SearchKeyword { get; set; }

		[Persist] public bool IsRead { get; set; }

		[Persist] public DateTime? DateRead { get; set; }
		[Persist] public DateTime SortDate { get; set; }
		[Persist] public DateTime DateCreated { get; set; }

		public bool IsNew { get; set; }

		public AdvancedObservableCollection<UserStatus> Children { get; set; }
		public AdvancedObservableCollection<UserStatusAttachment> Attachments { get; set; }

		public IEnumerable<UserStatusAttachment> Images
		{
			get { return Attachments.Where(a => a.HasPreview); }
		}

		public IEnumerable<UserStatusAttachment> Documents
		{
			get { return Attachments.Where(a => a.HasPreview == false && !String.IsNullOrEmpty(a.PreviewAltText)); }
		}

		public IEnumerable<UserStatusAttachment> Links
		{
			get { return Attachments.Where(a => a.HasPreview == false && String.IsNullOrEmpty(a.PreviewAltText)); }
		}

		public ImageSource Avatar
		{
			get
			{
				if (String.IsNullOrEmpty(From.AvatarUrl))
				{
					return AsyncImageQueue._Profile;
				}

				if (loader == null)
				{
					loader = new AsyncHttpImage(From.AvatarUrl, () => OnPropertyChanged("Avatar"), () => loader = null);
				}

				return loader.AsyncSource;
			}
		}

		public DateTime ParentSortDate
		{
			get
			{
				if (Parent == null)
					return SortDate;

				return Parent.SortDate;
			}
		}

		public ChannelConfiguration SourceChannel
		{
			get
			{
				if (!configLoaded)
				{
					var channel = ChannelsManager.GetChannelObject(SourceChannelId);

					if (channel != null)
						config = channel.Configuration;

					configLoaded = true;
				}

				return config;
			}
		}

		public IEnumerable<ChannelConfiguration> TargetChannels
		{
			get
			{
				string[] parts = TargetChannelId.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

				return parts.Select(p => Int64.Parse(p.Trim())).Where(p => p != 0)
					.Select(p => ChannelsManager.GetChannelObject(p).Configuration);
			}
		}

		public bool IsSearchResult
		{
			get { return !String.IsNullOrEmpty(SearchKeyword); }
		}

		public UserStatus Parent { get; set; }		

		#region StreamView

	    public bool IsSingleRoot
	    {
	        get
	        {
	            return IsFirst && Children.Count == 0;
	        }
	    }

		/// <summary>
		/// Returns true if this message is the first message in the conversation (root).
		/// </summary>
		public bool IsFirst
		{
			get { return Parent == null; }
		}

		/// <summary>
		/// Returns true if this message is a reply in the conversation (non-root).
		/// </summary>
		public bool IsReply
		{
			get { return !IsFirst; }
		}

		/// <summary>
		/// Returns true if this message is the first reply.
		/// </summary>
		public bool IsFirstReply
		{
			get
			{
				if (IsFirst) return false;

				return Parent.Children.First() == this;
			}
		}

		/// <summary>
		/// Returns true if this message is a reply in the conversation (non-root).
		/// </summary>
		public bool IsMiddleReply
		{
			get
			{
				if (IsFirst)
					return false;

				if (IsFirstReply || IsLastReply)
					return false;

				return true;
			}
		}

		/// <summary>
		/// Returns true if this message is the last message in the conversation.
		/// </summary>
		public bool IsLast
		{
			get { return Parent.Children.Last() == this; }
		}

		/// <summary>
		/// Returns true if this message is the last reply.
		/// </summary>
		public bool IsLastReply
		{
			get
			{
				if (IsFirst)
					return false;

				return IsLast;
			}
		}

		/// <summary>
		/// Returns true if this is the only reply in a conversation.
		/// </summary>
		public bool IsSingleReply
		{
			get
			{
				return IsFirstReply && IsLastReply;
			}
		}

		#endregion

		public UserStatus()
		{
			DateCreated = DateTime.Now;
			Children = new AdvancedObservableCollection<UserStatus>();
			Attachments = new AdvancedObservableCollection<UserStatusAttachment>();
			StatusKey = Guid.NewGuid().GetHash();
		}

		public UserStatus(IDataReader reader) : this()
		{
			StatusId = reader.GetInt64(0);
			StatusKey = reader.GetString(1);
			ParentKey = reader.GetString(2);

			ProfileId = reader.GetInt64(3);
			
			SourceChannelId = reader.GetInt64(4);
			TargetChannelId = reader.GetString(5);

			ChannelStatusKey = reader.GetString(6);

			From = new SourceAddress(reader.GetString(7));
			To = new SourceAddress(reader.GetString(8));

			Status = reader.GetString(9);

			InReplyTo = reader.GetString(10);
			StatusType = reader.GetInt32(11);

			SearchKeyword = reader.GetString(12);
			IsRead = reader.ReadBoolean(13);

			SortDate = reader.ReadDateTime(14);
			DateRead = reader.ReadDateTimeOrNull(15);
			DateCreated = reader.ReadDateTime(16);
		}

		public string UniqueId
		{
			get { return EntityKeyPrefixes.UserStatus + StatusId; }
		}

		public void UpdateProperty(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public void Add(UserStatus status)
		{
			status.Parent = this;

			Children.Add(status);
		}

		public void MarkRead()
		{
			IsRead = true;

			Children.ForEach(u => u.MarkRead());

			EnqueueCloudNotification(ModifyAction.IsRead, true);

			OnPropertyChanged("IsRead");
		}

		bool IsManaged()
		{
			if (SourceChannel == null)
				return false;

			return SourceChannel.IsConnected;
		}

		void EnqueueCloudNotification(ModifyAction action, object value)
		{
			//if (IsManaged())
			//    CommandQueue.Enqueue(AppCommands.SyncStatusUpdate, this, ExecutionTrigger.Connection, value, action);
		}

		public void TrackAction(ActionType action)
		{
			TrackAction(action, DateTime.Now, true);
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
			}

			if (changed && save)
				AsyncUpdateQueue.Enqueue(this);
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				Thread.CurrentThread.RaiseUIPropertyChanged(this, propertyName, PropertyChanged);
			}
		}		
	}
}


