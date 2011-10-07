using System;
using System.IO;
using Inbox2.Framework;
using Inbox2.Framework.Threading;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Platform.Logging;
using Inbox2ClientWorker.Core.Threading.Handlers;
using Inbox2ClientWorker.Core.Threading.Handlers.Matchers;

namespace Inbox2ClientWorker.Core.Threading.Tasks.Receive
{
	public class ReceiveMessageDetailsTask : BackgroundTask
	{
		private readonly ChannelConfiguration config;
		private readonly IClientInputChannel channel;
		private readonly ChannelFolder folder;
		private readonly ChannelMessageHeader header;		

		public ReceiveMessageDetailsTask(ChannelConfiguration config, IClientInputChannel channel, ChannelMessageHeader header, ChannelFolder folder)
		{
			this.config = config;
			this.channel = channel;
			this.header = header;
			this.folder = folder;
		}

		protected override void ExecuteCore()
		{
			try
			{
				Logger.Debug("Retreiving message {0} from {1}", LogSource.Receive, header, config.DisplayName);

				foreach (var channelMessage in channel.GetMessage(header))
				{
					// Create a duck copy from the mailMessage.
					var message = new Message
					    {
					        MessageNumber = header.MessageNumber,
							MessageIdentifier = channelMessage.MessageIdentifier,
							From = channelMessage.From,
							ReturnTo = channelMessage.ReturnTo,
							To = channelMessage.To,
							CC = channelMessage.CC,
							BCC = channelMessage.BCC,				
							InReplyTo = channelMessage.InReplyTo,
							Size = header.Size,
							Context = channelMessage.Context.ToClearSubject(),
							OriginalContext = channelMessage.Context,
							ConversationIdentifier = channelMessage.ConversationId,
							SourceFolder = channelMessage.SourceFolder,
							SourceChannelId = channelMessage.SourceChannelId,
							TargetChannelId = channelMessage.TargetChannelId,
							Metadata = channelMessage.Metadata,
							IsRead = channelMessage.IsRead,
							IsStarred = channelMessage.IsStarred,
							DateReceived = channelMessage.DateReceived,
							DateSent = channelMessage.DateSent							
					    };

					message.Context = message.Context != null ? message.Context.Trim() : String.Empty;

					string bodyText = channelMessage.BodyText.ReadString();
					string bodyHtml = channelMessage.BodyHtml.ReadString();

					var access = new ClientMessageAccess(message, bodyText, bodyHtml);

					if (folder.ToStorageFolder() == Folders.SentItems)
					{
						// For sent items we sent the TargetChannelId
						message.TargetChannelId = config.ChannelId;
					}
					else
					{
						// For all other items we sent the SourceChannelId
						message.SourceChannelId = config.ChannelId;
					}

					// Create BodyPreview field from reader
					message.BodyPreview = access.GetBodyPreview();
					message.BodyHtmlStreamName = access.WriteBodyHtml();
					message.BodyTextStreamName = access.WriteBodyText();
					message.MessageFolder = folder.ToStorageFolder();
					message.Metadata = header.Metadata;

					// Fix for messages which have a timestamp in the futre
					if (message.DateReceived > DateTime.Now)
						message.DateReceived = DateTime.Now;
					
                    // Set IsNew state for message
                    if (!message.IsRead)
                        message.IsNew = true;

					// Save message
					ClientState.Current.DataService.Save(message);


					// Message received, process attachments
					foreach (var attachment in channelMessage.Attachments)
					{
						var document = new Document
						    {
						        Filename = attachment.Filename,
						        SourceChannelId = attachment.SourceChannelId,
						        TargetChannelId = attachment.TargetChannelId,
								DocumentFolder = folder.ToStorageFolder(),
						        ContentType = attachment.ContentType,
						        ContentId = attachment.ContentId,
								ContentStream = attachment.ContentStream
						    };

						document.SourceChannelId = config.ChannelId;
						document.DateReceived = message.DateReceived;
						document.DateSent = message.DateSent;
						document.Message = message;

						DocumentsHandler.DocumentReceived(document);

						if (attachment.ContentStream != null)
						{
							attachment.ContentStream.Dispose();
							attachment.ContentStream = null;
						}
					}

					if (channelMessage.BodyText != null)
						channelMessage.BodyText.Dispose();

					if (channelMessage.BodyHtml != null)
						channelMessage.BodyHtml.Dispose();

					// Match thread
					MessageMatcher.Match(message);

					// Add to search index
					ClientState.Current.Search.Store(message);

					new ProfileMatcher(message).Execute();
				}
			}
			catch (Exception ex)
			{
				Logger.Error("An error occured when trying to download header {0}. Exception = {1}", LogSource.BackgroundTask, header, ex);

				throw;
			}
		}		
	}
}