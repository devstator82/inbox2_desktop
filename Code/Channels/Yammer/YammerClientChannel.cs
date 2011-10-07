using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Framework.OAuth;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Channels.Yammer
{
	public class YammerClientChannel : IClientInputChannel, IClientContactsChannel, IClientStatusUpdatesChannel
	{
		#region Properties

		public IChannelCredentialsProvider CredentialsProvider { get; set; }

		public string Hostname
		{
			get { return "http://twitter.com"; }
			set { }
		}

		public int Port
		{
			get { return 80; }
			set { }
		}

		public bool IsSecured
		{
			get { return false; }
			set { }
		}

		public int MaxConcurrentConnections
		{
			get { return 2; }
			set { }
		}

		public bool IsEnabled { get; set; }

		public string Protocol
		{
			get { return "Yammer API"; }
		}

		public string SourceAdress
		{
			get { return CredentialsProvider.GetCredentials().Claim; }
		}

		public string AuthMessage { get; private set; }

		#endregion

		public ConnectResult Connect()
		{
			return ConnectResult.Success;
		}

		public IEnumerable<ChannelFolder> GetFolders()
		{
			yield return new ChannelFolder("inbox", "inbox", ChannelFolderType.Inbox);
		}

		public void SelectFolder(ChannelFolder folder)
		{
		}

		public IEnumerable<ChannelMessageHeader> GetHeaders()
		{
			yield break;
		}

		public IEnumerable<ChannelMessage> GetMessage(ChannelMessageHeader header)
		{
			yield break;
		}

		public IEnumerable<ChannelContact> GetContacts()
		{
			foreach (var ycontact in GetYammerContacts())
			{
				var contact = new ChannelContact();
				contact.Person.Name = ycontact.FullName;

				contact.Profile.ChannelProfileKey = ycontact.Id;
				contact.Profile.ProfileType = ProfileType.Social;
				contact.Profile.ScreenName = ycontact.ScreenName;
				contact.Profile.Url = ycontact.Url;
				contact.Profile.Location = ycontact.Location;
				contact.Profile.Title = ycontact.Title;
				contact.Profile.SourceAddress = new SourceAddress(ycontact.ScreenName, ycontact.FullName);

				contact.Profile.ChannelAvatar = new ChannelAvatar();
				contact.Profile.ChannelAvatar.Url = ycontact.AvatarUrl;

				yield return contact;
			}
		}

		IClientContactsChannel IClientContactsChannel.Clone()
		{
			return new YammerClientChannel
				{
					Hostname = Hostname,
					Port = Port,
					IsSecured = IsSecured,
					MaxConcurrentConnections = MaxConcurrentConnections,
					CredentialsProvider = CredentialsProvider
				};
		}

		IEnumerable<YammerContact> GetYammerContacts()
		{
			int page = 1;

			while (true)
			{
				var result = YammerWebRequest.PerformRequest(new Uri("https://www.yammer.com/api/v1/users.xml?page=" + page), 
					ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, ChannelHelper.Token, ChannelHelper.TokenSecret);

				if (result.Element("response").Elements().Count() == 0)
					yield break;

				foreach (var response in result.Elements("response").Elements("response"))
					yield return ParseYammerContact(response);

				page++;
			}
		}		

		public ChannelSocialProfile GetProfile()
		{
			var result = YammerWebRequest.PerformRequest(new Uri("https://www.yammer.com/api/v1/users/current.xml"),
				ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, ChannelHelper.Token, ChannelHelper.TokenSecret);

			var contact = ParseYammerContact(result.Element("response"));

			return new ChannelSocialProfile
			{
				Id = contact.Id,
				FullName = contact.FullName,
				AvatarUrl = contact.AvatarUrl
			};
		}

		public IEnumerable<ChannelStatusUpdate> GetMentions(int pageSize)
		{
			yield break;
		}

		public IEnumerable<ChannelStatusUpdate> GetUpdates(int pageSize)
		{
			var contacts = GetYammerContacts();
			var result = YammerWebRequest.PerformRequest(new Uri("https://www.yammer.com/api/v1/messages.xml"), 
				ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, ChannelHelper.Token, ChannelHelper.TokenSecret);

			var dict = new Dictionary<string, List<ChannelStatusUpdate>>();

			foreach (var element in result.Element("response").Elements("messages").Elements("message"))
			{
				var update = new ChannelStatusUpdate();
				var contact = contacts.First(c => c.Id == element.Element("sender-id").Value);

				update.ChannelStatusKey = element.Element("id").Value;
				update.From = new SourceAddress(contact.Id, contact.FullName, contact.AvatarUrl);
				update.Status = element.Element("body").Element("plain").Value;
				update.DatePosted = DateTime.Parse(element.Element("created-at").Value);

				foreach (var attachmentElement in element.Descendants("attachment"))
				{
					var attachment = new ChannelStatusUpdateAttachment();
					var type = attachmentElement.Element("type").Value;

					attachment.PreviewAltText = attachmentElement.Element("name").Value;
					attachment.TargetUrl = attachmentElement.Element("web-url").Value;

					if (type == "image")
					{
						attachment.MediaType = StatusAttachmentTypes.Photo;
						attachment.PreviewImageUrl = attachmentElement.Element("image").Element("thumbnail-url").Value;
					}
					else
					{
						attachment.MediaType = StatusAttachmentTypes.Document;						
					}

					update.Attachments.Add(attachment);
				}

				var threadid = element.Element("thread-id").Value;

				if (!dict.ContainsKey(threadid))
					dict.Add(threadid, new List<ChannelStatusUpdate>());

				dict[threadid].Add(update);
			}

			var updates = new List<ChannelStatusUpdate>();

			foreach (var value in dict.Values)
			{
				var sortedList = value.OrderBy(c => c.DatePosted).ToList();
				var first = sortedList.First();

				first.Children.AddRange(sortedList.Skip(1));

				updates.Add(first);
			}

			return updates.OrderByDescending(s => s.SortDate);
		}

		public IEnumerable<ChannelStatusUpdate> GetUserUpdates(string username, int pageSize)
		{
			const string url = "https://www.yammer.com/api/v1/messages/from_user/{0}.xml";
			var result = YammerWebRequest.PerformRequest(new Uri(String.Format(url, username)),
				ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, ChannelHelper.Token, ChannelHelper.TokenSecret);

			foreach (var element in result.Element("response").Elements("messages").Elements("message"))
			{
				var update = new ChannelStatusUpdate();

				update.ChannelStatusKey = element.Element("id").Value;
				update.From = new SourceAddress(element.Element("sender-id").Value, username);
				update.Status = element.Element("body").Element("plain").Value;
				update.DatePosted = DateTime.Parse(element.Element("created-at").Value);

				yield return update;
			}
		}

		public IEnumerable<ChannelStatusUpdate> GetUpdates(string keyword, int pageSize)
		{
			yield break;
		}

		public void UpdateMyStatus(ChannelStatusUpdate update)
		{
			var p = new Dictionary<string, object> { {"body", update.Status} };

			if (!String.IsNullOrEmpty(update.InReplyTo))
				p.Add("replied_to_id", update.InReplyTo);

			YammerWebRequest.PerformRequest(new Uri("https://www.yammer.com/api/v1/messages/"), ChannelHelper.ConsumerKey,
				ChannelHelper.ConsumerSecret, ChannelHelper.Token, ChannelHelper.TokenSecret, p);
		}

		IClientStatusUpdatesChannel IClientStatusUpdatesChannel.Clone()
		{
			return new YammerClientChannel
				{
					Hostname = Hostname,
					Port = Port,
					IsSecured = IsSecured,
					MaxConcurrentConnections = MaxConcurrentConnections,
					CredentialsProvider = CredentialsProvider
				};
		}

		public bool Disconnect()
		{
			return true;
		}

		public IClientInputChannel Clone()
		{
			YammerClientChannel channel = new YammerClientChannel();
			channel.Hostname = Hostname;
			channel.Port = Port;
			channel.IsSecured = IsSecured;
			channel.MaxConcurrentConnections = MaxConcurrentConnections;
			channel.CredentialsProvider = CredentialsProvider;

			return channel;
		}

		YammerContact ParseYammerContact(XElement response)
		{
			YammerContact contact = new YammerContact();
			contact.Id = response.Element("id").Value;
			contact.ScreenName = response.Element("name").Value;			
			contact.FullName = response.Element("full-name").Value;
			contact.Url = response.Element("web-url").Value;
			contact.Location = response.Element("location").Value;
			contact.Title = response.Element("job-title").Value;
			contact.AvatarUrl = response.Element("mugshot-url").Value;

			return contact;
		}

		public void Dispose()
		{
			
		}
	}
}
