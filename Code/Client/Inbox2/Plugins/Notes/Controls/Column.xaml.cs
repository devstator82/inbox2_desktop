using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Plugins.Entities;
using Inbox2.Plugins.Notes.Windows;

namespace Inbox2.Plugins.Notes.Controls
{
	/// <summary>
	/// Interaction logic for Column.xaml
	/// </summary>
    public partial class Column : UserControl, IScrollSlave
	{
        #region Fields

        public event EventHandler<EventArgs> SelectedNoteChanged;

        #endregion

        #region Properties

        public NotesPlugin Plugin
        {
            get { return PluginsManager.Current.GetPlugin<NotesPlugin>(); }
        }

        public NotesState State
        {
            get { return (NotesState)Plugin.State; }
        }

        public CollectionViewSource NotesViewSource
        {
            get { return State.NotesViewSource; }
        }

        public IScrollSource ScrollTarget
        {
            get { return NotesListView.GetScrollSource(); }
        }

        #endregion

        #region Constructors

        public Column()
		{
			InitializeComponent();

            DataContext = this;
        }

        #endregion

        #region Command handlers

        void New_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.New();
		}

		void View_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.View();
		}

		void Reply_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.Reply();
		}

		void ReplyAll_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.ReplyAll();
		}

		void Forward_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.Forward();
		}

		void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.Delete();
		}

		void MarkRead_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.MarkRead();
		}

		void MarkUnread_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.MarkUnread();
		}

		void View_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null
						   && Plugin.State.CanView;
		}

		void Reply_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null
						   && Plugin.State.CanReply;
		}

		void ReplyAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null
						   && Plugin.State.CanReplyAll;
		}

		void Forward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null
						   && Plugin.State.CanForward;
		}

		void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null
						   && Plugin.State.CanDelete;
		}

		void MarkRead_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null
						   && Plugin.State.CanMarkRead;
		}

		void MarkUnread_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null
						   && Plugin.State.CanMarkUnread;
		}

		#endregion

        #region Event handlers

        void SelectNotes(IEnumerable notes)
        {
            ClientState.Current.ViewController.SelectedPlugin = Plugin;
            State.SelectedNotes.ReplaceWithCast(notes);

            NotesListView.ScrollIntoView(State.SelectedNote);

            OnSelectedNoteChanged();
        }

        void SelectNote(Note note)
        {
            ClientState.Current.ViewController.SelectedPlugin = Plugin;
            State.SelectedNotes.Replace(new[] { note });

            NotesListView.ScrollIntoView(State.SelectedNote);

            OnSelectedNoteChanged();
        }

        void OnSelectedNoteChanged()
        {
            if (SelectedNoteChanged != null)
            {
                SelectedNoteChanged(this, EventArgs.Empty);
            }
        }

        void NotesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            SelectNotes(NotesListView.SelectedItems);
        }

        void NotesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var note = (e.OriginalSource as DependencyObject)
                .FindListViewItem<Note>(NotesListView.ItemContainerGenerator);

            if (note != null)
            {
                SelectNote(note);

                State.View();
            }
        }

        void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var note = (e.OriginalSource as DependencyObject)
                .FindListViewItem<Note>(NotesListView.ItemContainerGenerator);

            if (note != null)
            {
                SelectNote(note);

                string navigateUri = (e.OriginalSource as Hyperlink).NavigateUri.ToString();
                Process.Start(navigateUri);
            }
        }

        void NotesListView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                State.Delete();
            }
        }


        #endregion
    }
}
