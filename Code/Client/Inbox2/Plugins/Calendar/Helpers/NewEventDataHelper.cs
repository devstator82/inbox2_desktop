using System;
using System.Runtime.Serialization;

namespace Inbox2.Plugins.Calendar.Helpers
{
    [DataContract]
    public class NewEventDataHelper
    {
        // TODO : Are all members accounted for?
        [DataMember]
        public long? SourceMessageId { get; set; }

        [DataMember]
        public TimeSpan StartTime { get; set; }

        [DataMember]
        public TimeSpan EndTime { get; set; }

        [DataMember]
        public bool LastsWholeDay { get; set; }

        [DataMember]
        public string Subject { get; set; }

        [DataMember]
        public int SelectedChannelIndex { get; set; }
    }
}
