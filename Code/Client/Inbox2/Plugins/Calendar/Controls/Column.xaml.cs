using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Plugins.Entities;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Plugins.Calendar.Helpers;
using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace Inbox2.Plugins.Calendar.Controls
{
	/// <summary>
	/// Interaction logic for Column.xaml
	/// </summary>
	public partial class Column : IScrollSlave
    {
        #region Fields

        public event EventHandler<EventArgs> SelectedEventChanged;

        #endregion

        #region Properties

        public DateTime CurrentDate
		{
			get { return State.CurrentDate; }
			set { State.CurrentDate = value; }
		}
		public CollectionViewSource CalendarViewSource
		{
			get { return State.EventsViewSource; }
		}
	    public CollectionViewSource SelectedDaysViewSource
	    {
            get { return State.SelectedDaysViewSource; }
	    }
        public CalendarPlugin Plugin
		{
			get { return PluginsManager.Current.GetPlugin<CalendarPlugin>(); }
		}
		public IScrollSource ScrollTarget
		{
			get { return EventsListView.GetScrollSource(); }
		}
        public CalendarState State
		{
			get { return (CalendarState)Plugin.State; }
        }

        #endregion

        #region Constructors

        public Column()
		{
            // Initialize and set the DataContext
			InitializeComponent();
			DataContext = this;

            // Add listeners
            //State.EventsViewSource.View.CollectionChanged += SelectedDays_CollectionChanged;
            State.SelectedDays.CollectionChanged += SelectedDays_CollectionChanged;
		    ViewSelectedDayHelper.ActiveButtonsChanged += ViewSelectedDayHelper_ActiveDaysChanged;
        }

        #endregion

        #region Methods

        public void AddDayToGrid(Day contentday, int row, int column)
		{
			// Create a control for the day
			ContentControl control = new ContentControl { Content = contentday };

			// Add the control to the grid
			CalendarViewGrid.Children.Add(control);

			// Set the position of the grid
			Grid.SetRow(control, row);
			Grid.SetColumn(control, column);
		}

		public void ClearGrid()
		{
			// Reset the grid
			CalendarViewGrid.Children.Clear();

            // Reset the selected days and selected events collections
            State.SelectedDays.Clear();
            State.SelectedEvents.Clear();
		}

		protected DateTime GetStartDateInMonth(DateTime date)
		{
			// Get the first day of the given month
			DateTime currentdayofmonth = new DateTime(date.Year, date.Month, 1);

			// Look for first day before the first of this month determined by the calendar
			while (currentdayofmonth.DayOfWeek != State.FirstDayOfWeek)
				currentdayofmonth = currentdayofmonth.AddDays(-1);

			// Return the start day
			return currentdayofmonth;
        }

        private void OnSelectedEventChanged()
        {
            // Raise the SelectedEventChanged event
            if (SelectedEventChanged != null)
            {
                SelectedEventChanged(this, EventArgs.Empty);
            }
        }

		public void RenderCalendar(DateTime date)
		{
			// Reset the grid
			ClearGrid();

			// Set the current month
			CurrentDate = date;

			// Refresh the month at the navigator
			RenderMonthNavigator();

			// Render the grid
			RenderGrid();
		}

		public void RenderGrid()
		{
			// Get the starting date of the month
			DateTime currentday = GetStartDateInMonth(CurrentDate);

			// Fill the grid with rows
			for (int row = 0; row < 6; row++)
			{
				// Fill the columns of the rows
				for (int column = 0; column < 7; column++)
				{
                    // Add the day to the grid
					AddDayToGrid(new Day(currentday), row, column);

					// Go to the next day
					currentday = currentday.AddDays(1);
				}
			}
		}

		public void RenderMonthNavigator()
		{
			// Set the label to the month of the current date
			LabelCurrentMonth.Content = CurrentDate.ToString("MMMM yyyy");
        }

        private void SelectEvents(IEnumerable events)
        {
            // Set the active plugin to this one
            ClientState.Current.ViewController.SelectedPlugin = Plugin;

            // Replace the selected events
            State.SelectedEvents.ReplaceWithCast(events);

            // Scroll to the selected events
            EventsListView.ScrollIntoView(State.SelectedEvents);

            // Raise the selected event changed event
            OnSelectedEventChanged();
        }

	    #endregion

        #region Command handlers

        private void EventsListView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                State.Delete();
            }
        }

        private void EventsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If there are no events selected, do nothing (e.g. when the selected events are deleted)
            if (e.AddedItems.Count == 0) return;

            // Execute select events code
            SelectEvents(EventsListView.SelectedItems);
        }

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Call the plugin's new method
            Plugin.State.New();
        }

	    private void Next_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			// Render the calendar with the next month
			RenderCalendar(CurrentDate.AddMonths(1));
        }

	    private void Previous_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			// Render the calendar with the previous month
			RenderCalendar(CurrentDate.AddMonths(-1));
        }

        private void SelectedDays_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Refresh the SelectedDaysViewSource
            SelectedDaysViewSource.View.Refresh();
        }

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			// Render the calender for the current date
			RenderCalendar(DateTime.Now);
        }

        private void ViewCalendarDay_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Get the day from the parameter
            Day day = (Day) e.Parameter;

            // If the day allready exists, remove it, else add it to the selected collection
            if (State.SelectedDays.Contains(day)) State.SelectedDays.Remove(day);
            else State.SelectedDays.Add(day);
        }

	    private void ViewCalenderEvent_Executed(object sender, ExecutedRoutedEventArgs e)
	    {
            // Get the event from the parameter
	        Event calendarevent = (Event) e.Parameter;

            // An event is selected in the calendar view, that can only be one at the time
            State.SelectedEvents.Clear();

            // Add the event to the selected collection
            State.SelectedEvents.Add(calendarevent);
	    }

        private void ViewCalendarEvent_MouseEnter(object sender, MouseEventArgs e)
        {
            // Set the placement target of the popup to the pressed button
            PopUp_CalendarEventDetails.PlacementTarget = (Button)sender;
        }

        private void ViewSelectedDayHelper_ActiveDaysChanged(object sender, EventArgs e)
        {
            
        }

		#endregion
    }
}
