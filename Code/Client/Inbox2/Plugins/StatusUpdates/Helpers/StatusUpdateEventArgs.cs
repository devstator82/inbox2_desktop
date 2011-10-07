using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework.VirtualMailBox.Entities;

namespace Inbox2.Plugins.StatusUpdates.Helpers
{
	public enum StatusUpdateAction
	{
		New,
		Reply,
		Share
	}

	public class StatusUpdateEventArgs : EventArgs
	{
		public UserStatus Status { get; private set; }

		public string StatusText { get; private set; }

		public long ChannelId { get; private set; }

		public StatusUpdateAction Action { get; private set; }

		public StatusUpdateEventArgs(UserStatus status, string statusText, long channelId, StatusUpdateAction action)
		{
			Status = status;
			StatusText = statusText;
			ChannelId = channelId;
			Action = action;
		}
	}
}