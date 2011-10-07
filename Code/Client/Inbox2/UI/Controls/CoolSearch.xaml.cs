using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Stats;
using Inbox2.Framework.Threading;
using Inbox2.Framework.UI;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.UI.Helpers;

namespace Inbox2.UI.Controls
{
	/// <summary>
	/// Interaction logic for CoolSearch.xaml
	/// </summary>
	public partial class CoolSearch : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		
		private readonly Flipper searchFlipper;
		private readonly VirtualMailBox mailbox;

		public bool IsInSearchMode
		{
			get { return SearchTextBox.Text.Length > 0; }
		}

		public bool IsSearching { get; private set; }
		
		public CoolSearch()
		{
			searchFlipper = new Flipper(TimeSpan.FromMilliseconds(500), InlineSearch);
			mailbox = VirtualMailBox.Current;

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

		void HideList()
		{
			AutoCompletionListBox.ItemsSource = null;
			AutoCompletionPopup.IsOpen = false;
		}

		void InlineSearch()
		{
			var query = SearchTextBox.Text.Trim();

			if (String.IsNullOrEmpty(query))
				return;

			if (query.Length < 3)
				return;

			// Delay new search if another search is still pending
			if (IsSearching)
				searchFlipper.Delay();

			IsSearching = true;
			OnPropertyChanged("IsSearching");
			((Storyboard)FindResource("RunLoaderStoryboard")).Begin(this);

			new BackgroundActionTask(() => InlineSearchAsync(query)).ExecuteAsync();
		}

		void InlineSearchAsync(string query)
		{
			var results = new List<CoolSearchResult>();

            if (query.IndexOf(": ") > -1)
            {
                var parts = query.Split(new[] { ": " }, 2, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 1)
                    return;
                else
                {
                    var constraint = parts[0];
                    query = parts[1];

                    switch (constraint.ToLower())
                    {
                        case "friend":
                        case "friends":
                        case "from":
                        case "person":
                            {
                                FillPersons(results, query);
                                break;
                            }
                        case "label":
                        case "labels":
                        case "tag":
                        case "tags":
                            {
                                FillLabels(results, query);
                                break;
                            }
                        case "message":
                        case "messages":
                        case "msg":
                        case "conversation":
                        case "conversations":
                        case "conv":
                            {
                                FillConversations(results, query);
                                break;
                            }
                        case "document":
                        case "documents":
                        case "attachment":
                        case "attachments":
                            {
                                FillDocuments(results, query);
                                break;
                            }
                    }
                }
            }
            else
            {
                FillAll(results, query);
            }            

		    Thread.CurrentThread.ExecuteOnUIThread(delegate
		       	{
					NoResultsTextBlock.Visibility = results.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
					AutoCompletionListBox.ItemsSource = results;
					AutoCompletionPopup.IsOpen = true;

					IsSearching = false;
					OnPropertyChanged("IsSearching");

					((Storyboard)FindResource("RunLoaderStoryboard")).Stop(this);
		       	});			
		}

        void FillAll(List<CoolSearchResult> results, string query)
        {
            // 1. Find persons by name
            FillPersons(results, query);

            // 2. Find label by name
            FillLabels(results, query);

            // 3. Find conversations by subject
            FillConversations(results, query);

            // 4. Find documents by filename
            FillDocuments(results, query);
        }

	    void FillPersons(List<CoolSearchResult> results, string query)
	    {
			using (mailbox.Persons.ReaderLock)
				results.AddRange(mailbox.Persons
					.Where(p => p.Name.IndexOf(query, StringComparison.InvariantCultureIgnoreCase) > -1 && p.Profiles.Count > 0)
					.Take(6)
					.Select(p => new CoolSearchResult(p)));
	    }

        void FillLabels(List<CoolSearchResult> results, string query)
        {
			using (mailbox.Labels.ReaderLock)
				results.AddRange(mailbox.Labels
					.Where(l => l.Key.IndexOf(query, StringComparison.InvariantCultureIgnoreCase) > -1)
					.Where(l => l.Value.Sum(v => v.Messages.Count) > 0)
					.Take(6)
					.Select(l => new CoolSearchResult(l.Value.First())));
        }

        void FillConversations(List<CoolSearchResult> results, string query)
        {
			using (mailbox.Conversations.ReaderLock)
				results.AddRange(mailbox.Conversations
					.Where(c => c.Context.IndexOf(query, StringComparison.InvariantCultureIgnoreCase) > -1)
					.Take(6)
					.Select(c => new CoolSearchResult(c)));
        }

        void FillDocuments(List<CoolSearchResult> results, string query)
        {
			using (mailbox.Documents.ReaderLock)
				results.AddRange(mailbox.Documents
					.Where(d => d.Filename.IndexOf(query, StringComparison.InvariantCultureIgnoreCase) > -1)
					.Take(6)
					.Select(d => new CoolSearchResult(d)));
        }

	    void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
		{
			OnPropertyChanged("IsInSearchMode");

			if (String.IsNullOrEmpty(SearchTextBox.Text.Trim()))
			{
				HideList();

				ClientStats.LogEvent("End search (dock/keyboard)");

				if (e.Key == Key.Escape)
					EventBroker.Publish(AppEvents.RequestFocus);

				return;
			}

			switch (e.Key)
			{
				case Key.Up:
					// Move selection up
					if (AutoCompletionListBox.SelectedIndex > 0)
						AutoCompletionListBox.SelectedIndex--;

					e.Handled = true;

					break;

				case Key.Down:
					// Move selection down
					if (AutoCompletionListBox.SelectedIndex < AutoCompletionListBox.Items.Count)
						AutoCompletionListBox.SelectedIndex++;

					e.Handled = true;

					break;

				case Key.Escape:

                    if (AutoCompletionPopup.IsOpen)
                        HideList();
                    else
                    {
						ClientStats.LogEvent("End search (dock/keyboard)");

                        SearchTextBox.Text = String.Empty;

                        EventBroker.Publish(AppEvents.RequestFocus);
                    }

			        e.Handled = true;

					break;

				case Key.Enter:

					// goto selected entity
					var item = AutoCompletionListBox.SelectedItem as CoolSearchResult;

					if (item != null)
					{
						item.NavigateTo();

						HideList();
					}
					else
					{
						ClientStats.LogEvent("New search (dock)");

						EventBroker.Publish(AppEvents.RequestNewSearch, SearchTextBox.Text);						
					}

					SearchTextBox.Text = String.Empty;

					e.Handled = true;

					break;

				default:
					searchFlipper.Delay();

					break;
			}
		}		

		void EndSearchButton_Click(object sender, RoutedEventArgs e)
		{
			ClientStats.LogEvent("End search (dock/mouse)");

			SearchTextBox.Text = String.Empty;

			OnPropertyChanged("IsInSearchMode");
		}		

		void AutoCompletionListBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				// goto selected entity
				var item = AutoCompletionListBox.SelectedItem as CoolSearchResult;

				if (item != null)
				{
					ClientStats.LogEvent("Select search result (dock/keyboard)");

					item.NavigateTo();

					SearchTextBox.Text = String.Empty;

					e.Handled = true;
				}
			}
		}

		void SearchTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			HideList();
		}

		void AutoCompletionListBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			var item = (e.OriginalSource as DependencyObject)
				.FindListViewItem<CoolSearchResult>(AutoCompletionListBox.ItemContainerGenerator);

			if (item != null)
			{
				ClientStats.LogEvent("Select search result (dock/mouse)");

				item.NavigateTo();

				SearchTextBox.Text = String.Empty;

				e.Handled = true;
			}
		}

		void AutoCompletionListBox_LostFocus(object sender, RoutedEventArgs e)
		{
			HideList();
		}
	}
}
