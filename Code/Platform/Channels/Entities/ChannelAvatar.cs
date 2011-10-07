using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Channels.Attributes;

namespace Inbox2.Platform.Channels.Entities
{
	[DataContract]
	public class ChannelAvatar
	{
		[DataMember(Order = 1)]
		public string Url { get; set; }

		[DataMember(Order = 2)]
		public string Description { get; set; }

		[DataMember(Order = 3)]
		public short Width { get; set; }

		[DataMember(Order = 4)]
		public short Height { get; set; }

        public Stream ContentStream { get; set; }
	}
}