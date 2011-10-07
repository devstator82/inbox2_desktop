using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Platform.Channels.Entities
{
	[DataContract]
	public class ChannelStatusUpdate
	{
		[DataMember(Order = 1)]
		public string ChannelStatusKey { get; set; }

		[DataMember(Order = 2)]
		public SourceAddress From { get; set; }

		[DataMember(Order = 3)]
		public SourceAddress To { get; set; }

		[DataMember(Order = 4)]
		public string Status { get; set; }

		[DataMember(Order = 5)]
		public string InReplyTo { get; set; }

		[DataMember(Order = 6)]
		public DateTime DatePosted { get; set; }

		[DataMember(Order = 7)]
		public List<ChannelStatusUpdate> Children { get; set; }

		[DataMember(Order = 8)]
		public List<ChannelStatusUpdateAttachment> Attachments { get; set; }

		public DateTime SortDate
		{
			get
			{
				if (Children.Count > 0)
					return Children.Max(s => s.DatePosted);

				return DatePosted;
			}
		}

		public ChannelStatusUpdate()
		{
			Children = new List<ChannelStatusUpdate>();
			Attachments = new List<ChannelStatusUpdateAttachment>();
		}
	}
}
