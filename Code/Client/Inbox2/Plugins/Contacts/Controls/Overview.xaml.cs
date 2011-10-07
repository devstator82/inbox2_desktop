using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Framework.Localization;
using Inbox2.Framework.UI;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Framework;

namespace Inbox2.Plugins.Contacts.Controls
{
	/// <summary>
	/// Interaction logic for Overview.xaml
	/// </summary>
	public partial class Overview : UserControl, IControllableTab
	{
		public event EventHandler<EventArgs> RequestCloseTab;

		private readonly SearchDockControl dock = new SearchDockControl();
		private readonly VirtualMailBox mailbox = VirtualMailBox.Current;

		#region Properties

		public string Title
		{
			get { return Strings.Contacts; }
		}

		public Control CustomHeaderContent
		{
			get { return dock; }
		}

		public PluginPackage Plugin
		{
			get { return PluginsManager.Current.GetPlugin<ContactsPlugin>(); }
		}

		public ContactsState State
		{
			get { return (ContactsState)Plugin.State; }
		}

		#endregion

		public Overview()
		{
			using (new CodeTimer("ContactsOverview/Constructor"))
			{
				InitializeComponent();

				DataContext = this;

				State.SelectionChanged += delegate { RefreshView(); };
			}
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			MessagesColumn.OverrideViewSource(null);
			DocumentsColumn.OverrideViewSource(null);
		}

		void RefreshView()
		{
			if (State.SelectedPerson == null)
				return;

			var messagesViewSource = new CollectionViewSource { Source = State.SelectedPerson.Messages };
			messagesViewSource.SortDescriptions.Add(new SortDescription("SortDate", ListSortDirection.Descending));

			var documentsViewSource = new CollectionViewSource { Source = State.SelectedPerson.Documents };
			documentsViewSource.SortDescriptions.Add(new SortDescription("Filename", ListSortDirection.Ascending));

			var statusUpdatesViewSource = new CollectionViewSource { Source = GetStatusUpdates() };
			statusUpdatesViewSource.SortDescriptions.Add(new SortDescription("SortDate", ListSortDirection.Descending));

			if (State.IsExternalSelection)
				dock.SearchTextBox.Text = State.SelectedPerson.Name;

			if (State.SelectedPerson.IsSoft)
				State.Filter.ShowSoftContacts = true;
			else if (State.Filter.ShowSoftContacts)
				State.Filter.ShowSoftContacts = false;

			UserProfileControl.Visibility = Visibility.Visible;
			SearchProfilesControl.Visibility = Visibility.Visible;

			MessagesColumn.OverrideViewSource(messagesViewSource);
			DocumentsColumn.OverrideViewSource(documentsViewSource);
			StatusUpdatesColumn.OverrideViewSource(statusUpdatesViewSource);

			UserProfileControl.Person = State.SelectedPerson;
			SearchProfilesControl.Person = State.SelectedPerson;
		}

		List<UserStatus> GetStatusUpdates()
		{
			var configurations = ChannelsManager.GetStatusChannels().Select(c => c.Configuration).ToList();

			// Twitter stores the Address in the screenname field (waseemsadiq) because it
			// also has a different addressing structure for sending DM's (123456).
			var screenames = State.SelectedPerson.Profiles
				.Where(p => configurations.Contains(p.SourceChannel))				
				.Select(p => p.ScreenName)
				.ToList();

			var addresses = State.SelectedPerson.Profiles
				.Where(p => configurations.Contains(p.SourceChannel))
				.Select(p => p.Address)
				.ToList();
			
			using (mailbox.StatusUpdates.ReaderLock)
				return mailbox.StatusUpdates.Where(
					s => s.From != null && (screenames.Contains(s.From.Address) || addresses.Contains(s.From.Address)))
					.ToList();
		}       

		void SearchProfilesControl_OnPersonRedirected(object sender, EventArgs e)
		{
			// Forces a refresh the search results
			State.Filter.SearchKeyword = State.Filter.SearchKeyword;
		}

		void SearchProfilesControl_OnProfileMerged(object sender, EventArgs e)
		{
			RefreshView();
		}        
	}
}
