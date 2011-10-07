using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Persistance;
using Inbox2.Platform.Channels.Attributes;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Text;
using Inbox2.Platform.Framework.Collections;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Framework.VirtualMailBox.Entities
{
	[PersistableClass]
	[Serializable]
	public class Person : IEntityBase
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private double score;		

		[IndexAndStore] [PrimaryKey] public long? PersonId { get; set; }
		[Persist] public long? RedirectPersonId { get; set; }
		[Persist] public string PersonKey { get; set; }
		[Persist] public long SourceChannelId { get; set; }
		[IndexAndStore]
		[Persist] public string Firstname { get; set; }
		[IndexAndStore]
		[Persist] public string Lastname { get; set; }
		[Persist] public DateTime? DateOfBirth { get; set; }
		[Persist] public string Locale { get; set; }
		[Persist] public string Gender { get; set; }
		[Persist] public string Timezone { get; set; }
		[Persist] public DateTime DateCreated { get; set; }

		public AdvancedObservableCollection<Profile> Profiles { get; private set; }
		public AdvancedObservableCollection<Message> Messages { get; private set; }
		public AdvancedObservableCollection<Document> Documents { get; private set; }

		public IEnumerable<Profile> UniqueProfiles
		{
			get
			{
				return Profiles.Where(p => p.SourceChannelId > 0).Distinct(
					new Platform.Framework.Collections.EqualityComparer<Profile>((kvp1, kvp2) =>
						kvp1.SourceChannel.DisplayName == kvp2.SourceChannel.DisplayName, // Comparer
						kvp => kvp.SourceChannel.DisplayName.GetHashCode())); // Hash code function
			}
		}

		public SourceAddress SourceAddress
		{
			get
			{
				if (Profiles.Count == 0)
					return null;

				return Profiles[0].SourceAddress;
			}
		}

		public string Name
		{
			get
			{
				return String.Format("{0} {1}", Firstname, Lastname).Trim();
			}
			set
			{
				var res = PersonName.Parse(value);

				Firstname = res.Firstname;
				Lastname = res.Lastname;
			}
		}				

		public bool IsSoft
		{
			get { return Profiles.Count(p => p.IsSoft) == Profiles.Count; }
		}

		public string UniqueId
		{
			get { return EntityKeyPrefixes.Person + PersonId; }
		}

		public double Score
		{
			get { return score; }
		}		

		public Profile BestProfile
		{
			get
			{
				var profile = Profiles.FirstOrDefault(p => !String.IsNullOrEmpty(p.AvatarStreamName));

				return profile ?? Profiles.FirstOrDefault();
			}
		}

		public Person()
		{
			PersonKey = Guid.NewGuid().GetHash();
			Profiles = new AdvancedObservableCollection<Profile>();
			Messages = new AdvancedObservableCollection<Message>();
			Documents = new AdvancedObservableCollection<Document>();
		}

		public Person(SourceAddress address) : this()
		{
			if (String.IsNullOrEmpty(address.Address))
				throw new ApplicationException("Invalid address");

			if (!String.IsNullOrEmpty(address.DisplayName))
			{
				Name = address.DisplayName;
			}
			else
			{
				Name = address.Address.SmartSplit("@")[0];
			}
		}

		public Person(IDataReader reader) : this()
		{			
			PersonId = reader.GetInt64(0);

			if (!reader.IsDBNull(1))
				PersonKey = reader.GetString(1);

			if (!reader.IsDBNull(2))
				RedirectPersonId = reader.ReadInt64OrNull(2);

			if (!reader.IsDBNull(3))
				SourceChannelId = reader.GetInt64(3);

			if (!reader.IsDBNull(4))
				Firstname = reader.GetString(4);

			if (!reader.IsDBNull(5))
				Lastname = reader.GetString(5);

			if (!reader.IsDBNull(6))
				DateOfBirth = reader.ReadDateTimeOrNull(6);

			if (!reader.IsDBNull(7))
				Locale = reader.GetString(7);

			if (!reader.IsDBNull(8))
				Gender = reader.GetString(8);

			if (!reader.IsDBNull(9))
				Timezone = reader.GetString(9);

			if (!reader.IsDBNull(10))
				DateCreated = reader.ReadDateTime(10);
		}

		public void Add(Profile profile)
		{
			Profiles.Add(profile);

			profile.Person = this;
		}

		public void UpdateProperty(string propertyName)
		{
			OnPropertyChanged(propertyName);
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				Thread.CurrentThread.RaiseUIPropertyChanged(this, propertyName, PropertyChanged);
			}
		}

		public void RebuildScore()
		{
			// Every social channel is good for 0.25 points on the friend scale,
			// every hard profile is good for 0.10 points,
			// every soft profile is good for 0.7 points.
			var socialScore = Math.Min(
				(0.25 * Profiles.Count(p => p.SourceChannel != null && p.SourceChannel.StatusUpdatesChannel != null)) +
				(0.10 * Profiles.Count(p => (p.SourceChannel == null || p.SourceChannel.StatusUpdatesChannel == null) && !p.IsSoft)) +
				(0.07 * Profiles.Count(p => (p.SourceChannel == null || p.SourceChannel.StatusUpdatesChannel == null) && p.IsSoft)), 1);

			var outgoingMessages = Messages.Count(m => m.MessageFolder == Folders.SentItems) + 1;
			
			score = socialScore * outgoingMessages;
		}

		public override string ToString()
		{
			return String.Format("[{0}]", Name);
		}
	}
}