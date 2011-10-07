using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using ExchangeServicesWsdlClient;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Logging;
using Logger = Inbox2.Platform.Logging.Logger;

namespace Inbox2.Channels.Exchange
{
	public class ExchangeClientChannel : IClientInputChannel, IClientOutputChannel, IPagableChannel, IReadStateChannel, IClientContactsChannel, ILabelsChannel
    {
		private ChannelFolder folder;
		private List<ChannelFolder> folders;

        #region Properties

		public long StartIndex { get; set; }

		public long EndIndex { get; set; }

		public long PageSize { get; set; }

		public ChannelProgressDelegate BytesRead { get; set; }

		public ChannelProgressDelegate BytesWritten { get; set; }
  
        public string Hostname { get; set; }

        public int Port { get; set; }

        public bool IsSecured { get; set; }

        public string Username
        {
            get { return CredentialsProvider.GetCredentials().Claim; }
        }

        public string Password
        {
            get { return CredentialsProvider.GetCredentials().Evidence; }
        }

        public bool IsEnabled { get; set; }

        public int MaxConcurrentConnections { get; set; }

        public IChannelCredentialsProvider CredentialsProvider { get; set; }

        public string Protocol
        {
            get { return "ExchangeWS"; }
        }

        public string SourceAdress
        {
            get
            {
                string username = CredentialsProvider.GetCredentials().Claim;

                return username.Contains("@") ? username : String.Format("{0}@{1}", username, Hostname);
            }
        }

		public LabelsSupport LabelsSupport
		{
			get { return LabelsSupport.Folders; }
		}

		public string AuthMessage { get; private set; }

        #endregion    	

		public ConnectResult Connect()
        {
			var credentials = CredentialsProvider.GetCredentials();
			var binding = ChannelHelper.BuildChannel(Hostname, credentials.Claim, credentials.Evidence);

			folders = new List<ChannelFolder>();

			// Try connecting 
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(binding.Url);
			request.AllowAutoRedirect = false;
			request.Credentials = new NetworkCredential(credentials.Claim, credentials.Evidence);

			try
			{
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();

				Logger.Debug("Server {0} returned status-code {1}", LogSource.Channel, binding.Url, response.StatusCode);

				if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Found)
					return ConnectResult.Success;

				AuthMessage = String.Format("Statuscode {0}", response.StatusCode);

				return ConnectResult.AuthFailure;
			}
			catch (Exception ex)
			{
				AuthMessage = ex.Message;

				return ConnectResult.AuthFailure;
			}
        }

        public IEnumerable<ChannelFolder> GetFolders()
        {
			var client = new ExchangeClient(Hostname, CredentialsProvider);

			// inbox
        	var inbox = client.GetFolder(DistinguishedFolderIdNameType.inbox);
			folders.Add(new ChannelFolder(inbox.FolderId.Id, inbox.DisplayName, ChannelFolderType.Inbox));
			folders.AddRange(GetChildFolders(client, inbox.FolderId.Id, ChannelFolderType.Label));

			// sent items
			var sent = client.GetFolder(DistinguishedFolderIdNameType.sentitems);
        	folders.Add(new ChannelFolder(sent.FolderId.Id, sent.DisplayName, ChannelFolderType.SentItems));
			folders.AddRange(GetChildFolders(client, sent.FolderId.Id, ChannelFolderType.SentItems));

			// trash
			var trash = client.GetFolder(DistinguishedFolderIdNameType.deleteditems);
			folders.Add(new ChannelFolder(trash.FolderId.Id, trash.DisplayName, ChannelFolderType.Trash));

			// spam
			var spam = client.GetFolder(DistinguishedFolderIdNameType.junkemail);
			folders.Add(new ChannelFolder(spam.FolderId.Id, spam.DisplayName, ChannelFolderType.Spam));

        	return folders;
        }

		IEnumerable<ChannelFolder> GetChildFolders(ExchangeClient client, string folderId, ChannelFolderType folderType)
		{
			var childFolders = client.FindAllFolders(folderId);

			return childFolders.Select(f => new ChannelFolder(f.FolderId.Id, f.DisplayName, folderType));
		}

		public void SelectFolder(ChannelFolder folder)
        {
        	this.folder = folder;
        }

        public IEnumerable<ChannelMessageHeader> GetHeaders()
        {
        	var client = new ExchangeClient(Hostname, CredentialsProvider);

			foreach (MessageType inboxItem in client.GetHeaders(folder).Reverse().Skip((int)StartIndex).Take((int)PageSize))
            {
				var header = new ChannelMessageHeader
	             	{	             		
	             		MessageNumber = inboxItem.ItemId.Id,
	             		MessageIdentifier = inboxItem.InternetMessageId,
	             		Context = inboxItem.Subject,
						// Not sending size because exchange changes can the size due
						// to user actions usch as change priority.
	             		//Size = inboxItem.Size,
	             		DateReceived = inboxItem.DateTimeReceived,
						IsRead = inboxItem.IsRead,
						// Not processing IsStarred because somebody could send you an 
						// email with importance: high and would end up getting starred;
						// feature just asking for abuse :-)
						//IsStarred = inboxItem.Importance == ImportanceChoicesType.High,
	             	};

            	yield return header;                                     
            }            
        }
		
        public IEnumerable<ChannelMessage> GetMessage(ChannelMessageHeader header)
        {
			var client = new ExchangeClient(Hostname, CredentialsProvider);

			ChannelMessage message = new ChannelMessage();
			MessageType inboxItem = client.GetMessage(header.MessageNumber);

			// Now the message Body is there.
			BodyType messageBody = inboxItem.Body;

			message.Size = header.Size;
			message.MessageNumber = header.MessageNumber;
			message.MessageIdentifier = header.MessageIdentifier;
			message.Context = header.Context;
			message.From = new SourceAddress(inboxItem.From.Item.EmailAddress, inboxItem.From.Item.Name);
			
			if (inboxItem.ToRecipients != null)
				foreach (var toRecipient in inboxItem.ToRecipients)
					message.To.Add(new SourceAddress(toRecipient.EmailAddress, toRecipient.Name));

			if (inboxItem.CcRecipients != null)
				foreach (var ccRecipient in inboxItem.CcRecipients)
					message.CC.Add(new SourceAddress(ccRecipient.EmailAddress, ccRecipient.Name));

			if (inboxItem.BccRecipients != null)
				foreach (var bccRecipient in inboxItem.BccRecipients)
					message.BCC.Add(new SourceAddress(bccRecipient.EmailAddress, bccRecipient.Name));

			message.InReplyTo = inboxItem.InReplyTo;
			message.Metadata = header.Metadata;
			message.IsRead = inboxItem.IsRead;
			message.BodyHtml = messageBody.Value.ToStream();
			message.DateReceived = header.DateReceived;

			if (inboxItem.Attachments != null)
			{
				foreach (AttachmentType exchAttachment in inboxItem.Attachments)
				{
					var fileAttachment = client.GetAttachment(exchAttachment.AttachmentId.Id);

					message.Attachments.Add(new ChannelAttachment
	                 	{
	                 		Filename = fileAttachment.Name,
	                 		ContentType = ContentType.Attachment,
	                 		ContentStream = new MemoryStream(fileAttachment.Content)
	                 	});
				}
			}                    
            
            yield return message;
        }

		public void Send(ChannelMessage message)
		{
			var client = new ExchangeClient(Hostname, CredentialsProvider);

			var messageId = client.SaveMessage(message);

			// Upload attachments
			if (message.Attachments.Count > 0)
				client.SaveAttachments(messageId, message);

			// Refresh and send message by message-id
			client.SendMessage(client.GetMessageId(messageId.Id).ItemId);
		}
		
        public bool Disconnect()
        {
            return true;
        }

        public IClientInputChannel Clone()
        {
            return new ExchangeClientChannel
		    	{
		    		Hostname = Hostname,
		    		Port = Port,
		    		IsSecured = IsSecured,
		    		MaxConcurrentConnections = MaxConcurrentConnections,
		    		CredentialsProvider = CredentialsProvider
		    	};
        }

        public void Dispose()
        {            
        }

		public long GetNumberOfItems()
		{
			var client = new ExchangeClient(Hostname, CredentialsProvider);

			return client.GetNrItemsInFolder(folder);
		}

		public void MarkRead(ChannelMessageHeader message)
		{
			var client = new ExchangeClient(Hostname, CredentialsProvider);
			var messageId = client.GetMessageId(message.MessageNumber).ItemId;

			client.SetMessageReadState(messageId, true);
		}

		public void MarkUnread(ChannelMessageHeader message)
		{
			var client = new ExchangeClient(Hostname, CredentialsProvider);
			var messageId = client.GetMessageId(message.MessageNumber).ItemId;

			client.SetMessageReadState(messageId, false);
		}

		public void MarkDeleted(ChannelMessageHeader message)
		{
			var trash = GetFolders().First(f => f.FolderType == ChannelFolderType.Trash);

			var client = new ExchangeClient(Hostname, CredentialsProvider);
			var messageId = client.GetMessageId(message.MessageNumber).ItemId;

			client.MoveMessageToFolder(messageId, trash.FolderId);	
		}

		public void SetStarred(ChannelMessageHeader message, bool starred)
		{
			var client = new ExchangeClient(Hostname, CredentialsProvider);
			var messageId = client.GetMessageId(message.MessageNumber).ItemId;

			client.SetMessageImportance(messageId, starred);
		}

		public void Purge(ChannelMessageHeader message)
		{
			var client = new ExchangeClient(Hostname, CredentialsProvider);
			var messageId = client.GetMessageId(message.MessageNumber).ItemId;

			client.DeleteMessage(messageId);
		}

		public ChannelFolder CreateFolder(string folderName)
		{
			var client = new ExchangeClient(Hostname, CredentialsProvider);
			var folderId = client.CreateFolder(folderName, folders.First(f => f.FolderType == ChannelFolderType.Inbox).FolderId);

			var newFolder = new ChannelFolder(folderId, folderName, ChannelFolderType.Label);

			folders.Add(newFolder);

			return newFolder;
		}

		public void MoveToFolder(ChannelMessageHeader message, ChannelFolder folder)
		{
			var client = new ExchangeClient(Hostname, CredentialsProvider);
			var messageId = client.GetMessageId(message.MessageNumber).ItemId;

			client.MoveMessageToFolder(messageId, folder.FolderId);
		}

		public void CopyToFolder(ChannelMessageHeader message, ChannelFolder folder)
		{
			var client = new ExchangeClient(Hostname, CredentialsProvider);
			var messageId = client.GetMessageId(message.MessageNumber).ItemId;

			client.CopyMessageToFolder(messageId, folder.FolderId);
		}

		public void RemoveFromFolder(ChannelMessageHeader message, ChannelFolder folder)
		{
			var client = new ExchangeClient(Hostname, CredentialsProvider);
			var messageId = client.GetMessageId(message.MessageNumber).ItemId;

			client.DeleteMessage(messageId);
		}

		public void AddLabel(ChannelMessageHeader message, string labelname)
		{
			var client = new ExchangeClient(Hostname, CredentialsProvider);
			var messageId = client.GetMessageId(message.MessageNumber).ItemId;

			client.CopyMessageToFolder(messageId, folders.First(f => f.Name.ToLower() == labelname.ToLower()).FolderId);
		}

		public void RemoveLabel(string messagenumber, string labelname)
		{
			var client = new ExchangeClient(Hostname, CredentialsProvider);
			var messageId = client.GetMessageId(messagenumber).ItemId;

			client.DeleteMessage(messageId);
		}

		public IEnumerable<ChannelContact> GetContacts()
		{
			var client = new ExchangeClient(Hostname, CredentialsProvider);

			foreach (var contactItem in client.GetContacts())
			{
				if (contactItem.EmailAddresses == null || contactItem.EmailAddresses.Length == 0)
				{
					Logger.Warn("Contact {0} had no email address, ignoring", LogSource.Sync, contactItem.DisplayName);

					continue;
				}

				var contact = new ChannelContact();

				contact.Profile.ChannelProfileKey = contactItem.ItemId.Id;
				contact.Profile.SourceAddress = new SourceAddress(contactItem.EmailAddresses[0].Value, contactItem.DisplayName);

				contact.Person.Lastname = contactItem.Surname;
				contact.Person.Firstname = contactItem.GivenName;

				if (contactItem.BirthdaySpecified)
					contact.Person.DateOfBirth = contactItem.Birthday;

				contact.Profile.ScreenName = contactItem.Nickname;
				contact.Profile.Title = contactItem.JobTitle;
				contact.Profile.ScreenName = contactItem.DisplayName;
				contact.Profile.CompanyName = contactItem.CompanyName;

				if (contactItem.PhysicalAddresses.Length > 0)
				{
					contact.Profile.Street = contactItem.PhysicalAddresses[0].Street;
					contact.Profile.ZipCode = contactItem.PhysicalAddresses[0].PostalCode;
					contact.Profile.City = contactItem.PhysicalAddresses[0].City;
					contact.Profile.Country = contactItem.PhysicalAddresses[0].CountryOrRegion;
				}

				yield return contact;
			}
		}

		IClientContactsChannel IClientContactsChannel.Clone()
		{
			return new ExchangeClientChannel()
				{
					Hostname = Hostname,
					Port = Port,
					IsSecured = IsSecured,
					MaxConcurrentConnections = MaxConcurrentConnections,
					CredentialsProvider = CredentialsProvider
				};
		}
    }
}
