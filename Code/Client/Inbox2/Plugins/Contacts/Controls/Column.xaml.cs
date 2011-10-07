using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Stats;
using Inbox2.Framework.UI;
using Inbox2.Framework.VirtualMailBox.Entities;

namespace Inbox2.Plugins.Contacts.Controls
{
    /// <summary>
    /// Interaction logic for Column.xaml
    /// </summary>
	public partial class Column : UserControl
	{
		#region Fields

		private readonly Flipper selectionFlipper;

		#endregion

		#region Properties

		public ContactsPlugin Plugin
        {
            get { return PluginsManager.Current.GetPlugin<ContactsPlugin>(); }
        }

        public ContactsState State
        {
            get { return (ContactsState)Plugin.State; }
        }

        public CollectionViewSource ContactsViewSource
        {
            get { return State.PersonsViewSource; }
        }	

        #endregion

        #region Constructors

        public Column()
        {
            InitializeComponent();

            DataContext = this;

			selectionFlipper = new Flipper(TimeSpan.FromMilliseconds(400),
					() => State.SelectedPersons.ReplaceWithCast(ContactsListView.SelectedItems));
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

        #region Methods

		public void OverrideViewSource(CollectionViewSource newSource)
		{
			ContactsListView.ItemsSource = newSource.View;
		}

        #endregion

        #region Event Handlers

        void ContactsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

        	selectionFlipper.Delay();
        }

        void ContactsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var person = (e.OriginalSource as DependencyObject)
                .FindListViewItem<Person>(ContactsListView.ItemContainerGenerator);

            if (person != null)
            {
				State.SelectedPersons.Replace(new[] { person });

                State.View();

				ClientStats.LogEvent("View contact (doubleclick)");
            }
        }

        void ContactsListView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
				ClientStats.LogEvent("Delete contact (keyboard)");

                State.Delete();
            } 
			else if (e.Key == Key.Enter)
			{
				ClientStats.LogEvent("View contact (keyboard)");

				State.View();
			}
        }

        #endregion        
    }
}
