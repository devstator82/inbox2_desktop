using System;
using Inbox2.Framework.Plugins.Entities;

namespace Inbox2.Plugins.Calendar.Windows
{
    /// <summary>
    /// Interaction logic for NewItemWindow.xaml
    /// </summary>
    public partial class NewItemWindow
    {
        #region Constructors

        public NewItemWindow()
        {
            // Initialize and set title
            InitializeComponent();
            Title = "Quick add event";
        }

        public NewItemWindow(Event source) : this()
        {
            // Set title and set source
            Title = "Quick edit event";
            EventEditControl.SourceEvent = source;
        }

        #endregion

        private void EventEditControl_EventSaved(object sender, EventArgs e)
        {
            // Event is saved, close the window
            Close();
        }
    }
}
