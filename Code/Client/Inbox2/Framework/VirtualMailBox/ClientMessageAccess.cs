using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Attributes;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Framework.VirtualMailBox
{
	public class ClientMessageAccess
	{
		private readonly Message message;
		private readonly string bodyHtml;
		private readonly string bodyText;

		[IndexTokenizeAndStore]
		public string BodyHtml
		{
			get { return bodyHtml; }
		}

		[IndexTokenizeAndStore]
		public string BodyText
		{
			get { return bodyText; }
		}

		public ClientMessageAccess(Message message)
		{
			this.message = message;
			
			if (message.BodyHtmlStreamName.HasValue)
				using (var s = ClientState.Current.Storage.Read("m", message.BodyHtmlStreamName.ToString()))
					bodyHtml = s.ReadString();

			if (message.BodyTextStreamName.HasValue)
				using (var s = ClientState.Current.Storage.Read("m", message.BodyTextStreamName.ToString()))
					bodyText = s.ReadString();
		}

		public ClientMessageAccess(Message message, string bodyText, string bodyHtml)
		{
			this.message = message;
			this.bodyText = bodyText;
			this.bodyHtml = bodyHtml;
		}

		public string GetBestBodyMatch()
		{
			return GetBestBodyMatch(TextConversion.None);
		}

		public string GetBestBodyMatch(TextConversion conversion)
		{
			if (message.IsHtml)
				return conversion == TextConversion.ToText ? BodyHtml.StripHtml() : BodyHtml;
			else
				return conversion == TextConversion.ToHtml ? BodyText.MakeLinksClickable().NewlineToBreak() : BodyText;
		}

		public string GetBodyPreview()
		{
			int length = 200 - (String.IsNullOrEmpty(message.Context) ? 0 : message.Context.Length);

			if (String.IsNullOrEmpty(BodyHtml))
				return BodyText
					.GetStringBoundary(length < 0 ? 0 : length)
					.StripWhitespace();
			else
				return BodyHtml.RemoveReplyText(!String.IsNullOrEmpty(message.InReplyTo)).StripHtml()
					.GetStringBoundary(length < 0 ? 0 : length)
					.StripWhitespace();
		}

		public Guid? WriteBodyHtml()
		{
			if (!String.IsNullOrEmpty(BodyHtml) && BodyHtml.Trim().Length > 0)
			{
				Guid filename = Guid.NewGuid();

				ClientState.Current.Storage.Write("m", filename.ToString(), BodyHtml.ToStream());

				return filename;
			}

			return null;
		}

		public Guid? WriteBodyText()
		{
			if (!String.IsNullOrEmpty(BodyText) && BodyText.Trim().Length > 0)
			{
				Guid filename = Guid.NewGuid();

				ClientState.Current.Storage.Write("m", filename.ToString(), BodyText.ToStream());

				return filename;
			}

			return null;
		}		
	}
}
