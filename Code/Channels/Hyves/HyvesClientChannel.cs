using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Channels.Web;

namespace Inbox2.Channels.Hyves
{
	public class HyvesClientChannel : IClientInputChannel, IClientOutputChannel, IClientContactsChannel
	{
		private string authToken;
		private string authSecret;

		#region Properties

		public IChannelCredentialsProvider CredentialsProvider { get; set; }

		public string Hostname
		{
			get { return "http://data.hyves-api.nl/"; }
			set { }
		}

		public int Port
		{
			get { return 80; }
			set { }
		}

		public bool IsSecured
		{
			get { return true; }
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
			get { return "Hyves API"; }
		}

		public string SourceAdress
		{
			get { return CredentialsProvider.GetCredentials().Claim; }
		}

		public string AuthMessage { get; private set; }

		#endregion

		public ConnectResult Connect()
		{
			authToken = ChannelContext.Current.ClientContext.GetSetting("/Channels/Hyves/AuthToken").ToString();
			authSecret = ChannelContext.Current.ClientContext.GetSetting("/Channels/Hyves/AuthSecret").ToString(); 

			if (!String.IsNullOrEmpty(authToken) && !String.IsNullOrEmpty(authSecret))
				return ConnectResult.Success;

			return CredentialsProvider.ValidateCredentials();
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
			var messages =
				PerformRequest("http://data.hyves-api.nl/?ha_method=messages.getInbox&messagetype=member_to_member&ha_version=experimental&ha_format=xml&ha_fancylayout=true&ha_resultsperpage=50");

			foreach (var message in messages.Elements("message"))
			{
				yield return ParseHeader(message);
			}
		}

		public IEnumerable<ChannelMessage> GetMessage(ChannelMessageHeader header)
		{
			ChannelMessage message = new ChannelMessage();
			message.MessageNumber = header.MessageNumber;
			message.MessageIdentifier = header.MessageIdentifier;
			message.Context = header.Context;
			message.From = header.From;
			message.To.AddRange(header.To);
			message.InReplyTo = header.InReplyTo;
			message.BodyHtml = header.Body.ToStream();
			message.DateReceived = header.DateReceived;

			yield return message;
		}

		public IEnumerable<ChannelContact> GetContacts()
		{
			var friends =
				PerformRequest("http://data.hyves-api.nl/?ha_method=friends.get&ha_version=experimental&ha_format=xml&ha_fancylayout=true");

			List<string> friendsList = friends.Elements("userid").Select(e => e.Value).ToList();

			// Retrieve friends per 10 at a time
			for (int i = 0; i < friendsList.Count; i += 10)
			{
				// Creates lists like id1,id2,id3, etc
				var ids = String.Join(",", friendsList.Skip(i).Take(10).ToArray());

				var friend =
					PerformRequest(
						String.Format("http://data.hyves-api.nl/?userid={0}&ha_responsefields=profilepicture,cityname,countryname&ha_method=users.get&ha_version=experimental&ha_format=xml&ha_fancylayout=true", ids));

				foreach (var element in friend.Elements("user"))
				{
					yield return ParseContact(element);
				}				
			}
		}

		IClientContactsChannel IClientContactsChannel.Clone()
		{
			return new HyvesClientChannel
				{
					Hostname = Hostname,
					Port = Port,
					IsSecured = IsSecured,
					MaxConcurrentConnections = MaxConcurrentConnections,
					CredentialsProvider = CredentialsProvider
				};
		}

		public void Send(ChannelMessage message)
		{
			foreach (var rcpt in message.To.Union(message.CC).Union(message.BCC))
			{
				PerformRequest(
					String.Format("http://data.hyves-api.nl/?title={0}&body={1}&target_userid={2}&ha_method=messages.send&ha_version=experimental&ha_format=xml&ha_fancylayout=true",
						message.Context,
						message.BodyHtml.ReadString().StripHtml(),
						rcpt.Address));
			}
		}

		public bool Disconnect()
		{
			return true;
		}

		public IClientInputChannel Clone()
		{
			HyvesClientChannel channel = new HyvesClientChannel();
			channel.Hostname = Hostname;
			channel.Port = Port;
			channel.IsSecured = IsSecured;
			channel.MaxConcurrentConnections = MaxConcurrentConnections;
			channel.CredentialsProvider = CredentialsProvider;

			return channel;
		}

		ChannelContact ParseContact(XElement source)
		{
			ChannelContact contact = new ChannelContact();

			contact.Person.Firstname = source.Element("firstname").Value;
			contact.Person.Lastname = source.Element("lastname").Value;
			contact.Person.Gender = source.Element("gender").Value;

			contact.Profile.ScreenName = contact.Person.Name;
			contact.Profile.ChannelProfileKey = source.Element("userid").Value;
			contact.Profile.ProfileType = ProfileType.Social;
			contact.Profile.Url = source.Element("url").Value;
			contact.Profile.SourceAddress = 
				new SourceAddress(source.Element("userid").Value, contact.Person.Name);

			if (source.Element("cityname") != null)			
				contact.Profile.City = source.Element("cityname").Value;
			
			if (source.Element("countryname") != null)			
				contact.Profile.Country = source.Element("countryname").Value;

			if (source.Element("countryname") != null)
				contact.Profile.Country = source.Element("countryname").Value;

			if (source.Element("birthday") != null &&
				!(String.IsNullOrEmpty(source.Element("birthday").Element("year").Value)) &&
				!(String.IsNullOrEmpty(source.Element("birthday").Element("month").Value)) &&
				!(String.IsNullOrEmpty(source.Element("birthday").Element("day").Value)))
			{
				contact.Person.DateOfBirth = new DateTime(
						Int32.Parse(source.Element("birthday").Element("year").Value),
						Int32.Parse(source.Element("birthday").Element("month").Value),
						Int32.Parse(source.Element("birthday").Element("day").Value)
					);
			}

			if (source.Element("profilepicture") != null)
			{
				string url = source.Element("profilepicture").Element("icon_extralarge").Element("src").Value;

				ChannelAvatar avatar = new ChannelAvatar();
				avatar.Url = url;
				avatar.ContentStream = WebContentStreamHelper.GetContentStream(avatar.Url);

				contact.Profile.ChannelAvatar = avatar;
			}

			return contact;
		}

		ChannelMessageHeader ParseHeader(XElement source)
		{
			ChannelMessageHeader header = new ChannelMessageHeader();
			header.Context = source.Element("title").Value;
			header.MessageNumber = source.Element("messageid").Value;
			header.MessageIdentifier = source.Element("messageid").Value;

			string sourceid = source.Element("userid").Value;
			string targetid = source.Element("target_userid").Value;

			var recipients =
				PerformRequest(
					String.Format("http://data.hyves-api.nl/?userid={0}&ha_responsefields=profilepicture,cityname,countryname&ha_method=users.get&ha_version=experimental&ha_format=xml&ha_fancylayout=true", String.Join(",", new[] { sourceid, targetid})));

			var from = ParseContact(recipients.Elements("user").First(u => u.Element("userid").Value == sourceid));
			var to = ParseContact(recipients.Elements("user").First(u => u.Element("userid").Value == targetid));

			header.From = new SourceAddress(from.Profile.ChannelProfileKey, from.Profile.ScreenName);
			header.To.Add(new SourceAddress(to.Profile.ChannelProfileKey, to.Profile.ScreenName));

			string bodyType = source.Element("format").Value;

			if (bodyType == "html")
			{
				// For some weird reason, hyves sometimes sends htmlBody and sometimes htmlbody (lowercase b)
				var elem = source.Element("htmlBody") ?? source.Element("htmlbody");

				header.Body = elem.Value;
			}				
			else
				header.Body = source.Element("body").Value;

			header.DateReceived = Int64.Parse(source.Element("created").Value).ToUnixTime();

			return header;
		}

		internal static XElement PerformRequest(string requestUri)
		{
			var authorizedToken = ChannelContext.Current.ClientContext.GetSetting("/Channels/Hyves/AuthToken").ToString();
			var authorizedSecret = ChannelContext.Current.ClientContext.GetSetting("/Channels/Hyves/AuthSecret").ToString();

			return HyvesApiRequest.PerformRequest(new Uri(requestUri), authorizedToken, authorizedSecret);	
		}

		public void Dispose()
		{
			
		}
	}
}
