using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Threading;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Text;
using Inbox2.Platform.Logging;

namespace Inbox2.Core.Threading.Handlers.Matchers
{
	public class ProfileMatcher
	{
		private readonly Message message;
		private readonly VirtualMailBox mailbox;

		public ProfileMatcher(Message message)
		{
			this.message = message;
			this.mailbox = VirtualMailBox.Current;
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
			List<Profile> profiles;

			using (mailbox.Profiles.ReaderLock)
				profiles = mailbox.Profiles.Where(p => p.Address == address.Address).ToList();

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
					List<Person> persons;

					// The non spaced matche helps with contacts that are for instance called waseemsadiq on twitter and Waseem Sadiq elsewhere
					using (mailbox.Persons.ReaderLock)
						persons = mailbox.Persons.Where(c => c.Name == name || c.Name == name.Replace(" ", String.Empty).Trim()).ToList();

					if (persons.Count > 0)
					{
						Logger.Debug("Profile for address {0} had contact, creating new profile", LogSource.Sync, address);

						// Add profile to existing person
						var person = persons.First();

						// Person has been redirected
						if (person.RedirectPersonId.HasValue)
						{
							using (mailbox.Persons.ReaderLock)
							{
								// Find redirected person
								Person person1 = person;

								person = mailbox.Persons.FirstOrDefault(p => p.PersonId == person1.RedirectPersonId);
							}
						}

						if (person != null)
						{
							// If personid does not have a value yet, spin until the object is written to the database
							while (!person.PersonId.HasValue)
								Thread.SpinWait(1000);

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

				Thread.CurrentThread.ExecuteOnUIThread(delegate
					{
						// Set profile
						message.Profile = profile;

						profile.Messages.Add(message);

						if (profile.Person != null)
						{
							profile.Person.Messages.Add(message);

							profile.Person.RebuildScore();
						}
					});

				return profile.PersonId;
			}
		}

		long SavePerson(SourceAddress address)
		{
			// Create new person
			Person person = DispatcherActivator<Person>.Create();

			person.Name = address.DisplayName;
			// See comment in SaveProfile method
			person.SourceChannelId =
				SourceAddress.IsValidEmail(address.Address) ? 0 : message.SourceChannelId; ;
			person.DateCreated = DateTime.Now;

			mailbox.Persons.Add(person);

			Thread.CurrentThread.ExecuteOnUIThread(() => person.Messages.Add(message));

			person.RebuildScore();

			ClientState.Current.DataService.Save(person);

			Logger.Debug("Person saved successfully in ProfileMatcher. Person = {0}", LogSource.Sync, person.PersonId);

			SaveProfile(person, address);

			return person.PersonId.Value;
		}

		void SaveProfile(Person person, SourceAddress address)
		{
			// Create new profile
			Profile profile = DispatcherActivator<Profile>.Create();

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

			mailbox.Profiles.Add(profile);

			Thread.CurrentThread.ExecuteOnUIThread(delegate
				{
					person.Profiles.Add(profile);

					// Set profile
					message.Profile = profile;

					profile.Messages.Add(message);
				});

			ClientState.Current.DataService.Save(profile);

			Logger.Debug("Profile saved successfully in ProfileMatcher. Person = {0}, Profile.SourceAddress = {1}", LogSource.Sync, person.PersonId, profile.SourceAddress);
		}
	}
}
