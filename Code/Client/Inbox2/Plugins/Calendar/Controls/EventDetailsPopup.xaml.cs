using System;
using Inbox2.Framework;

namespace Inbox2.Plugins.Calendar.Controls
{
    /// <summary>
    /// Interaction logic for EventDetailsPopup.xaml
    /// </summary>
    public partial class EventDetailsPopup
    {
        #region Properties

        public CalendarPlugin Plugin
        {
            get { return PluginsManager.Current.GetPlugin<CalendarPlugin>(); }
        }
        public CalendarState State
        {
            get { return (CalendarState)Plugin.State; }
        }

        #endregion

        #region Constructors

        public EventDetailsPopup()
        {
            // Initialize and set the DataContext
            InitializeComponent();
            DataContext = this;
        }

        #endregion

        #region Command handlers

        private void PopUp_CalendarEventDetails_Closed(object sender, EventArgs e)
        {
            // The calendar event is deselected so clear the selected events collection
            State.SelectedEvents.Clear();
        }

        #endregion
    }
}
