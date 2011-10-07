using System;
using System.Runtime.Serialization;

namespace Inbox2.Platform.Channels.Entities
{
	[Serializable]
	[DataContract]
	public enum EventClassType
	{
		[EnumMember] Public = 0,
		[EnumMember] Private = 1,
		[EnumMember] Confidential = 2,
	}
}
