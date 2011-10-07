using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Plugins.Entities;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Plugins.Calendar.Controls
{
    /// <summary>
    /// Interaction logic for EventEditControl.xaml
    /// </summary>
    public partial class EventEditControl
    {
        #region Fields

        public event EventHandler<EventArgs> EventSaved;

        public Event SourceEvent { get; set; }

        #endregion

        #region Properties

        public CalendarState State
        {
            get { return (CalendarState)PluginsManager.Current.GetPlugin<CalendarPlugin>().State; }
        }

        #endregion

        #region Constructors

        public EventEditControl()
        {
            // Initialize and set DataContext to itself
            InitializeComponent();
            DataContext = this;
        }

        #endregion

        #region Methods

        void SaveEvent(int messageFolder)
        {
            //Event calendarevent = new Event();

            if (SourceEvent != null)
            {
                //var notesView = (IEditableCollectionView)State.EventsViewSource.View;

                //notesView.EditItem(calendarevent);

                //calendarevent = SourceEvent;
                //note.Context = GetContextFrom(ContentTextBox.Text);
                //note.Content = ContentTextBox.Text;
                //note.ContentType = IsMostlyUrlContent(note) ? NoteTypes.Url : NoteTypes.Note;
                //note.DateModified = DateTime.Now;

                // Save the note
                //ClientState.Current.DataService.Update(calendarevent);

                //note.UpdateProperty("Content");
                //note.UpdateProperty("ContentType");

                //notesView.CommitEdit();
            }
            else
            {
                //int version = 0;

                //note.Version = version;
                //note.Context = ContentTextBox.Text;
                //note.Content = ContentTextBox.Text.StripHtml();
                //note.ContentType = IsMostlyUrlContent(note) ? NoteTypes.Url : NoteTypes.Note;
                //note.DownloadState = DownloadStates.Downloaded;
                //note.SendState = SendStates.DoNotSend;
                //note.NoteFolder = Folders.Inbox;
                //note.NoteState = EntityStates.Unread;
                //note.SourceChannelId = ChannelsManager.GetDefaultChannel().Configuration.ChannelId;
                //note.DateCreated = DateTime.Now;

                // Save the note
                //ClientState.Current.DataService.Save(calendarevent);

                //Thread.CurrentThread.ExecuteOnUIThread(() => State.Events.Add(calendarevent));
            }
        }

        //string GetContextFrom(string contents)
        //{
        //    if (contents.Length <= 30)
        //        return contents;

        //    return contents.Substring(0, 30) + "...";
        //}

        //bool IsMostlyUrlContent(Note note)
        //{
        //    Regex regex = new Regex(@"(?i:http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        //    string source = note.Content;
        //    MatchCollection mc = regex.Matches(source);

        //    if (mc.Count == 0)
        //        return false;

        //    // Get total length of all entries
        //    int total = 0;
        //    foreach (Match match in mc)
        //        total += match.Length;

        //    // See if our url(s) occupies at least 30% of the the text
        //    int perc = 100 * total / source.Length;

        //    return perc > 30;
        //}

        #endregion

        #region Event handlers

        private void Send_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Check all input fields
            //e.CanExecute = ContentTextBox.Text.Length > 0;

            // Default
            e.CanExecute = false;
        }

        private void Send_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveEvent(Folders.Inbox);

            if (EventSaved != null)
                EventSaved(this, EventArgs.Empty);
        }

        private void EventEditControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Set all available details
            if (SourceEvent != null)
            {
                SubjectTextbox.Text = SourceEvent.Description;
            }
        }

        #endregion
    }
}
