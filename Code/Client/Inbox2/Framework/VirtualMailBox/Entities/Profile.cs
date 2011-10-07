using System;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Persistance;
using Inbox2.Framework.Threading.AsyncUpdate;
using Inbox2.Framework.UI.AsyncImage;
using Inbox2.Platform.Channels.Attributes;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Collections;
using Inbox2.Platform.Framework.Extensions;

namespace Inbox2.Framework.VirtualMailBox.Entities
{
	[PersistableClass]
	[Serializable]
	public class Profile : INotifyPropertyChanged, IEntityBase
	{
		public event PropertyChangedEventHandler PropertyChanged;
		
		private AsyncAvatar loader;
		private ChannelConfiguration config;
		private bool configLoaded;

		[PrimaryKey] public long? ProfileId { get; set; }
		[Persist] public string ProfileKey { get; set; }
		[Persist] public long PersonId { get; set; }
		[Persist] public bool IsSoft { get; set; }
		[Persist] public string ChannelProfileKey { get; set; }
		[Persist] public long SourceChannelId { get; set; }
		[Persist] public ProfileType ProfileType { get; set; }
		[Persist] public string AvatarStreamName { get; set; }
		[Persist] public string ScreenName { get; set; }
		[Persist] public string Address { get; set; }
		[Persist] public string Url { get; set; }
		[Persist] public SourceAddress SourceAddress { get; set; }
		[Persist] public string Location { get; set; }
		[Persist] public string GeoLocation { get; set; }
		[Persist] public string CompanyName { get; set; }
		[Persist] public string Title { get; set; }
		[Persist] public string Street { get; set; }
		[Persist] public string HouseNumber { get; set; }
		[Persist] public string ZipCode { get; set; }
		[Persist] public string City { get; set; }
		[Persist] public string State { get; set; }
		[Persist] public string Country { get; set; }
		[Persist] public string CountryCode { get; set; }
		[Persist] public string PhoneNr { get; set; }
		[Persist] public string MobileNr { get; set; }
		[Persist] public string FaxNr { get; set; }
		[Persist] public DateTime DateCreated { get; set; }

		public string UniqueId
		{
			get { return EntityKeyPrefixes.Profile + ProfileId; }
		}

		public ChannelConfiguration SourceChannel
		{
			get
			{
				if (!configLoaded)
				{
					var channel = ChannelsManager.GetChannelObject(SourceChannelId);

					if (channel != null)
						config = channel.Configuration;

					configLoaded = true;
				}

				return config;
			}
		}

		public Person Person { get; set; }
		
		public ImageSource Avatar
		{
			get
			{
				if (String.IsNullOrEmpty(AvatarStreamName))
					return AsyncImageQueue._Profile;

				if (loader == null)
				{
					string filename = ClientState.Current.Storage.ResolvePhysicalFilename("a", AvatarStreamName);
					loader = new AsyncAvatar(filename, () => OnPropertyChanged("Avatar"), () => loader = null);
				}

				return loader.AsyncSource;
			}
		}

		public AdvancedObservableCollection<Message> Messages { get; private set; }
		public AdvancedObservableCollection<Document> Documents { get; private set; }

		public Profile()
		{
			ProfileKey = Guid.NewGuid().GetHash();
			Messages = new AdvancedObservableCollection<Message>();
			Documents = new AdvancedObservableCollection<Document>();
		}

		public Profile(IDataReader reader) : this()
		{
			ProfileId = reader.GetInt64(0);

			if (!reader.IsDBNull(1))
				ProfileKey = reader.GetString(1);

			if (!reader.IsDBNull(2))
				PersonId = reader.GetInt64(2);

			if (!reader.IsDBNull(3))
				IsSoft = reader.ReadBoolean(3);

			if (!reader.IsDBNull(4))
				ChannelProfileKey = reader.GetString(4);

			if (!reader.IsDBNull(5))
				SourceChannelId = reader.GetInt64(5);

			try
			{
				if (!reader.IsDBNull(6))
					ProfileType = (ProfileType)Enum.Parse(typeof(ProfileType), reader.GetString(6));
			}
			catch (Exception)
			{
				// Due to an update in the ProfileType enumeration, revert back to default in case of an exception
				ProfileType = ProfileType.Default;
			}			

			if (!reader.IsDBNull(7))
				AvatarStreamName = reader.GetString(7);

			if (!reader.IsDBNull(8))
				ScreenName = reader.GetString(8);

			if (!reader.IsDBNull(9))
				Address = reader.GetString(9);

			if (!reader.IsDBNull(10))
				Url = reader.GetString(10);

			if (!reader.IsDBNull(11))
				SourceAddress = new SourceAddress(reader.GetString(11));

			if (!reader.IsDBNull(12))
				Location = reader.GetString(12);

			if (!reader.IsDBNull(13))
				GeoLocation = reader.GetString(13);

			if (!reader.IsDBNull(14))
				CompanyName = reader.GetString(14);

			if (!reader.IsDBNull(15))
				Title = reader.GetString(15);

			if (!reader.IsDBNull(16))
				Street = reader.GetString(16);

			if (!reader.IsDBNull(17))
				HouseNumber = reader.GetString(17);

			if (!reader.IsDBNull(18))
				ZipCode = reader.GetString(18);

			if (!reader.IsDBNull(19))
				City = reader.GetString(19);

			if (!reader.IsDBNull(20))
				State = reader.GetString(20);

			if (!reader.IsDBNull(21))
				Country = reader.GetString(21);

			if (!reader.IsDBNull(22))
				CountryCode = reader.GetString(22);

			if (!reader.IsDBNull(23))
				PhoneNr = reader.GetString(23);

			if (!reader.IsDBNull(24))
				MobileNr = reader.GetString(24);

			if (!reader.IsDBNull(25))
				FaxNr = reader.GetString(25);

			if (!reader.IsDBNull(26))
				DateCreated = reader.ReadDateTime(26);
		}

		public void JoinWith(Person person)
		{
			// Remove profile from old person
			if (Person != null)
			{
				Person.Profiles.Remove(this);
				Person.Messages.RemoveAll(m => Messages.Contains(m));
				Person.Documents.RemoveAll(d => Documents.Contains(d));

				if (Person.Profiles.Count == 0)
				{
					// If no profiles left, update person to indicate redirection
					Person.RedirectPersonId = Person.PersonId;

					AsyncUpdateQueue.Enqueue(Person);
				}
			}

			// Add profile to new person
			Person = person;
			Person.Profiles.Add(this);
			Person.Messages.AddRange(Messages);
			Person.Documents.AddRange(Documents);

			// Update person reference
			PersonId = Person.PersonId.Value;

			// Save profile to database
			AsyncUpdateQueue.Enqueue(this);
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				Thread.CurrentThread.RaiseUIPropertyChanged(this, propertyName, PropertyChanged);
			}
		}

		public void UpdateProperty(string propertyName)
		{
			OnPropertyChanged(propertyName);
		}
	}
}