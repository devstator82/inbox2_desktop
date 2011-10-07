using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Extensions;
using LumiSoft.Net.Mime;

namespace Inbox2.Platform.Channels.Parsing
{
	public static class ChannelMessageParser
	{
		public static ChannelMessage From(Stream stream)
		{
			return From(stream, null);
		}

		public static ChannelMessage From(Stream stream, ChannelMessageHeader header)
		{
			stream.Seek(0, SeekOrigin.Begin);

			var mimeMessage = Mime.Parse(stream);
			var message = new ChannelMessage();

			if (header != null)
			{
				message.MessageNumber = header.MessageNumber;
				message.Size = header.Size;
				message.SourceFolder = header.SourceFolder;
				message.IsRead = header.IsRead;
				message.IsStarred = header.IsStarred;
				message.DateReceived = header.DateReceived;
			}
			else
			{
				message.SourceFolder = "INBOX";
				message.DateReceived = DateTime.Now;
			}

			message.Context = mimeMessage.MainEntity.Subject;
			message.MessageIdentifier = mimeMessage.MainEntity.MessageID;
			message.InReplyTo = mimeMessage.MainEntity.InReplyTo;
			message.BodyHtml = mimeMessage.BodyHtml.ToStream();

			if (!String.IsNullOrEmpty(mimeMessage.BodyText))
				message.BodyText = mimeMessage.BodyText.ToStream();

			message.From = new SourceAddress(mimeMessage.MainEntity.From.ToAddressListString());

			if (mimeMessage.MainEntity.To == null)
			{
				var deliveredTo = mimeMessage.MainEntity.Header.GetFirst("Delivered-To:");

				if (deliveredTo != null)
					message.To = new SourceAddressCollection(deliveredTo.Value);
			}
			else
			{
				message.To = new SourceAddressCollection(mimeMessage.MainEntity.To.ToAddressListString());
			}

			if (mimeMessage.MainEntity.Cc != null)
				message.CC = new SourceAddressCollection(mimeMessage.MainEntity.Cc.ToAddressListString());

			if (mimeMessage.MainEntity.Bcc != null)
				message.BCC = new SourceAddressCollection(mimeMessage.MainEntity.Bcc.ToAddressListString());

			foreach (var att in mimeMessage.Attachments)
			{
				var attachment = new ChannelAttachment
				    {
				        Filename = att.ContentDisposition == ContentDisposition_enum.Attachment
				                 	? att.ContentDisposition_FileName
				                 	: att.ContentType_Name
				    };

				if (!String.IsNullOrEmpty(att.ContentID))
					attachment.ContentId = att.ContentID.Trim('<', '>');

				attachment.ContentType = String.IsNullOrEmpty(att.ContentID)
					? ContentType.Attachment : ContentType.Inline;

				attachment.ContentStream = new MemoryStream(att.Data);

				message.Attachments.Add(attachment);
			}

			return message;
		}
	}
}
