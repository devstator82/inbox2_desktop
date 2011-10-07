using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.VirtualMailBox.Entities;

namespace Inbox2.Core.Threading.Handlers
{
	public class UserStatusHandler
	{
		public static void Init()
		{
			EventBroker.Subscribe<UserStatus>(AppEvents.StatusUpdateReceived, StatusUpdateReceived);	
		}

		public static void StatusUpdateReceived(UserStatus status)
		{
			// Add to search index
			ClientState.Current.Search.Store(status);
		}
	}
}
