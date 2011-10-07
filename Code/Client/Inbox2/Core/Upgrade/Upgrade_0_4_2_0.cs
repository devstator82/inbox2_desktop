using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.Deployment;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Logging;

namespace Inbox2.Core.Upgrade
{
    class Upgrade_0_4_2_0 : UpgradeActionBase
    {
        public override Version TargetVersion
        {
            get { return new Version("0.4.2.0"); }
        }

        protected override void UpgradeCore()
        {
			ClientState.Current.DataService.ExecuteNonQuery("alter table [Messages] add column [DateRead] text");
			ClientState.Current.DataService.ExecuteNonQuery("alter table [Messages] add column [DateAction] text");
			ClientState.Current.DataService.ExecuteNonQuery("alter table [Messages] add column [DateReply] text");
			ClientState.Current.DataService.ExecuteNonQuery("alter table [UserStatus] add column [DateRead] text");
			ClientState.Current.DataService.ExecuteNonQuery("alter table [Persons] add column [CustomScore] text");
        }

		public override void AfterLoadUpgradeAsync()
		{
			var mailbox = VirtualMailBox.Current;
			List<Message> messages;

			using (mailbox.Messages.ReaderLock)
				messages = mailbox.Messages.Where(m => !String.IsNullOrEmpty(m.InReplyTo)).ToList();

			foreach (var message in messages)
			{
				// Try to find the source
				Message source;
				Message message1 = message;

				using (mailbox.Messages.ReaderLock)
					source = mailbox.Messages.FirstOrDefault(m => m.MessageIdentifier == message1.InReplyTo);

				if (source != null)
				{
					Logger.Debug("Upgrade_0_4_2_0: Found source for message {0}, tracking action", LogSource.Startup, message);

					source.TrackAction(ActionType.ReplyForward, message.SortDate);
				}
				else
				{
					Logger.Debug("Upgrade_0_4_2_0: Unable to find source for message {0}", LogSource.Startup, message);
				}
			}
		}
    }
}