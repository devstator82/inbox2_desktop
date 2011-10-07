using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeServicesWsdlClient;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Framework.Interop;

namespace Inbox2.Channels.Exchange
{
	public class ExchangeClient
	{
		private readonly string hostname;
		private readonly string username;
		private readonly string password;

		public ExchangeClient(string hostname, IChannelCredentialsProvider provider)
		{
			var creds = provider.GetCredentials();

			this.hostname = hostname;
			this.username = creds.Claim;
			this.password = creds.Evidence;
		}

		public ExchangeClient(string hostname, string username, string password)
		{
			this.hostname = hostname;
			this.username = username;
			this.password = password;
		}

		public BaseFolderType GetFolder(DistinguishedFolderIdNameType folderType)
		{
			var binding = ChannelHelper.BuildChannel(hostname, username, password);

			var getFolderType = new GetFolderType 
				{
					FolderIds = new[] { new DistinguishedFolderIdType { Id = folderType } },
					FolderShape = new FolderResponseShapeType { BaseShape = DefaultShapeNamesType.AllProperties }
				};

			var getFolderResponse = binding.GetFolder(getFolderType);

			if (getFolderResponse.ResponseMessages.Items[0].ResponseClass == ResponseClassType.Error)
				throw new Exception(getFolderResponse.ResponseMessages.Items[0].MessageText);

			return ((FolderInfoResponseMessageType) getFolderResponse.ResponseMessages.Items[0]).Folders[0];
		}

		public BaseFolderType GetRootFolder()
		{
			var binding = ChannelHelper.BuildChannel(hostname, username, password);

			var findFolderType = new FindFolderType
			{
				Traversal = FolderQueryTraversalType.Shallow,
				ParentFolderIds = new[] { new DistinguishedFolderIdType { Id = DistinguishedFolderIdNameType.root } },
				FolderShape = new FolderResponseShapeType { BaseShape = DefaultShapeNamesType.AllProperties }
			};

			var findFolderResponse = binding.FindFolder(findFolderType);

			if (findFolderResponse.ResponseMessages.Items[0].ResponseClass == ResponseClassType.Error)
				throw new Exception(findFolderResponse.ResponseMessages.Items[0].MessageText);

			BaseFolderType[] bft = ((FindFolderResponseMessageType)findFolderResponse.ResponseMessages.Items[0]).RootFolder.Folders;

			return bft[0];
		}

		/// <summary>
		/// Returns all folders in the exchange box.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<BaseFolderType> FindAllFolders(string id)
		{
			var binding = ChannelHelper.BuildChannel(hostname, username, password);

			var findFolderType = new FindFolderType
             	{
             		Traversal = FolderQueryTraversalType.Shallow,
					ParentFolderIds = new[] { new FolderIdType { Id = id } },
             		FolderShape = new FolderResponseShapeType { BaseShape = DefaultShapeNamesType.AllProperties }
             	};

			FindFolderResponseType findFolderResponse = binding.FindFolder(findFolderType);

			if (findFolderResponse.ResponseMessages.Items[0].ResponseClass == ResponseClassType.Error)
				throw new Exception(findFolderResponse.ResponseMessages.Items[0].MessageText);

			var folders = ((FindFolderResponseMessageType)findFolderResponse.ResponseMessages.Items[0]).RootFolder.Folders;

			foreach (var baseFolderType in folders)
			{
				yield return baseFolderType;

				// Find children of current folder
				foreach (var folderType in FindAllFolders(baseFolderType.FolderId.Id))
					yield return folderType;
			}
		}

		/// <summary>
		/// Gets a list of all the items in the mailbox with all their properties.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<MessageType> GetHeaders(ChannelFolder folder)
		{
			var binding = ChannelHelper.BuildChannel(hostname, username, password);

			var findItemRequest = new FindItemType { Traversal = ItemQueryTraversalType.Shallow };
			var itemProperties = new ItemResponseShapeType { BaseShape = DefaultShapeNamesType.AllProperties };
			
			findItemRequest.ItemShape = itemProperties;
			findItemRequest.ParentFolderIds = new BaseFolderIdType[] { new FolderIdType { Id = folder.FolderId } };

			FindItemResponseType findItemResponse = binding.FindItem(findItemRequest);

			foreach (FindItemResponseMessageType responseMessage in findItemResponse.ResponseMessages.Items)
			{
				if (responseMessage.ResponseClass == ResponseClassType.Success)
				{
					ArrayOfRealItemsType mailboxItems = (ArrayOfRealItemsType) responseMessage.RootFolder.Item;

					if (mailboxItems.Items == null)
						yield break;

					foreach (MessageType inboxItem in mailboxItems.Items)
						yield return inboxItem;
				}
			}
		}

		/// <summary>
		/// Gets a specific message from exchange by id (attachments are only loaded shallow).
		/// </summary>
		/// <param name="messageId"></param>
		/// <returns></returns>
		public MessageType GetMessage(string messageId)
		{
			var binding = ChannelHelper.BuildChannel(hostname, username, password);

			ItemIdType itemId = new ItemIdType();
			itemId.Id = messageId;

			GetItemType getItemRequest = new GetItemType();
			getItemRequest.ItemShape = new ItemResponseShapeType { BaseShape = DefaultShapeNamesType.AllProperties };
			getItemRequest.ItemIds = new[] { itemId };

			GetItemResponseType getItemResponse = binding.GetItem(getItemRequest);

			if (getItemResponse.ResponseMessages.Items[0].ResponseClass == ResponseClassType.Error)
				throw new Exception(getItemResponse.ResponseMessages.Items[0].MessageText);

			var getItemResponseMessage = (ItemInfoResponseMessageType)getItemResponse.ResponseMessages.Items[0];

			if (getItemResponseMessage.Items.Items == null || getItemResponseMessage.Items.Items.Length == 0)
				throw new ApplicationException("Error in GetMessage, empty ItemInfoResponseMessageType");

			return (MessageType)getItemResponseMessage.Items.Items[0];
		}

		/// <summary>
		/// Gets the id and changekey of a specific message by id.
		/// </summary>
		/// <param name="messageId"></param>
		/// <returns></returns>
		public MessageType GetMessageId(string messageId)
		{
			var binding = ChannelHelper.BuildChannel(hostname, username, password);

			ItemIdType itemId = new ItemIdType();
			itemId.Id = messageId;

			// Re-get item and changekey from exchange
			GetItemResponseType getItemResponse = binding.GetItem(new GetItemType
				{
					ItemShape = new ItemResponseShapeType { BaseShape = DefaultShapeNamesType.IdOnly },
					ItemIds = new[] { itemId }
				});

			if (getItemResponse.ResponseMessages.Items[0].ResponseClass == ResponseClassType.Error)
				throw new Exception(getItemResponse.ResponseMessages.Items[0].MessageText);

			return (MessageType)((ItemInfoResponseMessageType)getItemResponse.ResponseMessages.Items[0]).Items.Items[0];
		}

		/// <summary>
		/// Asks Exchange to send a specific message.
		/// </summary>
		/// <param name="messageId"></param>
		public void SendMessage(ItemIdType messageId)
		{
			var binding = ChannelHelper.BuildChannel(hostname, username, password);

			// Send message
			var sendItem = new SendItemType { ItemIds = new BaseItemIdType[1], SavedItemFolderId = new TargetFolderIdType() };
			var siSentItemsFolder = new DistinguishedFolderIdType { Id = DistinguishedFolderIdNameType.sentitems };

			sendItem.SavedItemFolderId.Item = siSentItemsFolder;
			sendItem.SaveItemToFolder = true;
			sendItem.ItemIds[0] = messageId;

			SendItemResponseType sendItemResponse = binding.SendItem(sendItem);

			if (sendItemResponse.ResponseMessages.Items[0].ResponseClass == ResponseClassType.Error)
				throw new Exception(sendItemResponse.ResponseMessages.Items[0].MessageText);
		}

		/// <summary>
		/// Gets a specific attachment from exchange by id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public FileAttachmentType GetAttachment(string id)
		{
			var binding = ChannelHelper.BuildChannel(hostname, username, password);
			var getAttachmentRequest = new GetAttachmentType();

			var attachmentIdArray = new RequestAttachmentIdType[1];
			attachmentIdArray[0] = new RequestAttachmentIdType { Id = id };

			getAttachmentRequest.AttachmentIds = attachmentIdArray;

			GetAttachmentResponseType getAttachmentResponse = binding.GetAttachment(getAttachmentRequest);

			if (getAttachmentResponse.ResponseMessages.Items[0].ResponseClass == ResponseClassType.Error)
				throw new Exception(getAttachmentResponse.ResponseMessages.Items[0].MessageText);

			var attachmentResponseMessage =
				(AttachmentInfoResponseMessageType)getAttachmentResponse.ResponseMessages.Items[0];

			if (attachmentResponseMessage.Attachments == null || attachmentResponseMessage.Attachments.Length == 0)
				throw new ApplicationException("Error in GetAttachment, empty AttachmentInfoResponseMessageType");

			return (FileAttachmentType)attachmentResponseMessage.Attachments[0];
		}

		/// <summary>
		/// Saves the given message to the drafts folder.
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public ItemIdType SaveMessage(ChannelMessage message)
		{
			var binding = ChannelHelper.BuildChannel(hostname, username, password);

			var createItemRequest = new CreateItemType();

			// Indicate that we want to save only at first
			createItemRequest.MessageDisposition = MessageDispositionType.SaveOnly;
			createItemRequest.MessageDispositionSpecified = true;
			createItemRequest.Items = new NonEmptyArrayOfAllItemsType();

			// Create a single e-mail message.
			var exchMessage = new MessageType();
			exchMessage.Subject = message.Context;
			exchMessage.Body = new BodyType { BodyType1 = BodyTypeType.HTML, Value = message.BodyHtml.ReadString() };
			exchMessage.ItemClass = "IPM.Note";
			exchMessage.Sender = new SingleRecipientType();
			exchMessage.Sender.Item = new EmailAddressType { EmailAddress = message.From.Address };

			exchMessage.ToRecipients = new EmailAddressType[message.To.Count];
			exchMessage.CcRecipients = new EmailAddressType[message.CC.Count];
			exchMessage.BccRecipients = new EmailAddressType[message.BCC.Count];

			for (int i = 0; i < message.To.Count; i++)
				exchMessage.ToRecipients[i] = new EmailAddressType { EmailAddress = message.To[i].Address };

			for (int i = 0; i < message.CC.Count; i++)
				exchMessage.CcRecipients[i] = new EmailAddressType { EmailAddress = message.CC[i].Address };

			for (int i = 0; i < message.BCC.Count; i++)
				exchMessage.BccRecipients[i] = new EmailAddressType { EmailAddress = message.BCC[i].Address };

			exchMessage.Sensitivity = SensitivityChoicesType.Normal;

			// Add the message to the array of items to be created.
			createItemRequest.Items.Items = new ItemType[1];
			createItemRequest.Items.Items[0] = exchMessage;

			// Send the request to create and send the e-mail item, and get the response.
			CreateItemResponseType createItemResponse = binding.CreateItem(createItemRequest);

			// Determine whether the request was a success.
			if (createItemResponse.ResponseMessages.Items[0].ResponseClass == ResponseClassType.Error)
				throw new Exception(createItemResponse.ResponseMessages.Items[0].MessageText);

			return ((ItemInfoResponseMessageType)createItemResponse.ResponseMessages.Items[0]).Items.Items[0].ItemId;
		}

		/// <summary>
		/// Saves all attachments belonging to a specific message. This method can only be called after the message
		/// has been saved in exchange.
		/// </summary>
		/// <param name="messageId"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public IEnumerable<ItemIdType> SaveAttachments(ItemIdType messageId, ChannelMessage message)
		{
			var binding = ChannelHelper.BuildChannel(hostname, username, password);

			// Create add attachment request.
			var attachementRequest = new CreateAttachmentType();
			attachementRequest.ParentItemId = messageId;
			attachementRequest.Attachments = new AttachmentType[message.Attachments.Count];

			for (int i = 0; i < message.Attachments.Count; i++)
			{
				var channelAttachment = message.Attachments[i];
				var exchAttachment = new FileAttachmentType();

				exchAttachment.Name = channelAttachment.Filename;
				exchAttachment.ContentType = MimeHelper.GetMimeType(channelAttachment.Filename);
				exchAttachment.Content = channelAttachment.ContentStream.GetBytes();

				attachementRequest.Attachments[i] = exchAttachment;

				var saveAttachmentResponse = binding.CreateAttachment(attachementRequest);

				// Determine whether the request was a success.
				if (saveAttachmentResponse.ResponseMessages.Items[0].ResponseClass == ResponseClassType.Error)
					throw new Exception(saveAttachmentResponse.ResponseMessages.Items[0].MessageText);

				AttachmentIdType attachmentId = ((AttachmentInfoResponseMessageType)saveAttachmentResponse.ResponseMessages.Items[0]).Attachments[0].AttachmentId;

				yield return new ItemIdType { ChangeKey = attachmentId.RootItemChangeKey, Id = attachmentId.RootItemId };
			}
		}

		/// <summary>
		/// Delete the given message permanatly.
		/// </summary>
		/// <param name="messageId"></param>
		public void DeleteMessage(ItemIdType messageId)
		{
			var binding = ChannelHelper.BuildChannel(hostname, username, password);

			var deleteItemRequest = new DeleteItemType
              	{
              		ItemIds = new BaseItemIdType[] {messageId},
              		DeleteType = DisposalType.HardDelete
              	};

			DeleteItemResponseType deleteResponse = binding.DeleteItem(deleteItemRequest);

			if (deleteResponse.ResponseMessages.Items[0].ResponseClass == ResponseClassType.Error)
				throw new Exception(deleteResponse.ResponseMessages.Items[0].MessageText);
		}

		/// <summary>
		/// Sets the message readstate.
		/// </summary>
		public void SetMessageReadState(ItemIdType messageId, bool isRead)
		{
			var binding = ChannelHelper.BuildChannel(hostname, username, password);

			var setField = new SetItemFieldType
			{
				Item1 = new MessageType { IsRead = isRead, IsReadSpecified = true },
				Item = new PathToUnindexedFieldType { FieldURI = UnindexedFieldURIType.messageIsRead }
			};

			var updatedItems = new[]
               	{
					new ItemChangeType
                  	{
                  		Updates = new ItemChangeDescriptionType[] { setField },
                  		Item = messageId
                  	}
				};

			var request = new UpdateItemType
			{
				ItemChanges = updatedItems,
				ConflictResolution = ConflictResolutionType.AutoResolve,
				MessageDisposition = MessageDispositionType.SaveOnly,
				MessageDispositionSpecified = true
			};

			UpdateItemResponseType updateItemResponse = binding.UpdateItem(request);

			if (updateItemResponse.ResponseMessages.Items[0].ResponseClass == ResponseClassType.Error)
				throw new Exception(updateItemResponse.ResponseMessages.Items[0].MessageText);
		}

		/// <summary>
		/// Sets the message importance flag.
		/// </summary>
		public void SetMessageImportance(ItemIdType messageId, bool isStarred)
		{
			var binding = ChannelHelper.BuildChannel(hostname, username, password);

			var setField = new SetItemFieldType
			{
				Item1 = new MessageType { Importance = isStarred ? ImportanceChoicesType.High : ImportanceChoicesType.Normal, ImportanceSpecified = true },
				Item = new PathToUnindexedFieldType { FieldURI = UnindexedFieldURIType.itemImportance }
			};

			var updatedItems = new[]
               	{
					new ItemChangeType
                  	{
                  		Updates = new ItemChangeDescriptionType[] { setField },
                  		Item = messageId
                  	}
				};

			var request = new UpdateItemType
			{
				ItemChanges = updatedItems,
				ConflictResolution = ConflictResolutionType.AutoResolve,
				MessageDisposition = MessageDispositionType.SaveOnly,
				MessageDispositionSpecified = true
			};

			UpdateItemResponseType updateItemResponse = binding.UpdateItem(request);

			if (updateItemResponse.ResponseMessages.Items[0].ResponseClass == ResponseClassType.Error)
				throw new Exception(updateItemResponse.ResponseMessages.Items[0].MessageText);			
		}

		/// <summary>
		/// Creates a folder with the given name and returns the associated folderid.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="parentFolderId"></param>
		/// <returns></returns>
		public string CreateFolder(string name, string parentFolderId)
		{
			var binding = ChannelHelper.BuildChannel(hostname, username, password);

			var request = new CreateFolderType
				{
					Folders = new BaseFolderType[] { new FolderType { DisplayName = name }  },
					ParentFolderId = new TargetFolderIdType { Item = new FolderIdType { Id = parentFolderId } }
				};

			CreateFolderResponseType moveItemResponse = binding.CreateFolder(request);

			if (moveItemResponse.ResponseMessages.Items[0].ResponseClass == ResponseClassType.Error)
				throw new Exception(moveItemResponse.ResponseMessages.Items[0].MessageText);

			var response = (FolderInfoResponseMessageType) moveItemResponse.ResponseMessages.Items[0];
			return response.Folders[0].FolderId.Id;
		}

		/// <summary>
		/// Moves the given message to the given folder.
		/// </summary>
		public void MoveMessageToFolder(ItemIdType messageId, string folderId)
		{
			var binding = ChannelHelper.BuildChannel(hostname, username, password);

			var request = new MoveItemType
				{
					ItemIds = new BaseItemIdType[] { messageId },
					ToFolderId = new TargetFolderIdType { Item = new FolderIdType { Id = folderId } }
				};

			MoveItemResponseType moveItemResponse = binding.MoveItem(request);

			if (moveItemResponse.ResponseMessages.Items[0].ResponseClass == ResponseClassType.Error)
				throw new Exception(moveItemResponse.ResponseMessages.Items[0].MessageText);
		}

		/// <summary>
		/// Moves the given message to the given folder.
		/// </summary>
		public void CopyMessageToFolder(ItemIdType messageId, string folderId)
		{
			var binding = ChannelHelper.BuildChannel(hostname, username, password);

			var request = new CopyItemType
			{
				ItemIds = new BaseItemIdType[] { messageId },
				ToFolderId = new TargetFolderIdType { Item = new FolderIdType { Id = folderId } }
			};

			CopyItemResponseType moveItemResponse = binding.CopyItem(request);

			if (moveItemResponse.ResponseMessages.Items[0].ResponseClass == ResponseClassType.Error)
				throw new Exception(moveItemResponse.ResponseMessages.Items[0].MessageText);
		}

		/// <summary>
		/// Gets the nr of items in a given folder
		/// </summary>
		/// <returns></returns>
		public long GetNrItemsInFolder(ChannelFolder folder)
		{
			var binding = ChannelHelper.BuildChannel(hostname, username, password);

			var findItemRequest = new FindItemType { Traversal = ItemQueryTraversalType.Shallow };
			var itemProperties = new ItemResponseShapeType { BaseShape = DefaultShapeNamesType.AllProperties };
			findItemRequest.ItemShape = itemProperties;

			var folderIdArray = new DistinguishedFolderIdType[2];
			folderIdArray[0] = new DistinguishedFolderIdType { Id = DistinguishedFolderIdNameType.inbox };

			findItemRequest.ParentFolderIds = folderIdArray;

			FindItemResponseType findItemResponse = binding.FindItem(findItemRequest);

			// Determine whether the request was a success.
			if (findItemResponse.ResponseMessages.Items[0].ResponseClass == ResponseClassType.Error)
				throw new Exception(findItemResponse.ResponseMessages.Items[0].MessageText);

			var responseMessage =
				(FindItemResponseMessageType) findItemResponse.ResponseMessages.Items[0];

			var mailboxItems = (ArrayOfRealItemsType)responseMessage.RootFolder.Item;

			if (mailboxItems.Items == null) return 0;

			return mailboxItems.Items.Length;
		}

		/// <summary>
		/// Retrieves contacts from exchange database.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ContactItemType> GetContacts()
		{
			var binding = ChannelHelper.BuildChannel(hostname, username, password);

			var findItemRequest = new FindItemType
              	{
              		ItemShape = new ItemResponseShapeType {BaseShape = DefaultShapeNamesType.AllProperties},
              		ParentFolderIds = new[] { new DistinguishedFolderIdType { Id = DistinguishedFolderIdNameType.contacts } }
              	};

			FindItemResponseType findItemResponse = binding.FindItem(findItemRequest);

			// Determine whether the request was a success.
			if (findItemResponse.ResponseMessages.Items[0].ResponseClass == ResponseClassType.Error)
				throw new Exception(findItemResponse.ResponseMessages.Items[0].MessageText);

			var responseMessage = (FindItemResponseMessageType)findItemResponse.ResponseMessages.Items[0];
			var contactItems = (ArrayOfRealItemsType)responseMessage.RootFolder.Item;

			return contactItems.Items.Cast<ContactItemType>();
		}
	}
}
