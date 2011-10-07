using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Platform.Channels.Entities
{
	[DataContract]
	public class ChannelStatusUpdateAttachment
	{
		[DataMember(Order = 1)]
		public string PreviewImageUrl { get; set; }

		[DataMember(Order = 2)]
		public string PreviewAltText { get; set; }

		[DataMember(Order = 3)]
		public string TargetUrl { get; set; }

		[DataMember(Order = 4)]
		public short MediaType { get; set; }
	}
}
