using System;
using System.Collections.Specialized;
using System.Windows.Data;
using Inbox2.Framework;
using Inbox2.Framework.Plugins.Entities;
using Inbox2.Platform.Framework.Collections;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Plugins.Calendar.Helpers
{
    public class Day
    {
        public CollectionViewSource AfternoonViewSource { get; private set; }
        public DateTime Date { get; private set; }
        public int DateDay { get { return Date.Day; } }
        public CollectionViewSource EveningViewSource { get; private set; }
        public AdvancedObservableCollection<Event> Events { get; private set; }
        public bool IsInCurrentMonth
        {
            get
            {
                // Check if the month and year of this day is the selected month of the navigator
                if (Date.Year == State.CurrentDate.Year && Date.Month == State.CurrentDate.Month) return true;
                return false;
            }
        }
        public bool IsToday
        {
            get
            {
                // Check if it is the 
                DateTime date = new DateTime(Date.Year, Date.Month, Date.Day);
                DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                if (date.Equals(now)) return true;
                return false;
            }
        }
        public CollectionViewSource MiddayViewSource { get; private set; }
        public CalendarPlugin Plugin
        {
            get { return PluginsManager.Current.GetPlugin<CalendarPlugin>(); }
        }
        public CalendarState State
        {
            get { return (CalendarState)Plugin.State; }
        }
        public CollectionViewSource WholeDayViewSource { get; private set; }

        public Day(DateTime date)
        {
            // Set the date
            Date = date;

            // Fill the events collection and add an event listener
            Events = new AdvancedObservableCollection<Event>();
            SetEvents();
            State.Events.CollectionChanged += Events_CollectionChanged;
            State.EventsViewSource.View.CollectionChanged += Events_CollectionChanged;

            // Viewsource for the whole day events
            AfternoonViewSource = new CollectionViewSource { Source = Events };
            AfternoonViewSource.View.Filter = AfternoonViewSourceFilter;

            // Viewsource for the whole day events
            EveningViewSource = new CollectionViewSource { Source = Events };
            EveningViewSource.View.Filter = EveningViewSourceFilter;

            // Viewsource for the whole day events
            MiddayViewSource = new CollectionViewSource { Source = Events };
            MiddayViewSource.View.Filter = MiddayViewSourceFilter;

            // Viewsource for the whole day events
            WholeDayViewSource = new CollectionViewSource { Source = Events };
            WholeDayViewSource.View.Filter = WholeDayViewSourceFilter;
        }

        private bool AfternoonViewSourceFilter(object sender)
        {
            // Get the event
            Event calendarevent = (Event)sender;

            // Check if it's a whole day, if so continue
            if (calendarevent.IsWholeDay) return false;

            // Check the time, if it's right, add it to the collection
            if (calendarevent.StartDate.TimeOfDay >= new TimeSpan(0, 0, 0) &&
                calendarevent.StartDate.TimeOfDay < new TimeSpan(12, 0, 0)) return true;
            return false;
        }

        private bool EveningViewSourceFilter(object sender)
        {
            // Get the event
            Event calendarevent = (Event)sender;

            // Check if it's a whole day, if so continue
            if (calendarevent.IsWholeDay) return false;

            // Check the time, if it's right, add it to the collection
            if (calendarevent.StartDate.TimeOfDay >= new TimeSpan(18, 0, 0) &&
                calendarevent.StartDate.TimeOfDay < new TimeSpan(24, 0, 0)) return true;
            return false;
        }

        private void Events_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Refresh the list
            SetEvents();
        }

        private bool MiddayViewSourceFilter(object sender)
        {
            // Get the event
            Event calendarevent = (Event)sender;

            // Check if it's a whole day, if so continue
            if (calendarevent.IsWholeDay) return false;

            // Check the time, if it's right, add it to the collection
            if (calendarevent.StartDate.TimeOfDay >= new TimeSpan(12, 0, 0) &&
                calendarevent.StartDate.TimeOfDay < new TimeSpan(18, 0, 0)) return true;
            return false;
        }

        public void SetEvents()
        {
            // Clear the list
            Events.Clear();

            // Check all events
            for (int i = 0; i < State.Events.Count; i++)
            {
                // Get event
                Event calendarevent = State.Events[i];

                // Check the date of the event
                if (!(calendarevent.StartDate.Year == Date.Year &&
                    calendarevent.StartDate.Month == Date.Month &&
                    calendarevent.StartDate.Day == Date.Day)) continue;

                // Check the general filter options of events
                if (!State.CalendarViewSourceFilter(calendarevent)) continue;

                // If the event is processed to be added, add it to the collection
                Events.Add(calendarevent);
            }
        }

        private bool WholeDayViewSourceFilter(object sender)
        {
            // Get the event
            Event calendarevent = (Event)sender;

            // Check if it's the same date
            if (calendarevent.IsWholeDay) return true;
            return false;
        }
    }
}
