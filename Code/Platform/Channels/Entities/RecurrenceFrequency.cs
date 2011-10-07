using System;
using System.Runtime.Serialization;

namespace Inbox2.Platform.Channels.Entities
{
	[Serializable]
    [DataContract]
    public enum RecurrenceFrequency
    {
        [EnumMember] Secondly = 0,
        [EnumMember] Minutely = 1,
        [EnumMember] Hourly = 2,
        [EnumMember] Daily = 3,
        [EnumMember] Weekly = 4,
        [EnumMember] Monthly = 5,
        [EnumMember] Yearly = 6,
    }
}
