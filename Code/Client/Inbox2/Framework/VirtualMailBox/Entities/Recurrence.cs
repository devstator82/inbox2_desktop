using Inbox2.Framework.Persistance;
using Inbox2.Framework.Plugins.Recurrence;
using Inbox2.Platform.Channels.Attributes;
using Inbox2.Platform.Channels.Entities;
using System;
using System.Collections.Generic;

namespace Inbox2.Framework.Plugins.Entities
{
    [PersistableClass]
    public class Recurrence : ChannelRecurrence
    {
        [IndexAndStore]
        [PrimaryKey] public long? InternalRecurrenceId { get; set; }
        public List<IRecurrenceValue> Rules { get; set; }
        public List<Event> Exceptions { get; set; }

        public List<Event> GetEvents()
        {
            return null;
        }

        public List<Event> GetEvents(DateTime from, DateTime to)
        {
            return null;
        }
    }
}
