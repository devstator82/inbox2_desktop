using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Platform.Channels.Interfaces
{
	[DataContract]
	public enum ConnectionType
	{
		[EnumMember(Value = "System")]
		System,

		[EnumMember(Value = "Smtp")]
		Smtp,

		[EnumMember(Value = "Pop3")]
		Pop3
	}
}
