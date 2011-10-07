using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Platform.Channels.Entities
{
	[Serializable]
	[DataContract]
	public enum ContentType
	{
		[EnumMember(Value = "Unknown")]
		Unknown = 0,

		[EnumMember(Value = "Attachment")]
		Attachment = 10,

		[EnumMember(Value = "Inline")]
		Inline = 20
	}
}
