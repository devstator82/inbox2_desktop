using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Localization;

namespace Inbox2.Plugins.Documents.Controls
{
	/// <summary>
	/// Interaction logic for SearchDockControl.xaml
	/// </summary>
	public partial class SearchDockControl : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public DetailsViewType DetailsViewType { get; private set; }

		public bool IsInSearchMode
		{
			get { return SearchTextBox.Text.Length > 0; }
		}

		public DocumentsPlugin Plugin
		{
			get { return PluginsManager.Current.GetPlugin<DocumentsPlugin>(); }
		}

		public DocumentsState State
		{
			get { return (DocumentsState)Plugin.State; }
		}

		public SearchDockControl(DetailsViewType detailsViewType)
		{
			DetailsViewType = detailsViewType;

			InitializeComponent();

			DataContext = this;
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			var fh = DetailsViewType == DetailsViewType.Images ? State.ImageFilter : State.DocumentFilter;

			fh.SearchKeyword = SearchTextBox.Text;

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

		void SearchDockControl_Initialized(object sender, EventArgs e)
		{
			if (DetailsViewType == DetailsViewType.Images)
			{
				SearchTextBox.ToolTip = Strings.SearchInYourImages;
				SearchTextBox.Tag = Strings.SearchImages;
			}
			else
			{
				SearchTextBox.ToolTip = Strings.SearchInYourDocuments;
				SearchTextBox.Tag = Strings.SearchDocuments;
			}
		}        
	}
}
