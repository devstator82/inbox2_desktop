using System;
using System.Linq;
using Inbox2.Framework;
using Inbox2.Framework.Persistance;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Logging;

namespace Inbox2ClientWorker.Core.Threading.Tasks.Sync
{
	internal class UserStatusParser
	{
		private readonly ChannelConfiguration config;		
		private readonly string searchKeyword;
		private readonly IDataService dataService;

		bool IsInSearchMode
		{
			get { return !String.IsNullOrEmpty(searchKeyword); }
		}

		public UserStatusParser(ChannelConfiguration config)
		{
			this.dataService = ClientState.Current.DataService;
			this.config = config;
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

				if (String.IsNullOrEmpty(searchKeyword))
					status = dataService.SelectBy<UserStatus>(new
					{
						SourceChannelId = config.ChannelId,
						ChannelStatusKey = statusupdate1.ChannelStatusKey,
						StatusType = statusType 
					});
				else
					status = dataService.SelectBy<UserStatus>(new
					{
						SearchKeyword = searchKeyword,
						ChannelStatusKey = statusupdate1.ChannelStatusKey,
						StatusType = statusType
					});

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

			ClientState.Current.Search.Store(status);

			return status;
		}

		void ProcessChild(UserStatus status, ChannelStatusUpdate child, int statusType)
		{
			var childStatus = ParseStatusUpdate(child, statusType);

			childStatus.ParentKey = status.StatusKey;
			childStatus.Attachments.ForEach(ClientState.Current.DataService.Save);

			ClientState.Current.DataService.Save(childStatus);

			ClientState.Current.Search.Store(status);
		}

		UserStatus ParseStatusUpdate(ChannelStatusUpdate statusupdate, int statusType)
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
				IsRead = false,
				IsNew = true,
				DateCreated = DateTime.Now,
			};

			foreach (var statusattachment in statusupdate.Attachments)
			{
				var attachment = new UserStatusAttachment();

				attachment.StatusKey = status.StatusKey;
				attachment.PreviewAltText = statusattachment.PreviewAltText;
				attachment.PreviewImageUrl = statusattachment.PreviewImageUrl;
				attachment.TargetUrl = statusattachment.TargetUrl;
				attachment.MediaType = statusattachment.MediaType;
				attachment.DateCreated = DateTime.Now;

				status.Attachments.Add(attachment);
			}

			return status;
		}
	}
}
