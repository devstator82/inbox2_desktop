using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Framework.VirtualMailBox
{
	public class VirtualMailBox
	{
		private static readonly VirtualMailBox _current = new VirtualMailBox();

		public static VirtualMailBox Current
		{
			get { return _current; }
		}
		
		private readonly Data data;
		private readonly object synclock;
		private readonly DateTime created;
		private bool initialized;
		private SearchKeyWords searchkeywords;		

		public event EventHandler<EventArgs> LoadComplete;
		public event EventHandler<EventArgs> InboxLoadComplete;

		/// <summary>
		/// Gets the last time this mailbox was created.
		/// </summary>
		public DateTime Created
		{
			get { return created; }
		}		

		/// <summary>
		/// Gets the saved search keywords.
		/// </summary>
		public SearchKeyWords StreamSearchKeywords
		{
			get
			{
				if (searchkeywords == null)
					searchkeywords = new SearchKeyWords();

				return searchkeywords;
			}
		}

		public ThreadSafeDictionary<string, List<Label>> Labels
		{
			get { return data.Labels; }
		}

		public ThreadSafeCollection<Conversation> Conversations
		{
			get { return data.Conversations; }
		}

		public ThreadSafeCollection<Message> Messages
		{
			get { return data.Messages; }
		}

		public ThreadSafeCollection<Document> Documents
		{
			get { return data.Documents; }
		}

		public ThreadSafeCollection<DocumentVersion> DocumentVersions
		{
			get { return data.DocumentVersions; }
		}

		public ThreadSafeCollection<Person> Persons
		{
			get { return data.Persons; }
		}

		public ThreadSafeCollection<Profile> Profiles
		{
			get { return data.Profiles; }
		}

		public ThreadSafeCollection<UserStatus> StatusUpdates
		{
			get { return data.StatusUpdates; }
		}

		public VirtualMailBox()
		{
			created = DateTime.Now;
			synclock = new object();
			data = new Data();
			data.PartialLoadComplete += DataPartialLoadComplete;
		}

		public Profile Find(SourceAddress address)
		{
			Profile found;

			using (Profiles.ReaderLock)
				found = Profiles.FirstOrDefault(p => p.Address.Equals(address.Address, StringComparison.InvariantCultureIgnoreCase));

			if (found == null)
			{
				found = new Profile {SourceAddress = address};
			}

			return found;
		}

		public Person FindPerson(SourceAddress address)
		{
			var profile = Find(address);

			if (profile != null && profile.Person != null)
				return profile.Person;

			return null;
		}

		public bool Load()
		{
			if (initialized == false)
			{
				lock (synclock)
				{
					if (initialized == false)
					{
						data.Load();

						Thread.CurrentThread.RaiseUIEventHandler(this, LoadComplete);

						initialized = true;

						// Trigger a collection on gen-2, also making sure the large object heap is collected,
						// see http://msdn.microsoft.com/en-us/magazine/cc534993.aspx for more info.
						GC.Collect(GC.MaxGeneration);
					}
				}
			}

			return data != null;
		}

		void DataPartialLoadComplete(object sender, EventArgs e)
		{
			Thread.CurrentThread.RaiseUIEventHandler(this, InboxLoadComplete);
		}		
	}
}