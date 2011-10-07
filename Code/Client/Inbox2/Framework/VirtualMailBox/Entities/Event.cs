using Inbox2.Framework.Persistance;
using Inbox2.Platform.Channels.Attributes;
using Inbox2.Platform.Channels.Entities;
using System.Collections.Generic;

namespace Inbox2.Framework.Plugins.Entities
{
    [PersistableClass]
    public class Event : ChannelEvent
    {
        [IndexAndStore]
        [PrimaryKey] public long? InternalEventId { get; set; }
		[Persist] public List<string> AttachedItems { get; set; }
        [Persist] public int EventFolder { get; set; }
        
		public bool IsEditing { get; set; }
        public Recurrence Recurrence { get; set; }

        public List<Event> GetRecurrences()
        {
            return null;
        }
    }
}
