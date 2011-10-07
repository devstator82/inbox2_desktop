using System;
using System.Linq;
using System.Threading;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Web;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Logging;

namespace Inbox2.Core.Threading.Handlers.Matchers
{
	public enum ContactMatchResult
	{
		NoMatch,
		MatchedOnPerson,
		MatchedOnProfile,
		MatchedOnChannelProfileKey,
		Error,
	}

	public class ContactMatcher
	{
		private readonly ChannelContact contact;

		private Person person;
		private Profile profile;
		private VirtualMailBox mailbox;

		private ContactMatchResult result = ContactMatchResult.NoMatch;

		public ContactMatchResult Result
		{
			get { return result; }
		}

		public ContactMatcher(ChannelContact contact)
		{
			this.contact = contact;
			this.mailbox = VirtualMailBox.Current;
		}

		public void Execute()
		{
			// Validate the minimum required fields
			if (String.IsNullOrEmpty(contact.Person.Name.Trim()))
			{
				Logger.Warn("Contact for channelprofilekey [{0}] has an empty name and an invalid sourceaddress, ignoring entry", LogSource.Sync, contact.Profile.ChannelProfileKey);

				result = ContactMatchResult.Error;

				return;
			}

			if (String.IsNullOrEmpty(contact.Profile.ChannelProfileKey))
			{
				Logger.Debug("Profile for person [{0}] did not have a valid ChannelProfileKey, ignoring entry", LogSource.Sync, contact.Person.Name);

				result = ContactMatchResult.Error;

				return;
			}

			if (contact.Profile.SourceAddress == null)
			{
				Logger.Warn("Contact for channelprofilekey [{0}] has an invalid sourceaddress, ignoring entry", LogSource.Sync, contact.Profile.ChannelProfileKey);

				result = ContactMatchResult.Error;

				return;
			}

			// Some contacts (especially from gmail) don't have names but only email adrreses, 
			// use the email address as name in that case
			if (String.IsNullOrEmpty(contact.Person.Name.Trim()))
			{
				contact.Person.Name = contact.Profile.SourceAddress.ToString();
				contact.IsSoft = true;
			}

			// Try to match the person to our addressbook based on unique channelprofilekey
			using (mailbox.Profiles.ReaderLock)
				profile = mailbox.Profiles.FirstOrDefault(p => p.ChannelProfileKey == contact.Profile.ChannelProfileKey);

			if (profile != null)
			{
				// Find person belonging to profile
				EnsurePerson();

				result = ContactMatchResult.MatchedOnProfile;

				return;
			}

			// Profile not found on channel profile key, perform a match on person name
			FindPersonByName();

			// Person has been redirected
			if (person != null && person.RedirectPersonId.HasValue)
				person = mailbox.Persons.FirstOrDefault(p => p.PersonId == person.RedirectPersonId);

			if (person != null)
			{
				var matchOnAddress = mailbox.Profiles.FirstOrDefault(p => p.Address == contact.Profile.SourceAddress.Address);

				if (matchOnAddress != null)
				{
					Logger.Debug("Found soft profile [{0}] for person [{1}] on address match [{2}], removing from mailbox and recreating new one...", LogSource.Sync,
						matchOnAddress.ProfileId, contact.Person.Name, contact.Profile.SourceAddress.Address);

					// We have a soft profile on the same address that we are now receiving a hard profile for,
					// remove the soft profile so that the matcher will create a new hard profile.
					ClientState.Current.DataService.Delete(matchOnAddress);
				}

				// Doesn't have this profile yet, add it
				Logger.Debug("Found new profile [{0}] for person [{1}]", LogSource.Sync, contact.Profile.ChannelProfileKey, contact.Person.Name);

				// Append new profile
				SaveProfile(contact);

				result = ContactMatchResult.MatchedOnPerson;
			}
			else
			{
				profile = mailbox.Profiles.FirstOrDefault(p => p.Address.Equals(contact.Profile.SourceAddress.Address, StringComparison.InvariantCultureIgnoreCase));

				// Try to match to profile address
				if (profile != null)
				{
					EnsurePerson();

					result = ContactMatchResult.MatchedOnProfile;

					return;
				}

				// Unable to match, create new person
				SavePerson(contact);

				// Create new profile
				SaveProfile(contact);
			}
		}

		void FindPersonByName()
		{
			person = mailbox.Persons.FirstOrDefault(p => p.Name.Equals(contact.Person.Name, StringComparison.InvariantCultureIgnoreCase)
					|| p.Name.Replace(" ", "").IndexOf(contact.Person.Name, StringComparison.InvariantCultureIgnoreCase) > -1);
		}

		void EnsurePerson()
		{
			using (mailbox.Persons.ReaderLock)
				person = mailbox.Persons.FirstOrDefault(p => p.PersonId == profile.PersonId);

			if (person == null)
			{
				FindPersonByName();

				// Person not found on id or name, create a new person entry
				if (person == null)
					SavePerson(contact);
			}
		}

		void SavePerson(ChannelContact channelContact)
		{
			person = channelContact.Person.DuckCopy<Person>();
			person.PersonId = person.PersonId;
			person.Firstname = person.Firstname.Capitalize();
			person.Lastname = person.Lastname.Capitalize();

			mailbox.Persons.Add(person);

			ClientState.Current.DataService.Save(person);

			Logger.Debug("Profile saved successfully in ContactMatcher. Person = {0}", LogSource.Sync, person.PersonId);			
		}

		void SaveProfile(ChannelContact channelContact)
		{
			profile = channelContact.Profile.DuckCopy<Profile>();
			profile.PersonId = person.PersonId.Value;
			profile.IsSoft = channelContact.IsSoft;

			// SourceAddress can be null with for instance phone contacts
			if (profile.SourceAddress != null)
			{
				if (String.IsNullOrEmpty(profile.ScreenName))
					profile.ScreenName = profile.SourceAddress.DisplayName;

				if (String.IsNullOrEmpty(profile.Address))
					profile.Address = profile.SourceAddress.Address;
			}

			try
			{
				if (channelContact.Profile.ChannelAvatar != null &&
					(channelContact.Profile.ChannelAvatar.ContentStream != null || !String.IsNullOrEmpty(channelContact.Profile.ChannelAvatar.Url)))
				{
					var streamname = Guid.NewGuid().GetHash(12) + "png";

					if (channelContact.Profile.ChannelAvatar.ContentStream == null)
					{
						var helper = new WebContentStreamHelper(channelContact.Profile.ChannelAvatar.Url);

						channelContact.Profile.ChannelAvatar.ContentStream = helper.GetContentStream();
					}

					using (channelContact.Profile.ChannelAvatar.ContentStream)
						ClientState.Current.Storage.Write("a", streamname, channelContact.Profile.ChannelAvatar.ContentStream);

					profile.AvatarStreamName = streamname;
				}
			}
			catch (Exception ex)
			{
				Logger.Error("An error occured while trying to save avatar. ChannelProfileKey = {0} Exception = {1}", LogSource.Sync, channelContact.Profile.ChannelProfileKey, ex);
			}

			mailbox.Profiles.Add(profile);

			Thread.CurrentThread.ExecuteOnUIThread(() => person.Add(profile));

			ClientState.Current.DataService.Save(profile);
			ClientState.Current.Search.Store(profile);

			Logger.Debug("Profile saved successfully in ContactMatcher. Person = {0}, Profile.SourceAddress = {1}", LogSource.Sync, person.PersonId, profile.SourceAddress);
		}
	}
}
