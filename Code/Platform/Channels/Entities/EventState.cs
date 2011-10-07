using System;
using System.Runtime.Serialization;

namespace Inbox2.Platform.Channels.Entities
{
	[Serializable]
    [DataContract]
    public enum EventState
    {
        [EnumMember] Tentative = 0,
        [EnumMember] Confirmed = 1,
        [EnumMember] Cancelled = 2,
    }
}
