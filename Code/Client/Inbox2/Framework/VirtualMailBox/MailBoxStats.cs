using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Framework.VirtualMailBox
{
	public static class MailBoxStats
	{
		public static double GetAverageEmailsSent()
		{
			var mailbox = VirtualMailBox.Current;

			using (mailbox.Persons.ReaderLock)
				return mailbox.Persons
					.Where(p => p.Messages.Count > 0)
					.Average(p => p.Messages.Count(m => m.MessageFolder == Folders.SentItems));
		}

		public static double GetAverageEmailsReceived()
		{
			var mailbox = VirtualMailBox.Current;

			using (mailbox.Persons.ReaderLock)
				return mailbox.Persons
					.Where(p => p.Messages.Count > 0)
					.Average(p => p.Messages.Count(m => m.MessageFolder != Folders.SentItems));
		}
	}
}
