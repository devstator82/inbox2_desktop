using System;
using System.Linq;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Logging;

namespace Inbox2.Core.Threading.Tasks.Sync
{
	internal class UserStatusParser
	{
		private readonly ChannelConfiguration config;		
		private readonly VirtualMailBox mailbox;
		private readonly string searchKeyword;

		public bool IsRead { get; set; }

		bool IsInSearchMode
		{
			get { return !String.IsNullOrEmpty(searchKeyword); }
		}

		public UserStatusParser(ChannelConfiguration config)
		{
			this.config = config;
			this.mailbox = VirtualMailBox.Current;
		}

		public UserStatusParser(ChannelConfiguration config, string searchKeyword) : this(config)
		{
			this.searchKeyword = searchKeyword;
		}

		public void ProcessStatusUpdate(ChannelStatusUpdate statusupdate, int statusType)
		{
			try
			{
				ChannelStatusUpdate statusupdate1 = statusupdate;
				UserStatus status;

				using (mailbox.StatusUpdates.ReaderLock)
					if (String.IsNullOrEmpty(searchKeyword))
						status = mailbox.StatusUpdates.FirstOrDefault(
							s => s.SourceChannelId == config.ChannelId
								 && s.ChannelStatusKey == statusupdate1.ChannelStatusKey
								 && s.StatusType == statusType);
					else
						status = mailbox.StatusUpdates.FirstOrDefault(
							s => s.SearchKeyword == searchKeyword
								 && s.ChannelStatusKey == statusupdate1.ChannelStatusKey
								 && s.StatusType == statusType);

				if (status != null)
				{
					// Allready have this status update
					if (status.Children.Count == statusupdate1.Children.Count)
						return;

					// Have received more replies to an existing thread
					foreach (var child in statusupdate1.Children)
					{
						var child1 = child;
						var oldChild = status.Children.FirstOrDefault(s => s.ChannelStatusKey == child1.ChannelStatusKey);

						if (oldChild == null)
							ProcessChild(status, child1, statusType);
					}
				}
				else
				{
					// Have received a new thread
					status = ProcessParent(statusupdate1, statusType);

					foreach (var child in statusupdate1.Children)
						ProcessChild(status, child, statusType);

					if (!String.IsNullOrEmpty(searchKeyword))
						status.SearchKeyword = searchKeyword;
				}
			}
			catch (Exception ex)
			{
				Logger.Error("An error has occured while matching status update. Exception = {0}", LogSource.AppServer, ex);
			}
		}

		UserStatus ProcessParent(ChannelStatusUpdate statusupdate, int statusType)
		{
			UserStatus status = ParseStatusUpdate(statusupdate, statusType);

			ClientState.Current.DataService.Save(status);
			status.Attachments.ForEach(ClientState.Current.DataService.Save);

			mailbox.StatusUpdates.Add(status);

			EventBroker.Publish(AppEvents.StatusUpdateReceived, status);

			return status;
		}

		void ProcessChild(UserStatus status, ChannelStatusUpdate child, int statusType)
		{
			var childStatus = ParseStatusUpdate(child, statusType);

			childStatus.ParentKey = status.StatusKey;
			childStatus.Attachments.ForEach(ClientState.Current.DataService.Save);

			ClientState.Current.DataService.Save(childStatus);

			status.Add(childStatus);

			EventBroker.Publish(AppEvents.StatusUpdateReceived, status);
		}

		public UserStatus ParseStatusUpdate(ChannelStatusUpdate statusupdate, int statusType)
		{
			var status = new UserStatus
			{
				From = statusupdate.From,
				To = statusupdate.To,
				Status = statusupdate.Status,
				StatusType = statusType,
				SortDate = statusupdate.DatePosted,
				SourceChannelId = IsInSearchMode ? 0 : config.ChannelId,
				ChannelStatusKey = statusupdate.ChannelStatusKey,
				SearchKeyword = searchKeyword,
				IsRead = IsRead,
				IsNew = true,
				DateCreated = DateTime.Now,
			};

			foreach (var statusattachment in statusupdate.Attachments)
			{
				var attachment = new UserStatusAttachment
				    {
				        StatusKey = status.StatusKey,
				        PreviewAltText = statusattachment.PreviewAltText,
				        PreviewImageUrl = statusattachment.PreviewImageUrl,
				        TargetUrl = statusattachment.TargetUrl,
				        MediaType = statusattachment.MediaType,
				        DateCreated = DateTime.Now
				    };

				status.Attachments.Add(attachment);
			}

			return status;
		}
	}
}
