using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Platform.Interfaces.Enumerations
{
	[DataContract]
	public enum EntityType
	{
		[EnumMember(Value = "User")]
		User = 0,

		[EnumMember(Value = "Message")]
		Message = 10,

		[EnumMember(Value = "Conversation")]
		Conversation = 20,

		[EnumMember(Value = "Document")]
		Document = 30,

		[EnumMember(Value = "DocumentVersion")]
		DocumentVersion = 31,

		[EnumMember(Value = "Note")]
		Note = 40,

		[EnumMember(Value = "Person")]
		Person = 50,

		[EnumMember(Value = "Profile")]
		Profile = 60,

		[EnumMember(Value = "Avatar")]
		Avatar = 70,

		[EnumMember(Value = "UserStatus")]
		UserStatus = 80,

		[EnumMember(Value = "UserStatusAttachment")]
		UserStatusAttachment = 90,
	}
}
