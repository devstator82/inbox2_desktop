using System;
using System.Collections.Generic;
using System.IO;

namespace Inbox2.Platform.Channels.Entities
{
	public class ChannelMessage
	{
		public string MessageNumber { get; set; }
		public string MessageIdentifier { get; set; }
		public string InReplyTo { get; set; }
		public string SourceFolder { get; set; }
		
		public long SourceChannelId { get; set; }
		public long TargetChannelId { get; set; }

		public string ConversationId { get; set; }

		public string Context { get; set; }

		public MemoryStream BodyText { get; set; }
		public MemoryStream BodyHtml { get; set; }
	
		public long Size { get; set; }

		public SourceAddress From { get; set; }
		public SourceAddress ReturnTo { get; set; }
		public SourceAddressCollection To { get; set; }
		public SourceAddressCollection CC { get; set; }
		public SourceAddressCollection BCC { get; set; }

		public bool IsRead { get; set; }
		public bool IsStarred { get; set; }
		
		public DateTime? DateReceived { get; set; }
		public DateTime? DateSent { get; set; }

		public ChannelMetadata Metadata { get; set; }
		public List<ChannelAttachment> Attachments { get; set; }

		public ChannelMessage()
		{
			To = new SourceAddressCollection();
			CC = new SourceAddressCollection();
			BCC = new SourceAddressCollection();
			Metadata = new ChannelMetadata();
			Attachments = new List<ChannelAttachment>();			
		}
	}
}