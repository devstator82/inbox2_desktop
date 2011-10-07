using System;
using System.Linq;
using Inbox2.Framework;
using Inbox2.Framework.Persistance;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Text;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Logging;

namespace Inbox2ClientWorker.Core.Threading.Handlers.Matchers
{
	public class ProfileMatcher
	{
		private readonly Message message;
		private readonly IDataService dataService;

		public ProfileMatcher(Message message)
		{
			this.message = message;
			this.dataService = ClientState.Current.DataService;
		}

		public void Execute()
		{
			var sourceaddresses =
				new SourceAddressCollection()
				.AddRange(message.To)
				.AddRange(message.CC)
				.AddRange(message.BCC);

			// Try to match every source address to a profile
			foreach (var address in sourceaddresses)
				ProcessSourceAddress(address);

			ProcessSourceAddress(message.From);
		}

		long ProcessSourceAddress(SourceAddress address)
		{
			var profiles = dataService.SelectAllBy<Profile>(new { Address = address.Address }).ToList();
			
			if (profiles.Count == 0)
			{
				Logger.Debug("Profile for address {0} not found", LogSource.Sync, address);

				if (String.IsNullOrEmpty(address.DisplayName))
				{
					Logger.Warn("Address {0} had no displayname, ignoring", LogSource.Sync, address);

					return -1;
				}
				else
				{
					// No profile found, try to match on person
					string name = PersonName.Parse(address.DisplayName).ToString();

					// The non spaced match helps with contacts that are for instance called waseemsadiq on twitter and Waseem Sadiq elsewhere
					var person = dataService.SelectBy<Person>(
						String.Format("select * from Persons where (Firstname || ' ' || Lastname) = '{0}' or Firstname || ' ' || Lastname = '{1}'", 
							name.AddSQLiteSlashes(), name.Replace(" ", "").AddSQLiteSlashes()));	

					if (person != null)
					{
						Logger.Debug("Profile for address {0} had contact, creating new profile", LogSource.Sync, address);

						// Person has been redirected
						if (person.RedirectPersonId.HasValue)
							person = dataService.SelectBy<Person>(new { PersonId = person.RedirectPersonId });

						if (person != null)
						{
							SaveProfile(person, address);

							return person.PersonId.Value;
						}
					}

					Logger.Debug("Person for address {0} not found, creating new person and profile", LogSource.Sync, address);

					// Create new soft contact and soft profile
					return SavePerson(address);
				}
			}
			else
			{
				var profile = profiles.First();

				return profile.PersonId;
			}
		}

		long SavePerson(SourceAddress address)
		{
			// Create new person
			var person = new Person();
			
			// See comment in SaveProfile method
			person.SourceChannelId =
				SourceAddress.IsValidEmail(address.Address) ? 0 : message.SourceChannelId; ;

			person.Name = address.DisplayName;
			person.DateCreated = DateTime.Now;
			
			ClientState.Current.DataService.Save(person);

			Logger.Debug("Person saved successfully in ProfileMatcher. Person = {0}", LogSource.Sync, person.PersonId);
			
			SaveProfile(person, address);

			return person.PersonId.Value;
		}

		void SaveProfile(Person person, SourceAddress address)
		{
			// Create new profile
			var profile = new Profile();

			profile.PersonId = person.PersonId.Value;

			// SourceChannelId is 0 if its a valid email (because soft email addresses are not 
			// nescessarily tied to any channel), otherwise its the SourceChannelId of the message
			// (usually Facebook/Twitter/etc)
			profile.SourceChannelId =
				SourceAddress.IsValidEmail(address.Address) ? 0 : message.SourceChannelId;

			profile.ScreenName = address.DisplayName;
			profile.Address = address.Address;
			profile.SourceAddress = address;
			profile.ProfileType = ProfileType.Default;
			profile.IsSoft = true;
			profile.DateCreated = DateTime.Now;			

			ClientState.Current.DataService.Save(profile);

			Logger.Debug("Profile saved successfully in ProfileMatcher. Person = {0}, Profile.SourceAddress = {1}", LogSource.Sync, person.PersonId, profile.SourceAddress);
		}
	}
}
