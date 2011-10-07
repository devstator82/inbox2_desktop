using Inbox2.Platform.Channels.Attributes;
using System;

namespace Inbox2.Platform.Channels.Entities
{
    public class ChannelEvent
    {
		public string ChannelEventKey { get; set; }

        public string Subject { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        
		public EventClassType Class { get; set; }
        public EventPriority Priority { get; set; }
        public EventState State { get; set; }
		
		public long SourceChannelId { get; set; }

        public DateTime Stamp { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsWholeDay
        {
            get 
            { 
                // Check if it's a whole day
                if (StartDate.AddDays(1).Equals(EndDate)) return true;
                return false;
            }
        }

		public DateTime DateCreated { get; set; }
        public DateTime? Modified { get; set; }
    }
}
