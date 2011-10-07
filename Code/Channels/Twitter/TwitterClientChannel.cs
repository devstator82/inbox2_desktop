using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Framework.OAuth;
using TweetSharp;

namespace Inbox2.Channels.Twitter
{
	public class TwitterClientChannel : IClientInputChannel, IClientOutputChannel, IClientContactsChannel, IClientStatusUpdatesChannel
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
			get { return "Twitter API"; }
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
			return GetDirectMessages();
		}

		public IEnumerable<ChannelMessage> GetMessage(ChannelMessageHeader header)
		{
			ChannelMessage message = new ChannelMessage();
			message.MessageNumber = header.MessageNumber;
			message.MessageIdentifier = header.MessageIdentifier;
			message.From = header.From;
			message.Context = header.Context;
			message.BodyText = header.Body.ToStream();			
			message.DateReceived = header.DateReceived;
			message.IsRead = true;

			yield return message;
		}		

		public IEnumerable<ChannelMessageHeader> GetDirectMessages()
		{
			var service = new TwitterService(ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, ChannelHelper.Token, ChannelHelper.TokenSecret);
			var result = service.ListDirectMessagesReceived().Union(service.ListDirectMessagesSent());

			foreach (var dm in result)
			{
				yield return new ChannelMessageHeader
					{
						MessageIdentifier = dm.Id.ToString(),
						MessageNumber = dm.Id.ToString(),
						Context = String.Format("DM from {0}", dm.Sender.Name),
						From = new SourceAddress(dm.Sender.Id.ToString(), dm.Sender.Name),
						Body = dm.TextAsHtml,
						DateReceived = dm.CreatedDate.ToLocalTime()
					};
				
			}
		}

		public ChannelSocialProfile GetProfile()
		{
			var service = new TwitterService(ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, ChannelHelper.Token, ChannelHelper.TokenSecret);
			var result = service.GetUserProfile();

			return new ChannelSocialProfile
			    {
					Id = result.Id.ToString(),
					FullName = result.Name,
					AvatarUrl = result.ProfileImageUrl
			    };
		}

		public IEnumerable<ChannelContact> GetContacts()
		{
			var service = new TwitterService(ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, ChannelHelper.Token, ChannelHelper.TokenSecret);
			var result = service.ListFriends();

			foreach (var user in result)
			{
				var contact = new ChannelContact { IsSoft = true };

				contact.Profile.ChannelProfileKey = user.Id.ToString();
				contact.Person.Name = user.Name;
				contact.Profile.ScreenName = user.ScreenName;
				contact.Profile.SourceAddress = new SourceAddress(user.Id.ToString(), user.Name);
				contact.Profile.ProfileType = ProfileType.Social;
				contact.Profile.Url = user.Url;
				contact.Profile.Title = user.Description;
				contact.Profile.Location = user.Location;
				contact.Profile.ChannelAvatar = new ChannelAvatar();
				contact.Profile.ChannelAvatar.Url = user.ProfileImageUrl;

				yield return contact;
			}
		}

		IClientContactsChannel IClientContactsChannel.Clone()
		{
			return new TwitterClientChannel
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
			var service = new TwitterService(ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, ChannelHelper.Token, ChannelHelper.TokenSecret);

			foreach (var singleToName in message.To)
			{
				service.SendDirectMessage(Int32.Parse(singleToName.Address), message.BodyText.ReadString());
			}
		}				

		public IEnumerable<ChannelStatusUpdate> GetMentions(int pageSize)
		{
			var service = new TwitterService(ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, ChannelHelper.Token, ChannelHelper.TokenSecret);
			var result = service.ListTweetsMentioningMe();

			foreach (var tweet in result)
			{
				yield return new ChannelStatusUpdate
				    {
						ChannelStatusKey = tweet.Id.ToString(),
						Status = tweet.Text,
						From = new SourceAddress(
							tweet.User.ScreenName, 
							tweet.User.Name, 
							tweet.User.ProfileImageUrl.Replace("_normal.jpg", "_mini.jpg")),
						DatePosted = tweet.CreatedDate
				    };
			}
		}

		public IEnumerable<ChannelStatusUpdate> GetUpdates(int pageSize)
		{
			var service = new TwitterService(ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, ChannelHelper.Token, ChannelHelper.TokenSecret);
			var result = service.ListTweetsOnFriendsTimeline();

			foreach (var tweet in result)
			{
				yield return new ChannelStatusUpdate
				{
					ChannelStatusKey = tweet.Id.ToString(),
					Status = tweet.Text,
					From = new SourceAddress(
						tweet.User.ScreenName,
						tweet.User.Name,
						tweet.User.ProfileImageUrl.Replace("_normal.jpg", "_mini.jpg")),
					DatePosted = tweet.CreatedDate
				};
			}
		}

		public IEnumerable<ChannelStatusUpdate> GetUserUpdates(string username, int count)
		{
			var service = new TwitterService(ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, ChannelHelper.Token, ChannelHelper.TokenSecret);
			var result = service.ListTweetsOnSpecifiedUserTimeline(Int32.Parse(username));

			foreach (var tweet in result)
			{
				yield return new ChannelStatusUpdate
				{
					ChannelStatusKey = tweet.Id.ToString(),
					Status = tweet.Text,
					From = new SourceAddress(
						tweet.User.ScreenName,
						tweet.User.Name,
						tweet.User.ProfileImageUrl.Replace("_normal.jpg", "_mini.jpg")),
					DatePosted = tweet.CreatedDate
				};
			}			
		}

		public IEnumerable<ChannelStatusUpdate> GetUpdates(string keyword, int pageSize)
		{
			var service = new TwitterService(ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, ChannelHelper.Token, ChannelHelper.TokenSecret);
			var result = service.Search(keyword, pageSize);

			foreach (var tweet in result.Statuses)
			{
				yield return new ChannelStatusUpdate
				    {
						ChannelStatusKey = tweet.Id.ToString(),
						Status = tweet.Text,
						From = new SourceAddress(
							tweet.Author.ScreenName,
							tweet.Author.ScreenName,
							tweet.Author.ProfileImageUrl.Replace("_normal.jpg", "_mini.jpg")),
						DatePosted = tweet.CreatedDate
				    };
			}
		}

		public void UpdateMyStatus(ChannelStatusUpdate update)
		{
			var service = new TwitterService(ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, ChannelHelper.Token, ChannelHelper.TokenSecret);

			if (!String.IsNullOrEmpty(update.InReplyTo))
				service.SendTweet(update.Status, Int64.Parse(update.InReplyTo));
			else
				service.SendTweet(update.Status);
		}

		IClientStatusUpdatesChannel IClientStatusUpdatesChannel.Clone()
		{
			return new TwitterClientChannel
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
			TwitterClientChannel channel = new TwitterClientChannel();
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
