using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Stats;

namespace Inbox2.Plugins.Contacts.Controls
{
	/// <summary>
	/// Interaction logic for SearchDockControl.xaml
	/// </summary>
	public partial class SearchDockControl : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public bool IsInSearchMode
		{
			get { return SearchTextBox.Text.Length > 0; }
		}

		public ContactsPlugin Plugin
		{
			get { return PluginsManager.Current.GetPlugin<ContactsPlugin>(); }
		}

		public ContactsState State
		{
			get { return (ContactsState)Plugin.State; }
		}

		public SearchDockControl()
		{
			InitializeComponent();

			DataContext = this;
		}

		void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			State.Filter.SearchKeyword = SearchTextBox.Text;

			if (String.IsNullOrEmpty(SearchTextBox.Text))
				if (State.Filter.ShowSoftContacts)
					State.Filter.ShowSoftContacts = false;

			OnPropertyChanged("IsInSearchMode");
		}

        void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                SearchTextBox.Text = String.Empty;

                EventBroker.Publish(AppEvents.RequestFocus);
            }
        }

		void EndSearchButton_Click(object sender, RoutedEventArgs e)
		{
			SearchTextBox.Text = String.Empty;

			OnPropertyChanged("IsInSearchMode");
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}

