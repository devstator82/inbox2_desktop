using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Attributes;

namespace Inbox2.Platform.Channels.Entities
{
	public class ChannelAttachment
	{
		public string Filename { get; set; }

		public long SourceChannelId { get; set; }
		public long TargetChannelId { get; set; }

		public ContentType ContentType { get; set; }

		public string ContentId { get; set; }

		public Stream ContentStream { get; set; }	

		public ChannelAttachment()
		{
		}
	}
}