using System.Runtime.Serialization;

namespace Inbox2.Platform.Channels.Entities
{
    [DataContract]
    public enum EventPriority
    {
        [EnumMember] None = 0,
        [EnumMember] High = 1,
        [EnumMember] Normal = 2,
        [EnumMember] Low = 3,
    }
}
