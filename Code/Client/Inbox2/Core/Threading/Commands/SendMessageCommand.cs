using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using Inbox2.Core.Configuration;
using Inbox2.Core.Configuration.Channels;
using Inbox2.Core.Threading.Tasks;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Localization;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Threading;
using Inbox2.Framework.Threading.Progress;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Framework.Web;
using Inbox2.Platform.Framework.Web.Upload;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Platform.Logging;

namespace Inbox2.Core.Threading.Commands
{
	class SendMessageCommand : ConnectedCommand
	{
		private readonly Message message;
		private readonly VirtualMailBox mailbox;

		public SendMessageCommand(Message message)
		{
			this.message = message;
			this.mailbox = VirtualMailBox.Current;
		}

		protected override void ExecuteCore()
		{
			var access = new ClientMessageAccess(message);
			var group = new ProgressGroup { Status = Strings.SendingMessage };

			ProgressManager.Current.Register(group);

			var msg = message.DuckCopy<ChannelMessage>();

			try
			{
				msg.BodyText = access.BodyText.ToStream();
				msg.BodyHtml = access.BodyHtml.ToStream();

				// Handle attachments
				foreach (var document in message.Documents)
				{
					var attachment = new ChannelAttachment
					    {
					        Filename = document.Filename,
					        ContentType = document.ContentType,
					        ContentStream = new MemoryStream()
					    };

					attachment.ContentStream = File.OpenRead(
						ClientState.Current.Storage.ResolvePhysicalFilename(".", document.StreamName));

					msg.Attachments.Add(attachment);
				}

				Logger.Debug("Message has {0} attachments", LogSource.BackgroundTask, msg.Attachments.Count);

				var recipients = BuildRecipients();

				try
				{
					group.SourceChannelId = message.TargetChannelId;

					var channel = ChannelsManager.GetChannelObject(message.TargetChannelId);

					// The messageid itself is not used by the send channel, so inject our friendlyrowkey into that field.
					// The field is added to the headers collection which in turn is used again to determine if its a sent
					// item that is allready in your inbox2 sent items folder.
					msg.MessageIdentifier = message.MessageKey;
					msg.ConversationId = message.ConversationIdentifier;

					// Filter only the recipients that belong to this channel
					msg.To = recipients[message.TargetChannelId].To;
					msg.CC = recipients[message.TargetChannelId].CC;
					msg.BCC = recipients[message.TargetChannelId].BCC;

					Logger.Debug("Sending message. Channel = {0}", LogSource.BackgroundTask, channel.Configuration.DisplayName);

					if (channel.Configuration.IsConnected)
						SendCloudMessage(channel.Configuration, msg);
					else
						channel.OutputChannel.Send(msg);

					Logger.Debug("Send was successfull. Channel = {0}", LogSource.BackgroundTask, channel.Configuration.DisplayName);
				}
				catch (Exception ex)
				{
					ClientState.Current.ShowMessage(
						new AppMessage(String.Concat(Strings.UnableToSendMessage, ", ", 
							String.Format(Strings.ServerSaid, ex.Message)))
							{
								SourceChannelId = message.TargetChannelId
							}, MessageType.Error);

					throw;
				}

				EventBroker.Publish(AppEvents.SendMessageFinished);

				Thread.CurrentThread.ExecuteOnUIThread(() =>
					ClientState.Current.ShowMessage(
						new AppMessage(Strings.MessageSentSuccessfully)
						{
							EntityId = message.MessageId.Value,
							EntityType = EntityType.Message
						}, MessageType.Success));
			}
			finally
			{
				if (msg.BodyText != null)
					msg.BodyText.Dispose();

				if (msg.BodyHtml != null)
					msg.BodyHtml.Dispose();

				group.IsCompleted = true;

				// Close attachment streams
				foreach (var channelAttachment in msg.Attachments)
				{
					channelAttachment.ContentStream.Dispose();
					channelAttachment.ContentStream = null;
				}
			}
		}

		public Dictionary<long, Recipients> BuildRecipients()
		{
			var channels = new List<ChannelInstance>();
			var result = new Dictionary<long, Recipients>();

			// Append recients per channel
			if (message.MessageFolder == Folders.SentItems
				|| message.MessageFolder == Folders.Drafts)
			{
				// Use target channels
				channels.Add(ChannelsManager.GetChannelObject(message.TargetChannel.ChannelId));
			}
			else
			{
				// Use source channel
				channels.Add(ChannelsManager.GetChannelObject(message.SourceChannel.ChannelId));
			}

			foreach (var channel in channels)
			{
				var recipients = new Recipients();

				recipients.To.AddRange(FilterByTargetChannel(message.To, channel));
				recipients.CC.AddRange(FilterByTargetChannel(message.CC, channel));
				recipients.BCC.AddRange(FilterByTargetChannel(message.BCC, channel));

				if (!recipients.IsEmpty)
				{
					result.Add(channel.Configuration.ChannelId, recipients);
				}
			}

			return result;
		}

		IEnumerable<SourceAddress> FilterByTargetChannel(IEnumerable<SourceAddress> source, ChannelInstance channel)
		{
			foreach (var address in source)
			{
				if (channel.Configuration.Charasteristics.SupportsEmail)
				{
					// Mail channels allow everything that is a valid email address
					if (SourceAddress.IsValidEmail(address.Address))
						yield return address;
				}
				else
				{
					// Social channels only allow entries that are available in the addressbook 
					SourceAddress address1 = address;

					using (mailbox.Profiles.ReaderLock)
						if (mailbox.Profiles.Where(p => p.Address == address1.Address).Count() > 0)
							yield return address;
				}
			}
		}

		void SendCloudMessage(ChannelConfiguration channel, ChannelMessage message)
		{
			var data = String.Format("wrap_access_token={0}&targetchannelkey={1}&from={2}&to={3}&cc={4}&bcc={5}&subject={6}&inreplyto={7}&conversationidentifier={8}&body={9}",
				CloudApi.AccessToken, 
				channel.ChannelKey, 				
				HttpUtility.UrlEncode(message.From.ToString()),
				HttpUtility.UrlEncode(message.To.ToString()),
				HttpUtility.UrlEncode(message.CC.ToString()),
				HttpUtility.UrlEncode(message.BCC.ToString()),
				HttpUtility.UrlEncode(message.Context),
				HttpUtility.UrlEncode(message.InReplyTo),
				HttpUtility.UrlEncode(message.ConversationId),
				HttpUtility.UrlEncode(message.BodyHtml.ReadString()));

			string messageKey;

			if (message.Attachments.Count > 0)
			{
				var files = message.Attachments
					.Select(s => new UploadFile(s.ContentStream, s.Filename, "application/octet-stream"))
					.ToList();

				messageKey = HttpServiceRequest.Post(String.Concat(CloudApi.ApiBaseUrl, "send/message"), data, files, true);
			}
			else
			{
				messageKey = HttpServiceRequest.Post(String.Concat(CloudApi.ApiBaseUrl, "send/message"), data, true);	
			}

			// Update messagekey
			this.message.MessageKey = messageKey;

			ClientState.Current.DataService.Update(this.message);
		}
	}
}
