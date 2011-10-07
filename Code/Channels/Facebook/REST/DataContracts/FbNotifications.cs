using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Channels.Facebook.REST.DataContracts
{
	public class FbNotifications
	{
		public string MostRecentMessage { get; set; }

		public List<string> FriendRequests { get; set; }

		public FbNotifications()
		{
			FriendRequests = new List<string>();
		}
	}
}
