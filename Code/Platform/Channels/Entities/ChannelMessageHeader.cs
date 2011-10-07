using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Inbox2.Platform.Channels.Entities
{
	[DataContract]
	public class ChannelMessageHeader
	{
		[DataMember(Order = 1)]
		public string MessageNumber { get; set; }

		[DataMember(Order = 2)]
		public string MessageIdentifier { get; set; }

		[DataMember(Order = 3)]
		public string InReplyTo { get; set; }

		[DataMember(Order = 4)]
		public string SourceFolder { get; set; }

		[DataMember(Order = 5)]
		public long Size { get; set; }

		[DataMember(Order = 6)]
		public long SourceChannelId { get; set; }

		[DataMember(Order = 7)]
		public string Context { get; set; }

		[DataMember(Order = 8)]
		public SourceAddress From { get; set; }

		[DataMember(Order = 9)]
		public SourceAddress ReturnTo { get; set; }

		[DataMember(Order = 10)]
		public SourceAddressCollection To { get; set; }

		[DataMember(Order = 11)]
		public SourceAddressCollection CC { get; set; }

		[DataMember(Order = 12)]
		public SourceAddressCollection BCC { get; set; }

		[DataMember(Order = 13)]
		public ChannelMetadata Metadata { get; set; }

		[DataMember(Order = 14)]
		public bool IsRead { get; set; }

		[DataMember(Order = 15)]
		public bool IsStarred { get; set; }

		[DataMember(Order = 16)]
		public string Body { get; set; }

		[DataMember(Order = 17)]
		public DateTime DateReceived { get; set; }

		public ChannelMessageHeader()
		{
			To = new SourceAddressCollection();
			CC = new SourceAddressCollection();
			BCC = new SourceAddressCollection();
			Metadata = new ChannelMetadata();
		}
		
		public override string ToString()
		{
			return String.Format("[{0} {1}]", MessageNumber, Context);
		}
	}
}