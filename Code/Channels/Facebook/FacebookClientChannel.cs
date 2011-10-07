using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Inbox2.Channels.Facebook.REST;
using Inbox2.Channels.Facebook.REST.DataContracts;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Channels.Web;

namespace Inbox2.Channels.Facebook
{
	public class FacebookClientChannel : IClientInputChannel, IClientOutputChannel, IClientContactsChannel, IClientStatusUpdatesChannel
	{
		#region Fields
		
		protected const string MessageComposePageMobile = "http://m.facebook.com/inbox/?rfb157ae2=&compose=&ids={0}&refid=0";
		protected FacebookRESTClient client;
		
		#endregion

		#region Properties

		public string Hostname
		{
			get { return "http://api.facebook.com/restserver.php"; }
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

		public bool IsEnabled
		{
			get; set;
		}

		public int MaxConcurrentConnections
		{
			get; set;
		}

		public IChannelCredentialsProvider CredentialsProvider
		{
			get; set;
		}

		public string Protocol
		{
			get { return "Facebook API"; }
		}

		public string SourceAdress
		{
			get { return CredentialsProvider.GetCredentials().Claim; }
		}

		public string AuthMessage { get; private set; }

		#endregion

		public FacebookClientChannel()
		{			
		}

		public ConnectResult Connect()
		{
			BuildRestClient();

			FbAuth result = client.Authenticate();

			if (result == FbAuth.Success)
				return ConnectResult.Success;

			return ConnectResult.AuthFailure;
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
			BuildRestClient();

			var messages = client.GetMessages(FbMessageFolder.Inbox).ToList();

			foreach (FbMessage fbMessage in messages)
			{
				var header = new ChannelMessageHeader();

				header.MessageIdentifier = fbMessage.MessageId;
				header.MessageNumber = fbMessage.MessageId;
				header.Context = fbMessage.Subject;
				header.From = fbMessage.From;
				header.To = fbMessage.To;
				header.Body = fbMessage.Body;
				header.IsRead = fbMessage.Read;
				header.DateReceived = fbMessage.DateCreated;
				header.Metadata.i2mpRelationId = fbMessage.ThreadId;
			    					
				yield return header;
			}
		}

		public IEnumerable<ChannelMessage> GetMessage(ChannelMessageHeader header)
		{
			ChannelMessage message = new ChannelMessage();
			message.MessageNumber = header.MessageNumber;
			message.MessageIdentifier = header.MessageIdentifier;
			message.From = header.From;
			message.Context = header.Context;
			message.BodyText = header.Body.ToStream();
			message.IsRead = header.IsRead;
			message.DateReceived = header.DateReceived;
			message.ConversationId = header.Metadata.i2mpRelationId;

			yield return message;
		}

		public IEnumerable<ChannelContact> GetContacts()
		{
			BuildRestClient();

		    int count = 1;
            var waitTime = new TimeSpan(0, 0, 1);

            foreach (FbContact fbContact in client.GetContacts())
			{
				ChannelContact contact = ParseFbContact(fbContact);

				if (count % 10 == 0)
					Thread.Sleep(waitTime);

			    count++;
				yield return contact;
			}
		}

		IClientContactsChannel IClientContactsChannel.Clone()
		{
			return new FacebookClientChannel
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
		}

		public ChannelSocialProfile GetProfile()
		{
			BuildRestClient();
			var user = client.GetLoggedInUser();

			return new ChannelSocialProfile
		       	{
		       		Id = user.UserId,
		       		FullName = user.Name,
		       		AvatarUrl = user.AvatarSquareUrl
		       	};
		}

		public IEnumerable<ChannelStatusUpdate> GetMentions(int pageSize)
		{
			yield break;
		}

		public IEnumerable<ChannelStatusUpdate> GetUpdates(int pageSize)
		{
			BuildRestClient();

			foreach (var fbStatus in client.GetStatusses(pageSize))
			{
				var status = ParseFbStatus(fbStatus);

				status.Children.AddRange(fbStatus.Comments.Select(ParseFbStatus));

				yield return status;
			}
		}

		public IEnumerable<ChannelStatusUpdate> GetUserUpdates(string username, int pageSize)
		{
			BuildRestClient();

			// Always select 50 from fb and return the requested nr because some updates from
			// Facebook might be filtered out due to actorid being different from sourceid.
			return client.GetStatusses(username, 50).Select(ParseFbStatus).Take(pageSize);
		}

		public IEnumerable<ChannelStatusUpdate> GetUpdates(string keyword, int pageSize)
		{
			BuildRestClient();

			yield break;
		}

		public void UpdateMyStatus(ChannelStatusUpdate update)
		{
			BuildRestClient();

			if (String.IsNullOrEmpty(update.InReplyTo))
				client.SetStatus(update.Status);
			else
				client.PostComment(update.Status, update.InReplyTo);
		}

		IClientStatusUpdatesChannel IClientStatusUpdatesChannel.Clone()
		{
			return new FacebookClientChannel
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
            if (client != null)
            {
                client.Dispose();
                client = null;
            }

		    return true;
		}

		public IClientInputChannel Clone()
		{
			FacebookClientChannel channel = new FacebookClientChannel();
			channel.Hostname = Hostname;
			channel.Port = Port;
			channel.IsSecured = IsSecured;
			channel.MaxConcurrentConnections = MaxConcurrentConnections;
			channel.CredentialsProvider = CredentialsProvider;

			return channel;
		}

		void BuildRestClient()
		{
			if (client == null)
			{
				var sessionKey = ChannelContext.Current.ClientContext.GetSetting("/Channels/Facebook/SessionKey").ToString();
				var sessionSecret = ChannelContext.Current.ClientContext.GetSetting("/Channels/Facebook/SessionSecret").ToString();

				var apiKey = FacebookApiKeys.GetApiKey();
				var apiSecret = FacebookApiKeys.GetApiSecret();

				if ("/Settings/Channels/Codebase".AsKey("cloud") == "client")
				{
					client = new FacebookRESTClient(apiKey, sessionSecret, sessionKey, sessionSecret);	
				}
				else
				{
					client = new FacebookRESTClient(apiKey, apiSecret, sessionKey, sessionSecret);	
				}				
			}
		}

		public void Dispose()
		{
			Disconnect();
		}

		ChannelContact ParseFbContact(FbContact fbContact)
		{
			ChannelContact contact = new ChannelContact();

			contact.Person.Firstname = fbContact.Firstname;
			contact.Person.Lastname = fbContact.Lastname;

			contact.Profile.ChannelProfileKey = fbContact.UserId;
			contact.Profile.ProfileType = ProfileType.Social;
			contact.Profile.SourceAddress = new SourceAddress(fbContact.UserId, contact.Person.Name);

			if (!String.IsNullOrEmpty(fbContact.AvatarSquareUrl.Trim()))
			{
				ChannelAvatar avatar = new ChannelAvatar();
				avatar.Url = fbContact.AvatarSquareUrl;
				avatar.ContentStream = WebContentStreamHelper.GetContentStream(avatar.Url);

				contact.Profile.ChannelAvatar = avatar;
			}

			return contact;
		}

		ChannelStatusUpdate ParseFbStatus(FbStatus fbStatus)
		{
			var status = new ChannelStatusUpdate();

			status.ChannelStatusKey = fbStatus.StatusId;
			status.From = fbStatus.From;
			status.To = fbStatus.To;
			status.Status = fbStatus.Message;
			status.DatePosted = fbStatus.DateCreated;

			foreach (var fbAttachment in fbStatus.Attachments)
			{
				var attachment = new ChannelStatusUpdateAttachment();

				attachment.MediaType = (short)fbAttachment.MediaType;
				attachment.PreviewAltText = fbAttachment.PreviewAltText;
				attachment.PreviewImageUrl = fbAttachment.PreviewImageUrl;
				attachment.TargetUrl = fbAttachment.TargetUrl;

				status.Attachments.Add(attachment);
			}

			return status;
		}
	}
}
