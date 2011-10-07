using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Framework.Locking;
using Inbox2.Platform.Interfaces;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Platform.Logging;

namespace Inbox2.Framework.VirtualMailBox
{
	public class Data
	{
		public event EventHandler<EventArgs> PartialLoadComplete;

		private readonly ThreadSafeDictionary<string, List<Label>> labels;
		private readonly ThreadSafeCollection<Conversation> conversations;
		private readonly ThreadSafeCollection<Message> messages;
		private readonly ThreadSafeCollection<Document> documents;
		private readonly ThreadSafeCollection<DocumentVersion> versions;
		private readonly ThreadSafeCollection<Person> persons;
		private readonly ThreadSafeCollection<Profile> profiles;
		private readonly ThreadSafeCollection<UserStatus> statusUpdates;
		private readonly ThreadSafeCollection<UserStatusAttachment> statusUpdateAttachments;

		private Dictionary<long, Message> keyedMessages;
		private Dictionary<long, Document> keyedDocuments;
		private Dictionary<long, Person> keyedPersons;
		private Dictionary<string, Conversation> keyedConversations;
		private Dictionary<string, UserStatus> keyedStatusUpdates;
		private Dictionary<string, Profile> keyedProfiles;

		#region Properties

		public ThreadSafeDictionary<string, List<Label>> Labels
		{
			get { return labels; }
		}

		public ThreadSafeCollection<Conversation> Conversations
		{
			get { return conversations; }
		}

		public ThreadSafeCollection<Message> Messages
		{
			get { return messages; }
		}

		public ThreadSafeCollection<Document> Documents
		{
			get { return documents; }
		}

		public ThreadSafeCollection<DocumentVersion> DocumentVersions
		{
			get { return versions; }
		}

		public ThreadSafeCollection<Person> Persons
		{
			get { return persons; }
		}

		public ThreadSafeCollection<Profile> Profiles
		{
			get { return profiles; }
		}

		public ThreadSafeCollection<UserStatus> StatusUpdates
		{
			get { return statusUpdates; }
		}

		public ThreadSafeCollection<UserStatusAttachment> StatusUpdateAttachments
		{
			get { return statusUpdateAttachments; }
		}

		#endregion

		public Data()
		{
			labels = new ThreadSafeDictionary<string, List<Label>>();

			conversations = new ThreadSafeCollection<Conversation>();
			messages = new ThreadSafeCollection<Message>();
			documents = new ThreadSafeCollection<Document>();
			versions = new ThreadSafeCollection<DocumentVersion>();
			persons = new ThreadSafeCollection<Person>();
			profiles = new ThreadSafeCollection<Profile>();
			statusUpdates = new ThreadSafeCollection<UserStatus>();
			statusUpdateAttachments = new ThreadSafeCollection<UserStatusAttachment>();

			keyedMessages = new Dictionary<long, Message>();
			keyedDocuments = new Dictionary<long, Document>();
			keyedPersons = new Dictionary<long, Person>();
			keyedConversations = new Dictionary<string, Conversation>();
			keyedStatusUpdates = new Dictionary<string, UserStatus>();
			keyedProfiles = new Dictionary<string, Profile>();
		}				

		internal void Load()
		{
			using (new CodeTimer("VirtualMailBox/Load"))
			{
				// Add our fake message that we need to work around the crappy WPF filtering bug
				messages.Add(new Message
				{
					MessageId = -1,
					From = new SourceAddress(),
					DateReceived = DateTime.MinValue,
					IsRead = true
				});

				WaitHandle.WaitAll(new[] {
					new DataLoadTask(LoadMessages).ExecuteAsync(),
					new DataLoadTask(LoadConversations).ExecuteAsync(),
					new DataLoadTask(LoadStatusUpdates).ExecuteAsync() });

				BuildConversations();

				if (PartialLoadComplete != null)
					PartialLoadComplete(this, EventArgs.Empty);

				WaitHandle.WaitAll(new[] {
					new DataLoadTask(LoadDocuments).ExecuteAsync(),
					new DataLoadTask(LoadAddressBook).ExecuteAsync() });

				// Don't need these dictionaries anymore
				keyedMessages = null;
				keyedDocuments = null;
				keyedPersons = null;
				keyedConversations = null;
				keyedStatusUpdates = null;
				keyedProfiles = null;
			}			
		}

		void LoadMessages()
		{
			using (new CodeTimer("VirtualMailBox/Load/Messages"))
			{
				const string query = "select MessageId, MessageKey, MessageNumber, MessageIdentifier, InReplyTo, SourceFolder, Size, ConversationIdentifier, SourceChannelId, "
					+ "TargetChannelId, MessageFolder, [From], ReturnTo, [To], CC, BCC, Context, BodyTextStreamName, BodyHtmlStreamName, Labels, SendLabels, IsRead, IsStarred, "
					+ "TargetMessageState, DateRead, DateAction, DateReply, DateReceived, DateSent from Messages";

				using (var reader = ClientState.Current.DataService.ExecuteReader(query))
				{
					while (reader.Read())
					{
						var message = new Message(reader);

						messages.Add(message);
						keyedMessages.Add(message.MessageId.Value, message);

						#region Load Labels

						if (!String.IsNullOrEmpty(message.Labels))
						{
							foreach (var labelstring in message.Labels.SmartSplit("|").ToList())
							{
								var label = new Label(labelstring);
								var key = label.Labelname.ToLower();

								if (!labels.ContainsKey(key))
								{
									labels.Add(key, new List<Label>());

									if (label.LabelType == LabelType.Custom)
										EventBroker.Publish(AppEvents.LabelCreated, key);
								}

								label.Messages.Add(message);

								labels[key].Add(label);

								message.LabelsList.Add(label);
							}
						}

						#endregion
					}
				}
			}
		}

		void LoadConversations()
		{
			using (new CodeTimer("VirtualMailBox/Load/Conversations"))
			{
				const string query = "select ConversationId, ConversationIdentifier, Context from Conversations";

				using (var reader = ClientState.Current.DataService.ExecuteReader(query))
				{
					while (reader.Read())
					{
						var conversation = new Conversation(reader);

						conversations.Add(conversation);
						keyedConversations.Add(conversation.ConversationIdentifier, conversation);
					}
				}
			}
		}

		void BuildConversations()
		{
			using (new CodeTimer("VirtualMailBox/Load/BuildConversations"))
			{
				#region Add messages to conversations

				foreach (var message in messages)
				{
					if (String.IsNullOrEmpty(message.ConversationIdentifier))
						continue;

					// Try to find the conversation from mailbox
					if (!keyedConversations.ContainsKey(message.ConversationIdentifier))
					{
						Logger.Warn("Conversation not found. ConversationIdentifier = {0}", LogSource.Actions,
									message.ConversationIdentifier);

						continue;
					}

					keyedConversations[message.ConversationIdentifier].Add(message);
				}

				#endregion
			}
		}

		void LoadStatusUpdates()
		{
			using (new CodeTimer("VirtualMailBox/Load/StatusUpdates"))
			{
				const string query1 = "select StatusId, StatusKey, ParentKey, ProfileId, SourceChannelId, TargetChannelId, ChannelStatusKey, [From], [To], [Status], InReplyTo, StatusType, SearchKeyword, IsRead, SortDate, DateRead, DateCreated from UserStatus";
				const string query2 = "select AttachmentId, UserId, StatusKey, PreviewImageUrl, PreviewAltText, TargetUrl, MediaType, DateCreated from UserStatusAttachments";

				var temp = new List<UserStatus>();

				using (var reader = ClientState.Current.DataService.ExecuteReader(query1))
				{
					while (reader.Read())
					{
						var statusUpdate = new UserStatus(reader);

						temp.Add(statusUpdate);
						keyedStatusUpdates.Add(statusUpdate.StatusKey, statusUpdate);
					}
				}

				using (var reader = ClientState.Current.DataService.ExecuteReader(query2))
					while (reader.Read())
						statusUpdateAttachments.Add(new UserStatusAttachment(reader));

				var dbStatusUpdateAttachments = ClientState.Current.DataService.SelectAll<UserStatusAttachment>().ToList();

				#region Add child statusses to parents

				foreach (var status in temp)
				{
					if (!String.IsNullOrEmpty(status.ParentKey))
					{
						if (keyedStatusUpdates.ContainsKey(status.ParentKey))
							keyedStatusUpdates[status.ParentKey].Add(status);
					}
				}

				#endregion

				#region Add status update attachments

				foreach (var statusUpdateAttachment in dbStatusUpdateAttachments)
				{
					if (keyedStatusUpdates.ContainsKey(statusUpdateAttachment.StatusKey))
						keyedStatusUpdates[statusUpdateAttachment.StatusKey].Attachments.Add(statusUpdateAttachment);
				}

				#endregion

				statusUpdates.AddRange(temp.OrderByDescending(s => s.ParentSortDate));
			}
		}

		void LoadDocuments()
		{
			using (new CodeTimer("VirtualMailBox/Load/Documents"))
			{
				// Load documents
				const string query1 = "select DocumentId, Filename, SourceChannelId, TargetChannelId, ContentType, ContentId, DocumentKey, StreamName, Crc32, Size, IsRead, DocumentFolder, Labels, DateReceived, DateSent, DateCreated, DateModified from Documents";
				const string query2 = "select VersionId, DocumentId, MessageId, Filename, SourceChannelId, TargetChannelId, [From], [To], StreamName, Crc32, Size, DateReceived, DateSent, DateCreated, DateModified from DocumentVersions";

				using (var reader = ClientState.Current.DataService.ExecuteReader(query1))
				{
					while (reader.Read())
					{
						var document = new Document(reader);

						documents.Add(document);
						keyedDocuments.Add(document.DocumentId.Value, document);
					}
				}

				using (var reader = ClientState.Current.DataService.ExecuteReader(query2))
					while (reader.Read())
						versions.Add(new DocumentVersion(reader));

				#region Add versions to documents

				foreach (var version in versions)
				{
					if (keyedDocuments.ContainsKey(version.DocumentId))
					{
						keyedDocuments[version.DocumentId].Versions.Add(version);

						// Add document to message if there is an association
						if (version.MessageId.HasValue)
						{
							if (keyedMessages.ContainsKey(version.MessageId.Value))
								keyedMessages[version.MessageId.Value].Documents.Add(keyedDocuments[version.DocumentId]);
						}
					}
				}

				foreach (var document in documents)
				{
					if (!String.IsNullOrEmpty(document.Labels))
					{
						foreach (var labelstring in document.Labels
							.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries)
							.Where(s => !String.IsNullOrEmpty(s))
							.ToList())
						{
							var label = new Label(labelstring);
							var key = label.Labelname.ToLower();

							if (!labels.ContainsKey(key))
								labels.Add(key, new List<Label>());

							label.Documents.Add(document);

							labels[key].Add(label);

							document.LabelsList.Add(label);
						}
					}
				}

				#endregion
			}
		}

		void LoadAddressBook()
		{
			using (new CodeTimer("VirtualMailBox/Load/AddressBook"))
			{
				const string query1 = "select PersonId, PersonKey, RedirectPersonId, SourceChannelId, Firstname, Lastname, DateOfBirth, Locale, Gender, Timezone, DateCreated from Persons";
				const string query2 = "select ProfileId, ProfileKey, PersonId, IsSoft, ChannelProfileKey, SourceChannelId, ProfileType, AvatarStreamName, ScreenName, Address, Url, SourceAddress, Location, GeoLocation, CompanyName, Title, Street, HouseNumber, ZipCode, City, State, Country, CountryCode, PhoneNr, MobileNr, FaxNr, DateCreated from Profiles";

				var temp = new List<Person>();

				using (var reader = ClientState.Current.DataService.ExecuteReader(query1))
				{
					while (reader.Read())
					{
						var person = new Person(reader);

						temp.Add(person);
						keyedPersons.Add(person.PersonId.Value, person);
					}
				}

				using (var reader = ClientState.Current.DataService.ExecuteReader(query2))
				{
					while (reader.Read())
					{
						var profile = new Profile(reader);

						profiles.Add(profile);

						if (profile.Address != null)
							if (!keyedProfiles.ContainsKey(profile.Address))
								keyedProfiles.Add(profile.Address, profile);
					}
				}

				#region Add profiles to persons

				foreach (var profile in profiles)
				{
					if (keyedPersons.ContainsKey(profile.PersonId))
						keyedPersons[profile.PersonId].Add(profile);
				}

				#endregion

				// Swap persons into addressbook
				persons.AddRange(temp);

				#region Add messages to persons and profiles

				foreach (var message in messages)
				{
					var profile = LoadPerson(message, message.From);

					// Set profile
					if (profile != null)
						message.Profile = profile;

					foreach (var to in message.To)
						LoadPerson(message, to);
				}

				#endregion
			}

			// Rebuild score for all persons
			persons.ForEach(p => p.RebuildScore());
		}

		Profile LoadPerson(Message message, SourceAddress address)
		{
			if (!keyedProfiles.ContainsKey(address.Address))
				return null;

			var profile = keyedProfiles[address.Address];

			if (profile.Person == null)
			{
				Logger.Warn("Profile found, but person was null. MessageId = {0}", LogSource.Actions, message.MessageId);

				return null;
			}

			if (!profile.Messages.Contains(message))
			{
				profile.Messages.Add(message);
				profile.Documents.AddRange(message.Documents);
			}

			if (!profile.Person.Messages.Contains(message))
			{
				profile.Person.Messages.Add(message);
				profile.Person.Documents.AddRange(message.Documents);
			}

			return profile;
		}
	}
}
