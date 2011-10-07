using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Inbox2.Core.Threading.Handlers.Matchers;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Threading;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Logging;

namespace Inbox2.Core.Threading.Tasks.Application
{
	public class PurgeDataTask : BackgroundTask
	{
		private readonly VirtualMailBox mailbox;

		public PurgeDataTask()
		{
			mailbox = VirtualMailBox.Current;
		}

		protected override void ExecuteCore()
		{
			List<UserStatus> delete;

			using (mailbox.StatusUpdates.ReaderLock)
				delete = mailbox.StatusUpdates.Where(u => u.SortDate < DateTime.Now.AddDays(-7)).ToList();

			foreach (var status in delete)
			{
				try
				{
					ClientState.Current.DataService.Delete(status);

					ClientState.Current.Search.Delete(status);					
				}
				catch (Exception ex)
				{
					Logger.Error("An error has occured while purging userstatus, Exception = {0}", LogSource.BackgroundTask, ex);
				}
			}

			Thread.CurrentThread.ExecuteOnUIThread(() => delete.ForEach(d => mailbox.StatusUpdates.Remove(d)));			

			// Find messages that have not been matched
			List<Message> messages;

			using (mailbox.Messages.ReaderLock)
				messages = mailbox.Messages.Where(m => m.ConversationIdentifier == String.Empty && m.DateCreated < DateTime.Now.AddMinutes(1)).ToList();

			foreach (var message in messages)
			{
				try
				{
					MessageMatcher.Match(message);
				}
				catch (Exception ex)
				{
					Logger.Error("An error has occured while trying to match old message. Exception = {0}", LogSource.BackgroundTask, ex);					
				}
			}
		}
	}
}
