using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Windows.Data;
using Inbox2.Framework;
using Inbox2.Framework.Collections;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Framework.Collections;
using Inbox2.Plugins.Contacts.Helpers;

namespace Inbox2.Plugins.Contacts
{
	public class ContactsState : PluginStateBase
	{
		#region Fields

		private VirtualMailBox mailbox;

		#endregion

		#region Properties

		internal bool IsExternalSelection { get; set; }

		public CollectionViewSource PersonsViewSource { get; private set; }

		public AdvancedObservableCollection<Person> SelectedPersons { get; private set; }

		public Person SelectedPerson
		{
			get { return SelectedPersons.FirstOrDefault(); }
		}

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

		#region Constructions

		public ContactsState()
		{
			mailbox = VirtualMailBox.Current;

			SelectedPersons = new AdvancedObservableCollection<Person>();

			PersonsViewSource = new CollectionViewSource { Source = VirtualMailBox.Current.Persons };
			PersonsViewSource.View.Filter = ContactsViewSourceFilter;

			Sort = new SortHelper(PersonsViewSource);
			Sort.LoadSettings();

			Filter = new FilterHelper(PersonsViewSource);

            SelectedPersons.CollectionChanged += delegate
         	{
         		OnPropertyChanged("SelectedPerson");

         		OnSelectionChanged();
         	};

			EventBroker.Subscribe(AppEvents.SyncContactsFinished, () => Thread.CurrentThread.ExecuteOnUIThread(() => PersonsViewSource.View.Refresh()));
		}

		#endregion

		#region Methods

		bool ContactsViewSourceFilter(object sender)
		{
			var person = (Person) sender;

            if (person.Profiles.Count == 0)
                return false;

			if (Filter.ShowSoftContacts == false && person.IsSoft)
				return false;

			if (!String.IsNullOrEmpty(Filter.SearchKeyword))
				return person.Name.IndexOf(Filter.SearchKeyword, StringComparison.InvariantCultureIgnoreCase) > -1;

			// Break out if source channel is not visible
			var channel = ChannelsManager.GetChannelObject(person.SourceChannelId);

			if (channel != null && !channel.IsVisible)
				return false;

			return true;
		}		

		public override void Shutdown()
		{
			Sort.SaveSettings();
		}

		#endregion
	}
}
