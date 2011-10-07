using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Channels.Configuration
{
	[Serializable]
	public class ChannelCharasteristics
	{
		public bool CanNew { get; set; }

		public bool CanReply { get; set; }

		public bool CanReceive { get; set; }

		public bool CanSend { get; set; }

		public bool CanSendFiles { get; set; }

		public bool SupportsHtml { get; set; }

		public bool SupportsEmail { get; set; }

		public bool SupportsPublicMessage { get; set; }

		public bool SupportsPrivateMessage { get; set; }

		public bool SupportsMobileMessage { get; set; }

		public bool SupportsStatusUpdates { get; set; }

		public bool SupportsStatusUpdateMentions { get; set; }

		public bool SupportsStatusUpdatesSearch { get; set; }

		public bool SupportsStatusUpdatesReply { get; set; }

		public bool SupportsProfiles { get; set; }

		public bool SupportsSubject { get; set; }

		public bool SupportsReadStates { get; set; }

		public bool SupportsLabels { get; set; }

		public long MaxSubjectChars { get; set; }

		public long MaxBodyChars { get; set; }

		public bool CanCustomize { get; set; }

		public static ChannelCharasteristics Default
		{
			get 
			{ 
				return new ChannelCharasteristics 
			       	{ 
			       		CanNew = true, 
			       		CanReceive = true, 
			       		CanReply = true, 
			       		CanSend = true, 
			       		CanSendFiles = true, 
			       		SupportsHtml = true, 
			       		SupportsReadStates = false,
			       		SupportsEmail = true,
			       		SupportsSubject = true,
						SupportsStatusUpdates = false,
						SupportsStatusUpdatesSearch = false,
			       		SupportsPrivateMessage = true, 
			       		SupportsPublicMessage = false, 
			       		SupportsMobileMessage = false,
			       		MaxBodyChars = -1, 
			       		MaxSubjectChars = -1 
			       	}; 
			}
		}
	}
}