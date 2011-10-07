using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Threading;
using Inbox2.Framework.UI;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Collections;

namespace Inbox2.Plugins.StatusUpdates.Controls
{
	/// <summary>
	/// Interaction logic for ChannelSearchStreamToolbarElement.xaml
	/// </summary>
	public partial class ChannelSearchStreamToolbarElement : UserControl, INotifyPropertyChanged, IInvokeProvider
	{
		private readonly ChannelInstance channel;
		private bool isSearching;

		public event PropertyChangedEventHandler PropertyChanged;

		public AdvancedObservableCollection<ChannelStatusUpdate> SearchResults { get; private set; }
		public CollectionViewSource SearchResultsViewSource { get; private set; }

		public bool HasResults
		{
			get { return SearchResults.Count > 0; }
		}
		
		public bool IsSearching
		{
			get { return isSearching; }
			private set
			{
				isSearching = value;

				OnPropertyChanged("IsSearching");
			}
		}

		public ChannelInstance Channel
		{
			get { return channel; }
		}

		public ChannelSearchStreamToolbarElement(ChannelInstance channel)
		{
			this.channel = channel;
			this.SearchResults = new AdvancedObservableCollection<ChannelStatusUpdate>();
			this.SearchResultsViewSource = new CollectionViewSource { Source = SearchResults };

			InitializeComponent();
			
			DataContext = this;
		}

		public void Invoke()
		{
			SearchPopup.IsOpen = true;
		}

		void Search_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			IsSearching = true;
			string query = PART_SearchTextbox.Text;

			((Storyboard)FindResource("RunLoaderStoryboard")).Begin(this);

			new BackgroundActionTask(delegate
				{
					var results = Channel.StatusUpdatesChannel.GetUpdates(query, 50).ToList();

					Thread.CurrentThread.ExecuteOnUIThread(delegate
						{
							SearchResults.Replace(results);

							OnPropertyChanged("HasResults");

							IsSearching = false;
							((Storyboard)FindResource("RunLoaderStoryboard")).Stop(this);
						});

				}).ExecuteAsync();			
		}

		void Search_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !String.IsNullOrEmpty(PART_SearchTextbox.Text.Trim());
		}

		void SearchPopup_Opened(object sender, EventArgs e)
		{
			FocusHelper.Focus(PART_SearchTextbox);
		}

		void PinButton_Click(object sender, RoutedEventArgs e)
		{
			SearchPopup.TryClose();
			SearchResults.Clear();

			OnPropertyChanged("HasResults");

			var keyword = PART_SearchTextbox.Text.Trim();

			// Save keyword
			VirtualMailBox.Current.StreamSearchKeywords.Add(Channel.Configuration.DisplayName, keyword);

			EventBroker.Publish(AppEvents.RequestSync);
			EventBroker.Publish(AppEvents.RebuildToolbar);			
		}

		void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}		

		void Reply_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var update = (ChannelStatusUpdate) e.Parameter;

			SearchPopup.TryClose();

			EventBroker.Publish(AppEvents.RequestStatusUpdate, String.Format("@{0} ", update.From.Address));			
		}

		void Forward_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var update = (ChannelStatusUpdate)e.Parameter;

			SearchPopup.TryClose();

			EventBroker.Publish(AppEvents.RequestStatusUpdate, String.Format("RT @{0} {1}", update.From.Address, update.Status));
		}

		void Reply_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		void Forward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}		
	}
}
