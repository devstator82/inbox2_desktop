using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Inbox2.Framework;
using Inbox2.Framework.Collections;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.ValueTypes;
using Inbox2.Framework.Plugins.Entities;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Framework.Collections;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Platform.Logging;
using Inbox2.Plugins.Calendar.Helpers;
using Inbox2.Plugins.Calendar.Windows;

namespace Inbox2.Plugins.Calendar
{
    public class CalendarState : PluginStateBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the events.
        /// </summary>
        /// <value>The events.</value>
        public AdvancedObservableCollection<Event> Events { get; private set; }
        /// <summary>
        /// Gets or sets the events view source.
        /// </summary>
        /// <value>The events view source.</value>
        public CollectionViewSource EventsViewSource { get; private set; }
        /// <summary>
        /// Gets or sets the selected days.
        /// </summary>
        /// <value>The selected days.</value>
        public AdvancedObservableCollection<Day> SelectedDays { get; private set; }
        public CollectionViewSource SelectedDaysViewSource { get; private set; }
        /// <summary>
        /// Gets the selected day.
        /// </summary>
        /// <value>The selected day.</value>
        public Day SelectedDay
        {
            get { return SelectedDays.FirstOrDefault(); }
        }
        /// <summary>
        /// Gets or sets the selected events.
        /// </summary>
        /// <value>The selected events.</value>
        public AdvancedObservableCollection<Event> SelectedEvents { get; private set; }
        /// <summary>
        /// Gets the selected event.
        /// </summary>
        /// <value>The selected event.</value>
        public Event SelectedEvent
        {
            get { return SelectedEvents.FirstOrDefault(); }
        }
        /// <summary>
        /// Gets or sets the sort.
        /// </summary>
        /// <value>The sort.</value>
        public SortHelper Sort { get; private set; }
        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public FilterHelper Filter { get; private set; }
        public DayOfWeek FirstDayOfWeek { get { return DayOfWeek.Monday; } }
        public DateTime CurrentDate { get; set; }

        #endregion

        #region Can do Properties

        public override bool CanView
        {
            get { return SelectedDay != null; }
        }

        public override bool CanReply
        {
            get { return false; }
        }

        public override bool CanReplyAll
        {
            get { return false; }
        }

        public override bool CanForward
        {
            get { return false; }
        }

        public override bool CanDelete
        {
            get { return true; }
        }

        public override bool CanMarkRead
        {
            get { return false; }
        }

        public override bool CanMarkUnread
        {
            get { return false; }
        }

        public override bool CanNew
        {
            get { return true; }
        }

        #endregion

		#region Constructors

        public CalendarState()
		{
			// Set the viewsource of the events collection
			Events = new AdvancedObservableCollection<Event>();
			EventsViewSource = new CollectionViewSource { Source = Events };
            EventsViewSource.View.Filter = CalendarViewSourceFilter;

            // Add channel turn off/on execution code
			new CollectionObserverDelegate<ChannelInstance>(ChannelsManager.Channels, 
				delegate(ChannelInstance channel)
              	{
                    channel.IsVisibleChanged += delegate { EventsViewSource.View.Refresh(); };
              	});

            // Add sorters
            Sort = new SortHelper(EventsViewSource);
			Sort.LoadSettings();

            // Add filters
            Filter = new FilterHelper(EventsViewSource);
			Filter.LoadSettings();

            // Add selection for days
            SelectedDays = new AdvancedObservableCollection<Day>();
            SelectedDays.CollectionChanged += delegate
           	{
                OnPropertyChanged("SelectedDay");
				OnSelectionChanged();
			};
            SelectedDaysViewSource = new CollectionViewSource {Source = Events};
            SelectedDaysViewSource.View.Filter = SelectedDaysViewSourceFilter;

            // Add selection for events
            SelectedEvents = new AdvancedObservableCollection<Event>();
            SelectedEvents.CollectionChanged += delegate
            {
                OnPropertyChanged("SelectedEvent");
                OnSelectionChanged();
            };
		}

		#endregion

        #region Methods

        /// <summary>
        /// Filters the conversations view source.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <returns></returns>
        public bool CalendarViewSourceFilter(object sender)
        {
            // Cast to an event
            Event calendarevent = (Event)sender;

            // Check if the event is set to an object
            if (calendarevent == null) return false;

            // Break out if channel is not visible
            var channel = ChannelsManager.GetChannelObject(calendarevent.SourceChannelId);
            if (channel != null && !channel.IsVisible) return false;

            // Determine the folder type and check it with the saved filter option
            switch (calendarevent.EventFolder)
            {
                case Folders.Inbox: return Filter.Received;
                case Folders.Trash: return Filter.Deleted;
                default:
                    Logger.Debug("Event {0} has non recognized folder {1}, hiding event",
                                 LogSource.UI, calendarevent, calendarevent.EventFolder);
                    return true;
            }
        }

        bool SelectedDaysViewSourceFilter(object sender)
        {
            // Cast the sender to an event
            Event calendarevent = (Event)sender;

            // A boolean to determine whether
            bool hasaday = false;

            // Walk through all selected days
            foreach (Day day in SelectedDays)
            {
                // Check if the event occurs in the day
                if (day.Events.Contains(calendarevent)) hasaday = true;
            }

            // When the event is present in a selected day, check if it is shown in the EventViewSource
            if (hasaday) hasaday = CalendarViewSourceFilter(calendarevent);

            // Return the result
            return hasaday;
        }

        public override void Shutdown()
        {
            // Save the settings of sorting and filtering
            Sort.SaveSettings();
            Filter.SaveSettings();
        }

        protected override void NewCore()
        {
            // Open new window
            //TODO CalendarPlugin: Keep this or replace it by detailview?
            //TODO CalendarPlugin: Set the window on the plugin
            NewItemWindow window = new NewItemWindow();
            window.Show();
        }

        protected override void ViewCore()
        {
            //TODO CalendarPlugin: Check this one, not sure if it is correct
            ClientState.Current.ViewController.MoveTo(
                PluginsManager.Current.GetPlugin<CalendarPlugin>().DetailsView,
                new OverviewDataHelper());
        }

        protected override void DeleteCore()
        {
            // Contains the same references as in SelectedEvents,
            // these references can change when un-doing so keep a snapshot around
            List<Event> previousSelection = new List<Event>(SelectedEvents);

            // Contains instance copies of events, this will be the old data before the do is applied.
            List<Event> eventsCopy = SelectedEvents.Select(d => d.DuckCopy<Event>()).ToList();

            #region Do action

            Action doAction = delegate
            {
                // Get the view source of the events collection
                IEditableCollectionView eventsView = (IEditableCollectionView)EventsViewSource.View;

                foreach (Event calendarevent in SelectedEvents)
                {
                    // Edit the view source
                    eventsView.EditItem(calendarevent);

                    // Move the event to the trash
                    calendarevent.EventFolder = Folders.Trash;
                    ClientState.Current.DataService.Update(calendarevent, "EventFolder");

                    // Let the world know that an event is moved to the trash
                    EventBroker.Publish(AppEvents.UpdateEventState, calendarevent);

                    // Commit the changes to the view
                    eventsView.CommitEdit();
                }
            };

            #endregion

            #region Undo action

            Action undoAction = delegate
            {
                // Walk through all the events in the previous selection
                foreach (Event calendarevent in previousSelection)
                {
                    // Get the origional event from the copied list
                    var oldCalendarEvent = eventsCopy.Single(d => d.InternalEventId == calendarevent.InternalEventId);

                    // Set the folder to the origional folder
                    calendarevent.EventFolder = oldCalendarEvent.EventFolder;

                    // Update the event
                    ClientState.Current.DataService.Update(calendarevent, "EventFolder");

                    // Let the world know that an event is updated
                    EventBroker.Publish(AppEvents.UpdateEventState, calendarevent);
                }

                // We cannot use the IEditableObject appraoch here because the document in question
                // probably might not be in view anymore. So instead we will refresh the whole view.
                EventsViewSource.View.Refresh();
            };

            #endregion

            // Add the do and undo actions to the UndoManager
            ClientState.Current.UndoManager.Execute(new HistoryAction(doAction, undoAction));
        }

        #endregion
    }
}
