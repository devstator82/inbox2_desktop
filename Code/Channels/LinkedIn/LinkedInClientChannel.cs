using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Channels.Extensions;

namespace Inbox2.Channels.LinkedIn
{
	public class LinkedInClientChannel : IClientInputChannel, IClientContactsChannel, IClientStatusUpdatesChannel
	{
		public string Hostname
		{
			get { return "http://m.linkedin.com"; }
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

		public bool IsEnabled { get; set; }

		public int MaxConcurrentConnections { get; set; }

		public IChannelCredentialsProvider CredentialsProvider { get; set; }

		public string Protocol
		{
			get { return "LinkedIn"; }
		}

		/// <summary>
		/// See: http://developer.linkedin.com/docs/DOC-1004
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ChannelContact> GetContacts()
		{
			var result = LinkedInWebRequest.PerformRequest(new Uri("http://api.linkedin.com/v1/people/~/connections"),
				ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, ChannelHelper.Token, ChannelHelper.TokenSecret);

			if (result.Element("connections").Elements().Count() == 0)
				yield break;

			foreach (var response in result.Elements("connections").Elements("person"))
			{
				ChannelContact contact = new ChannelContact();
				contact.Profile.ChannelProfileKey = response.Element("id").Value;
				contact.Person.Firstname = response.Element("first-name").Value;
				contact.Person.Lastname = response.Element("last-name").Value;

				contact.Profile.Title = response.Element("headline").Value;
				contact.Profile.Url = response.Element("site-standard-profile-request").Element("url").Value;

				if (response.Element("picture-url") != null)
				{
					contact.Profile.ChannelAvatar = new ChannelAvatar();
					contact.Profile.ChannelAvatar.Url = response.Element("picture-url").Value;
				}

				contact.Profile.SourceAddress = new SourceAddress(response.Element("id").Value, contact.Person.Name);
				contact.Profile.ProfileType = ProfileType.Social;

				yield return contact;
			}
		}

		IClientContactsChannel IClientContactsChannel.Clone()
		{
			return new LinkedInClientChannel
				{
					Hostname = Hostname,
					Port = Port,
					IsSecured = IsSecured,
					MaxConcurrentConnections = MaxConcurrentConnections,
					CredentialsProvider = CredentialsProvider
				};
		}

		public ChannelSocialProfile GetProfile()
		{
			var result = LinkedInWebRequest.PerformRequest(new Uri("http://api.linkedin.com/v1/people/~:(id,first-name,last-name,picture-url)"),
				ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, ChannelHelper.Token, ChannelHelper.TokenSecret);

			var response = result.Element("person");

			return new ChannelSocialProfile
			{
				Id = response.Element("id").Value,
				FullName = String.Concat(response.Element("first-name").Value, " ", response.Element("last-name").Value),
				AvatarUrl = response.Element("picture-url") == null ? String.Empty : response.Element("picture-url").Value
			};
		}

		public IEnumerable<ChannelStatusUpdate> GetMentions(int pageSize)
		{
			yield break;
		}

		public IEnumerable<ChannelStatusUpdate> GetUpdates(int pageSize)
		{
			var result = LinkedInWebRequest.PerformRequest(new Uri("http://api.linkedin.com/v1/people/~/network"), ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, ChannelHelper.Token, ChannelHelper.TokenSecret);

			if (result.Element("network").Elements().Count() == 0)
				yield break;

			foreach (var response in result.Element("network").Element("updates").Elements("update"))
			{
				ChannelStatusUpdate statusUpdate = null;
				var updateType = (LinkedInUpdateType)Enum.Parse(typeof(LinkedInUpdateType),
					response.Element("update-type").Value);

				switch (updateType)
				{
					case LinkedInUpdateType.ANSW:
						// TODO : Answer Update
						break;
					case LinkedInUpdateType.APPS:
					case LinkedInUpdateType.APPM:
						// TODO : Application Update
						break;
					case LinkedInUpdateType.CONN:
					case LinkedInUpdateType.NCON:
					case LinkedInUpdateType.CCEM:
						// Connection updates
						statusUpdate = new ChannelStatusUpdate
						{
							ChannelStatusKey = BuildChannelStatusKey(response),
							From = BuildSourceAddress(response),
							Status = BuildConnectionStatusMessage(response),
							DatePosted = GetDateTime(response)
						};
						break;
					case LinkedInUpdateType.JOBS:
					case LinkedInUpdateType.JOBP:
						// TODO : Posted a job
						break;
					case LinkedInUpdateType.JGRP:
						// TODO : Joined a group
						break;
					case LinkedInUpdateType.PICT:
					case LinkedInUpdateType.PICU:
						// TODO : Changed a picture
						break;
					case LinkedInUpdateType.RECU:
					case LinkedInUpdateType.PREC:
						// TODO : Recommendations
						break;
					case LinkedInUpdateType.PRFU:
					case LinkedInUpdateType.PROF:
						// TODO : Changed profile
						break;
					case LinkedInUpdateType.QSTN:
						// TODO : Question update
						break;
					case LinkedInUpdateType.STAT:
						// Status update
						statusUpdate = new ChannelStatusUpdate
						{
							ChannelStatusKey = BuildChannelStatusKey(response),
							From = BuildSourceAddress(response),
							Status = response.Element("update-content").Element("person").Element("current-status").Value,
							DatePosted = GetDateTime(response)
						};
						break;
				}

				if (statusUpdate != null)
					yield return statusUpdate;
			}
		}

		private DateTime GetDateTime(XElement response)
		{
			// LinkedIn returns us Milliseconds instead of seconds.
			// We first need to convert this to seconds.
			return ((long)TimeSpan.FromMilliseconds(Int64.Parse(response.Element("timestamp").Value)).TotalSeconds).ToUnixTime();
		}

		private string BuildChannelStatusKey(XElement response)
		{
			return string.Format(
				"{0}-{1}",
				response.Element("timestamp").Value,
				response.Element("update-content").Element("person").Element("id").Value
				);
		}

		private SourceAddress BuildSourceAddress(XElement response)
		{
			if (response.Element("update-content").Element("person").Element("picture-url") != null)
			{
				// User has avatar
				return new SourceAddress(
					response.Element("update-content").Element("person").Element("id").Value,
					string.Format("{0} {1}",
						response.Element("update-content").Element("person").Element("first-name").Value,
						response.Element("update-content").Element("person").Element("last-name").Value),
					response.Element("update-content").Element("person").Element("picture-url").Value,
					response.Element("update-content").Element("person").Element("site-standard-profile-request").Element("url").Value);
			}
			else
			{
				// No Avatar
				var address = new SourceAddress(
					response.Element("update-content").Element("person").Element("id").Value,
					string.Format("{0} {1}",
						response.Element("update-content").Element("person").Element("first-name").Value,
						response.Element("update-content").Element("person").Element("last-name").Value));

				address.ProfileUrl =
					response.Element("update-content").Element("person").Element("site-standard-profile-request").Element("url").Value;

				return address;
			}
		}

		private string BuildConnectionStatusMessage(XElement response)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("is now connected to ");

			var persons = response.Element("update-content").Element("person").Element("connections").Elements("person").ToArray();
			var connectedTo = new string[persons.Count()];
			for (int i = 0; i < persons.Count(); i++)
			{
				connectedTo[i] = String.Format("{0} {1}", persons[i].Element("first-name").Value, persons[i].Element("last-name").Value);
			}
			stringBuilder.Append(String.Join(", ", connectedTo));

			return stringBuilder.ToString();
		}

		public IEnumerable<ChannelStatusUpdate> GetUserUpdates(string username, int pageSize)
		{
			var result = LinkedInWebRequest.PerformRequest(new Uri(String.Concat("http://api.linkedin.com/v1/people/id=", username, ":(current-status,current-status-timestamp)")),
				ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, ChannelHelper.Token, ChannelHelper.TokenSecret);

			if (result.Elements("person").Elements().Count() == 0)
				yield break;

			foreach (var response in result.Elements("person"))
			{
				var update = new ChannelStatusUpdate();

				update.Status = response.Element("current-status").Value;

				var ts = TimeSpan.FromMilliseconds(Double.Parse(response.Element("current-status-timestamp").Value));
				update.DatePosted = ((long) ts.TotalSeconds).ToUnixTime();

				yield return update;
			}
		}

		public IEnumerable<ChannelStatusUpdate> GetUpdates(string keyword, int pageSize)
		{
			yield break;
		}

		public void UpdateMyStatus(ChannelStatusUpdate update)
		{
			var element = new XElement("current-status", update.Status.StripHtml());

			LinkedInWebRequest.Put(new Uri("http://api.linkedin.com/v1/people/~/current-status"), element.ToString(),
				ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, ChannelHelper.Token, ChannelHelper.TokenSecret);
		}

		IClientStatusUpdatesChannel IClientStatusUpdatesChannel.Clone()
		{
			return new LinkedInClientChannel
				{
					Hostname = Hostname,
					Port = Port,
					IsSecured = IsSecured,
					MaxConcurrentConnections = MaxConcurrentConnections,
					CredentialsProvider = CredentialsProvider
				};
		}

		public string SourceAdress
		{
			get { return CredentialsProvider.GetCredentials().Claim; }
		}

		public string AuthMessage { get; private set; }

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

		public bool Disconnect()
		{
			return true;
		}

		public IClientInputChannel Clone()
		{
			LinkedInClientChannel channel = new LinkedInClientChannel();
			channel.Hostname = Hostname;
			channel.Port = Port;
			channel.IsSecured = IsSecured;
			channel.MaxConcurrentConnections = MaxConcurrentConnections;
			channel.CredentialsProvider = CredentialsProvider;

			return channel;
		}

		public void Dispose()
		{

		}
	}
}
