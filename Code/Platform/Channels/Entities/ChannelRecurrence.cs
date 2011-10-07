using Inbox2.Platform.Channels.Attributes;
using System;

namespace Inbox2.Platform.Channels.Entities
{
    public class ChannelRecurrence
    {
        public RecurrenceFrequency Frequency { get; set; }
        public DateTime Until { get; set; }
        public short Count { get; set; }
        public string Rule { get; set; }
    }
}
