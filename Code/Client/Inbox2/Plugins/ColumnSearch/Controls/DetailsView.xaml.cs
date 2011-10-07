using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using Inbox2.Framework;
using Inbox2.Framework.Localization;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Framework;
using Inbox2.Plugins.ColumnSearch.Helpers;

namespace Inbox2.Plugins.ColumnSearch.Controls
{
	/// <summary>
	/// Interaction logic for DetailsView.xaml
	/// </summary>
	public partial class DetailsView : UserControl, INotifyPropertyChanged, IPersistableTab
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler<EventArgs> RequestCloseTab;

		private string searchQuery;

		private readonly SearchDockControl dock;
		private readonly VirtualMailBox mailbox;
		private readonly ThreadSafeCollection<Message> messages;
		private readonly ThreadSafeCollection<Document> documents;
		private readonly ThreadSafeCollection<Person> persons;
		private readonly ThreadSafeCollection<UserStatus> statusUpdates;

		private readonly CollectionViewSource messagesViewSource;
		private readonly CollectionViewSource documentsViewSource;
		private readonly CollectionViewSource personsViewSource;
		private readonly CollectionViewSource statusUpdatesViewSource;

		public string Title
		{
			get { return Strings.SearchFor + " '" + searchQuery + "'"; }
		}

		public Control CustomHeaderContent
		{
			get { return dock; }
		}

		public PluginPackage Plugin
		{
			get { return PluginsManager.Current.GetPlugin<ColumnSearchPlugin>(); }
		}

		public ViewType ViewType
		{
			get { return ViewType.DetailsView; }
		}

		public DetailsView()
		{
			using (new CodeTimer("ColumnSearch/Constructor"))
			{
				dock = new SearchDockControl();
				mailbox = VirtualMailBox.Current;
				messages = new ThreadSafeCollection<Message>();
				documents = new ThreadSafeCollection<Document>();
				persons = new ThreadSafeCollection<Person>();
				statusUpdates = new ThreadSafeCollection<UserStatus>();

				messagesViewSource = new CollectionViewSource { Source = messages };
				messagesViewSource.SortDescriptions.Add(new SortDescription("SortDate", ListSortDirection.Descending));

				documentsViewSource = new CollectionViewSource { Source = documents };
				documentsViewSource.SortDescriptions.Add(new SortDescription("Filename", ListSortDirection.Ascending));

				personsViewSource = new CollectionViewSource { Source = persons };
				personsViewSource.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

				statusUpdatesViewSource = new CollectionViewSource { Source = statusUpdates };
				statusUpdatesViewSource.SortDescriptions.Add(new SortDescription("SortDate", ListSortDirection.Descending));

				InitializeComponent();

				MessagesColumn.OverrideViewSource(messagesViewSource);
				DocumentsColumn.OverrideViewSource(documentsViewSource);
				StatusUpdatesColumn.OverrideViewSource(statusUpdatesViewSource);

				dock.UpdateSearch += delegate
					{
						LoadData(new ColumnSearchDataHelper { SearchQuery = dock.SearchQuery });
					};

				DataContext = this;
			}
		}

		public void LoadData(object data)
		{
			searchQuery = ((ColumnSearchDataHelper) data).SearchQuery;

			if (String.IsNullOrEmpty(searchQuery))
			{
				messages.Clear();
				documents.Clear();
				persons.Clear();
				statusUpdates.Clear();

				return;
			}

			dock.SearchQuery = searchQuery;

			OnPropertyChanged("Title");

			var searchMessages = ClientState.Current.Search.PerformSearch<Message>(searchQuery)
				.Select(id => mailbox.Messages.FirstOrDefault(m => m.MessageId == id))
				.Where(doc => doc != null)
				.ToList();

			var searchDocuments = ClientState.Current.Search.PerformSearch<Document>(searchQuery)
				.Select(id => mailbox.Documents.FirstOrDefault(d => d.DocumentId == id))
				.Where(doc => doc != null)
				.ToList();

			var searchPersons = ClientState.Current.Search.PerformSearch<Person>(searchQuery)
				.Select(id => mailbox.Persons.FirstOrDefault(p => p.PersonId == id))
				.Where(p => p != null)
				.ToList();

			var searchStatusUpdates = ClientState.Current.Search.PerformSearch<UserStatus>(searchQuery)
				.Select(id => mailbox.StatusUpdates.FirstOrDefault(s => s.StatusId == id))
				.Where(s => s != null)
				.ToList();

			messages.Replace(searchMessages);
			documents.Replace(searchDocuments);
			persons.Replace(searchPersons);
			statusUpdates.Replace(searchStatusUpdates);
		}

		public object SaveData()
		{
			return new ColumnSearchDataHelper { SearchQuery = searchQuery };
		}
	
		void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
