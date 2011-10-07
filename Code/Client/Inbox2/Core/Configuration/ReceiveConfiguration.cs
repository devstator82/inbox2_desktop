using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Core.Configuration
{
	public class ReceiveConfiguration
	{
		public short ReceiveInterval { get; set; }

		public short SyncStatusUpdatesInterval { get; set; }

		public short SyncSearchStreamInterval { get; set; }

		public short SyncContactsInterval { get; set; }

		public ReceiveConfiguration()
		{
			ReceiveInterval = 5;
			SyncStatusUpdatesInterval = 5;
			SyncSearchStreamInterval = 5;
			SyncContactsInterval = 60;
		}
	}
}
