using System;
using System.Collections.Generic;
using Inbox2.Framework.Persistance;
using Inbox2.Platform.Channels.Attributes;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Framework.Plugins.Entities
{
    [PersistableClass]
    public class Calendar : ChannelCalendar
    {
        [IndexAndStore]
        [PrimaryKey] public long? InternalCalendarId { get; set; }
        public List<Event> Events { get; set; }

        public Event GetEvent(long identifier)
        {
            return null;
        }

        public List<Event> GetEvents(DateTime from, DateTime to)
        {
            return null;
        }

        public List<Event> GetEvents(DateTime from, DateTime to, Type type)
        {
            return null;
        }
    }
}
