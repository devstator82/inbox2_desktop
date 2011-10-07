using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Inbox2.Framework;
using Inbox2.Framework.Collections;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Plugins.Entities;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Collections;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Plugins.Notes.Helpers;
using Inbox2.Plugins.Notes.Windows;

namespace Inbox2.Plugins.Notes
{
	public class NotesState : PluginStateBase
	{
        #region Fields

        public event EventHandler<EventArgs> SelectedNoteChanged;

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

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public AdvancedObservableCollection<Note> Notes { get; private set; }

        /// <summary>
        /// Gets or sets the notes view source.
        /// </summary>
        /// <value>The notes view source.</value>
        public CollectionViewSource NotesViewSource { get; private set; }

        /// <summary>
        /// Gets or sets the selected notes.
        /// </summary>
        /// <value>The selected notes.</value>
        public AdvancedObservableCollection<Note> SelectedNotes { get; private set; }

        /// <summary>
        /// Gets the selected note.
        /// </summary>
        /// <value>The selected note.</value>
        public Note SelectedNote
        {
            get { return SelectedNotes.FirstOrDefault(); }
        }

        #endregion

        #region Properties

        public override bool CanView
        {
            get { return SelectedNote != null; }
        }

        public override bool CanReply
        {
            get { return true; }
        }

        public override bool CanForward
        {
            get { return true; }
        }

        public override bool CanDelete
        {
            get { return true; }
        }

        public override bool CanMarkRead
        {
            get { return !SelectedNote.IsSet(EntityStates.Read); }
        }

        public override bool CanMarkUnread
        {
            get { return !SelectedNote.IsSet(EntityStates.Unread); }
        }

        #endregion

        #region Constructors

		public NotesState()
		{
			SelectedNotes = new AdvancedObservableCollection<Note>();
			Notes = new AdvancedObservableCollection<Note>();

			NotesViewSource = new CollectionViewSource { Source = Notes };
			NotesViewSource.View.Filter = NotesViewSourceFilter;

            new CollectionObserverDelegate<ChannelInstance>(ChannelsManager.Channels, delegate(ChannelInstance channel)
            {
                channel.IsVisibleChanged += delegate { NotesViewSource.View.Refresh(); };
            });

            Sort = new SortHelper(NotesViewSource);
            Sort.LoadSettings();

            Filter = new FilterHelper(NotesViewSource);
            Filter.LoadSettings();

			SelectedNotes.CollectionChanged += delegate
			{
				OnPropertyChanged("SelectedNote");

				OnSelectionChanged();
			};
		}

		#endregion

        #region Methods

        bool NotesViewSourceFilter(object sender)
        {
            Note note = (Note)sender;

            return note.ProcessingHints != ProcessingHints.Ignore
                   && note.NoteFolder != Folders.Trash;
        }

		protected override void NewCore()
		{
			// Open new window
			NewItemWindow window = new NewItemWindow();
			window.Show();
		}

        protected override void ViewCore()
        {
            // Open window
            NewItemWindow window = new NewItemWindow(SelectedNote);
            window.Show();
        }

        public override void Shutdown()
        {
            Sort.SaveSettings();
            Filter.SaveSettings();
        }

        #endregion
    }
}
