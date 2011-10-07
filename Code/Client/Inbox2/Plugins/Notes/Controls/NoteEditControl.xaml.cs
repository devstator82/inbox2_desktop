using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Plugins.Entities;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Plugins.Notes.Controls
{
    /// <summary>
    /// Interaction logic for NoteEditControl.xaml
    /// </summary>
    public partial class NoteEditControl : UserControl
    {
        #region Fields

        public event EventHandler<EventArgs> NoteSaved;

        public Note SourceNote { get; set; }

        #endregion

		#region Properties
		
		public NotesState State
		{
			get { return (NotesState)PluginsManager.Current.GetPlugin<NotesPlugin>().State; }
		}

		#endregion

		#region Construction

		public NoteEditControl()
        {
            InitializeComponent();

            DataContext = this;
        }

        #endregion

        #region Methods

        void SaveNote(int messageFolder)
        {
            Note note = new Note();			

            if (SourceNote != null)
            {
				var notesView = (IEditableCollectionView)State.NotesViewSource.View;

				notesView.EditItem(note);

                note = SourceNote;
            	note.Context = GetContextFrom(ContentTextBox.Text);
                note.Content = ContentTextBox.Text;
                note.ContentType = IsMostlyUrlContent(note) ? NoteTypes.Url : NoteTypes.Note;
                note.DateModified = DateTime.Now;

                // Save the note
                ClientState.Current.DataService.Update(note);

				note.UpdateProperty("Content");
				note.UpdateProperty("ContentType");

				notesView.CommitEdit();
            }
            else
            {
                int version = 0;

                note.Version = version;
                note.Context = ContentTextBox.Text;
                note.Content = ContentTextBox.Text.StripHtml();
                note.ContentType = IsMostlyUrlContent(note) ? NoteTypes.Url : NoteTypes.Note;
                note.DownloadState = DownloadStates.Downloaded;
                note.SendState = SendStates.DoNotSend;
                note.NoteFolder = Folders.Inbox;
                note.NoteState = EntityStates.Unread;
                note.SourceChannelId = ChannelsManager.GetDefaultChannel().Configuration.ChannelId;
                note.DateCreated = DateTime.Now;

                // Save the note
                ClientState.Current.DataService.Save(note);

				Thread.CurrentThread.ExecuteOnUIThread(() => State.Notes.Add(note));
            }
        }

		string GetContextFrom(string contents)
		{
			if (contents.Length <= 30)
				return contents;

			return contents.Substring(0, 30) + "...";
		}

        bool IsMostlyUrlContent(Note note)
        {
            Regex regex = new Regex(@"(?i:http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            string source = note.Content;
            MatchCollection mc = regex.Matches(source);

            if (mc.Count == 0)
                return false;

            // Get total length of all entries
            int total = 0;
            foreach (Match match in mc)
                total += match.Length;

            // See if our url(s) occupies at least 30% of the the text
            int perc = 100 * total / source.Length;

            return perc > 30;
        }

        #endregion

        #region Event handlers

        void Send_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ContentTextBox.Text.Length > 0;
        }

        void Send_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveNote(Folders.SentItems);

            if (NoteSaved != null)
                NoteSaved(this, EventArgs.Empty);
        }

        private void NoteEditControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (SourceNote != null)
                ContentTextBox.Text = SourceNote.Content;
        }

        #endregion

    }
}
