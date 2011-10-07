using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Exceptions;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Framework.Interop;
using LumiSoft.Net.Mail;
using LumiSoft.Net.MIME;
using LumiSoft.Net.SMTP.Client;

namespace Inbox2.Channels.Smtp
{
	public class SmtpClientChannel : IClientOutputChannel
	{
		public string Hostname { get; set; }

		public int Port { get; set; }

		public bool IsSecured { get; set; }

		public bool IsEnabled
		{
			get { return true; }
			set { }
		}

		public int MaxConcurrentConnections { get; set; }

		public IChannelCredentialsProvider CredentialsProvider { get; set; }

		public string Protocol
		{
			get { return "SMTP"; }
		}

		public void Send(ChannelMessage message)
		{
			var creds = CredentialsProvider.GetCredentials().ToNetworkCredential();
		
			Mail_Message msg = new Mail_Message();

			msg.MimeVersion = "1.0";
			msg.MessageID = MIME_Utils.CreateMessageID();
			msg.Subject = message.Context;
			msg.From = message.From.ToMailBoxList();
			msg.ReplyTo = message.ReturnTo == null ?
				message.From.ToAddressList() : message.ReturnTo.ToAddressList();					

			if (String.IsNullOrEmpty(message.InReplyTo) == false)
				msg.InReplyTo = message.InReplyTo;

			msg.To = new Mail_t_AddressList();
			foreach (var address in message.To)
				msg.To.Add(address.ToMailBox());

			msg.Cc = new Mail_t_AddressList();
			foreach (var address in message.CC)
				msg.Cc.Add(address.ToMailBox());			

			//--- multipart/mixed -------------------------------------------------------------------------------------------------
			MIME_h_ContentType contentType_multipartMixed = new MIME_h_ContentType(MIME_MediaTypes.Multipart.mixed);
			contentType_multipartMixed.Param_Boundary = Guid.NewGuid().ToString().Replace('-', '.');
			MIME_b_MultipartMixed multipartMixed = new MIME_b_MultipartMixed(contentType_multipartMixed);
			msg.Body = multipartMixed;

			//--- multipart/alternative -----------------------------------------------------------------------------------------
			MIME_Entity entity_multipartAlternative = new MIME_Entity();
			MIME_h_ContentType contentType_multipartAlternative = new MIME_h_ContentType(MIME_MediaTypes.Multipart.alternative);
			contentType_multipartAlternative.Param_Boundary = Guid.NewGuid().ToString().Replace('-', '.');
			MIME_b_MultipartAlternative multipartAlternative = new MIME_b_MultipartAlternative(contentType_multipartAlternative);
			entity_multipartAlternative.Body = multipartAlternative;
			multipartMixed.BodyParts.Add(entity_multipartAlternative);

			//--- text/plain ----------------------------------------------------------------------------------------------------
			MIME_Entity entity_text_plain = new MIME_Entity();
			MIME_b_Text text_plain = new MIME_b_Text(MIME_MediaTypes.Text.plain);
			entity_text_plain.Body = text_plain;
			// Add text body if there is any
			if (message.BodyText != null && message.BodyText.Length > 0)
			{
				var bodyText = message.BodyText.ReadString();

				// Make sure there is a newline at the end of our text, otherwise it will screw up
				// our multipart data format
				if (!bodyText.EndsWith(Environment.NewLine))
					bodyText = bodyText + Environment.NewLine;

				text_plain.SetText(MIME_TransferEncodings.SevenBit, Encoding.UTF8, bodyText);
			}
				
			multipartAlternative.BodyParts.Add(entity_text_plain);

			//--- text/html ------------------------------------------------------------------------------------------------------
			MIME_Entity entity_text_html = new MIME_Entity();
			MIME_b_Text text_html = new MIME_b_Text(MIME_MediaTypes.Text.html);
			entity_text_html.Body = text_html;
			if (message.BodyHtml != null && message.BodyHtml.Length > 0)
			{
				var bodyHtml = message.BodyHtml.ReadString();

				// Make sure there is a newline at the end of our text, otherwise it will screw up
				// our multipart data format
				if (!bodyHtml.EndsWith(Environment.NewLine))
					bodyHtml = bodyHtml + Environment.NewLine;

				text_html.SetText(MIME_TransferEncodings.SevenBit, Encoding.UTF8, bodyHtml);
			}

			multipartAlternative.BodyParts.Add(entity_text_html);

			foreach (var channelAttachment in message.Attachments)
			{
				MIME_b_Application attachmentBody = new MIME_b_Application(MIME_MediaTypes.Application.octet_stream);
				MIME_Entity attachment = new MIME_Entity();
				attachment.Body = attachmentBody;

				// Has to happen before the following lines of code
				multipartMixed.BodyParts.Add(attachment);

				attachment.ContentType = new MIME_h_ContentType(MimeHelper.GetMimeType(channelAttachment.Filename));
				attachment.ContentType.Param_Name = channelAttachment.Filename;

				MIME_h_ContentDisposition contentDisposition = new MIME_h_ContentDisposition(DispositionTypeNames.Attachment);
				contentDisposition.Param_FileName = channelAttachment.Filename;

				attachment.ContentDisposition = contentDisposition;
				attachment.ContentTransferEncoding = TransferEncoding.Base64.ToString();

				attachmentBody.SetData(channelAttachment.ContentStream, MIME_TransferEncodings.Base64);
			}

			// Inject headers
			if (!String.IsNullOrEmpty(message.MessageIdentifier))
				msg.Header.Add(new MIME_h_Unstructured("x-i2mp-messageid", message.MessageIdentifier));

			//if (!String.IsNullOrEmpty(message.Metadata.i2mpFlow))
			//    msg.Header.Add(new MIME_h_Unstructured("x-i2mp-flow", message.Metadata.i2mpFlow));

			//if (!String.IsNullOrEmpty(message.Metadata.i2mpReference))
			//    mailMessage.Headers.Add("X-i2mp-ref", message.Metadata.i2mpReference);

			//if (!String.IsNullOrEmpty(message.Metadata.i2mpSequence))
			//    mailMessage.Headers.Add("X-i2mp-seq", message.Metadata.i2mpSequence);

			//if (!String.IsNullOrEmpty(message.Metadata.i2mpRelation))
			//    mailMessage.Headers.Add("X-i2mp-rel", message.Metadata.i2mpRelation);

			//if (!String.IsNullOrEmpty(message.Metadata.i2mpRelationId))
			//    mailMessage.Headers.Add("X-i2mp-rel-id", message.Metadata.i2mpRelationId);


			// Send message
			try
			{
				SMTP_Client client = new SMTP_Client();

				if ("/Settings/Channels/LoggerEnabled".AsKey(false))
					client.Logger = new LumiSoft.Net.Log.Logger();

				// todo push this logic into the smtp client implementation itself
				if (Hostname == "smtp.live.com")
				{
					// Hack for hotmail, first do a connect with no secured channel,
					// then a STARTTLS
					client.Connect(Hostname, Port, false);
					client.StartTLS();
				}
				else
				{
					client.Connect(Hostname, Port, IsSecured);
				}

				client.Authenticate(creds.UserName, creds.Password);				

				using (MemoryStream ms = new MemoryStream())
				{
					client.MailFrom(msg.From[0].Address, -1);

					msg.ToStream(ms,
						new MIME_Encoding_EncodedWord(MIME_EncodedWordEncoding.Q, Encoding.UTF8), Encoding.UTF8);

					// Reset stream
					ms.Seek(0, SeekOrigin.Begin);

					foreach (var address in message.To)
						client.RcptTo(address.Address);	

					foreach (var address in message.CC)
						client.RcptTo(address.Address);	

					foreach (var address in message.BCC)
						client.RcptTo(address.Address);						

					try
					{
						client.SendMessage(ms);
					}
					finally
					{
						client.Dispose();
					}				
				}
			}
			catch(SmtpFailedRecipientsException e)
			{
				throw new ChannelFunctionalException(e.Message, e) { DoNotRetry = true };
			}
			catch (SmtpException e)
			{
				throw new ChannelFunctionalException(e.Message, e);
			}
			catch (Exception e)
			{
				throw new ChannelFunctionalException(e.Message, e);
			}
		}

		public void Dispose()
		{
			
		}
	}
}
