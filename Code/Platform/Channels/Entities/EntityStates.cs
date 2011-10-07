using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Platform.Channels.Entities
{
	[Serializable]
	[DataContract]
	public enum EntityStates : uint
	{
		[EnumMember]
		Read = 0,

		[EnumMember]
		Unread = 1,
		
		[EnumMember]
		Deleted = 2,

		[EnumMember]
		Starred = 3,

		[EnumMember]
		Unstarred = 4,

		[EnumMember]
		Archived = 5,

		[EnumMember]
		Unarchived = 6,

		[EnumMember]
		Moved = 7,

		[EnumMember]
		Labeled = 8,

		[EnumMember]
		Purged = 9,

		[EnumMember]
		Spam = 10,
	}
}
