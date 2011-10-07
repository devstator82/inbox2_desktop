using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Inbox2.Framework.UI;

namespace Inbox2.Plugins.ColumnSearch.Controls
{
	/// <summary>
	/// Interaction logic for SearchDockControl.xaml
	/// </summary>
	public partial class SearchDockControl : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public event EventHandler<EventArgs> UpdateSearch;

		protected Flipper searchFlipper;

		public string SearchQuery
		{
			get { return SearchTextBox.Text; }
			set { SearchTextBox.Text = value; }
		}

		public bool IsInSearchMode
		{
			get { return SearchTextBox.Text.Length > 0; }
		}

		public SearchDockControl()
		{
			InitializeComponent();

			searchFlipper = new Flipper(TimeSpan.FromMilliseconds(500), ExecuteSearch);

			DataContext = this;
		}

		void ExecuteSearch()
		{
			string searchQuery = SearchTextBox.Text;

			if (!String.IsNullOrEmpty(searchQuery.Trim()))
			{
				if (UpdateSearch != null)
					UpdateSearch(this, EventArgs.Empty);
			}
		}

		void SearchTextBox_OnKeyDown(object sender, KeyEventArgs e)
		{
			OnPropertyChanged("IsInSearchMode");

			searchFlipper.Delay();
		}

		void EndSearchButton_Click(object sender, RoutedEventArgs e)
		{
			SearchTextBox.Text = String.Empty;

			if (UpdateSearch != null)
				UpdateSearch(this, EventArgs.Empty);

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
