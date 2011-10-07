using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using Inbox2.Core.Configuration;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Channels.Text.Html;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Plugins.Conversations.Utils
{
	public static class MessageBodyGenerator
	{
		public static string CreateBodyText()
		{
			return CreateBodyText(null);
		}

		public static string CreateBodyText(string text)
		{
			StringBuilder sb = new StringBuilder();

			if (text != null && !String.IsNullOrEmpty(text.Trim()))
				sb.Append(text);

			AppendSignature(true, sb);

			return sb.ToString();
		}

		public static string CreateBodyTextForReply(Message source, string text)
		{
			var str = CreateBodyTextForReply(source, true);

			if (text != null && !String.IsNullOrEmpty(text.Trim()))
				return text + str;

			return str;
		}

		public static string CreateBodyTextForReply(Message source, bool appendSignature)
		{
			StringBuilder sb = new StringBuilder();

			AppendSignature(appendSignature, sb);

			sb.AppendLine("<br /><br />");

			sb.AppendLine("---------- Reply message ----------<br />");
			sb.AppendFormat("From: {0}\n<br />", source.From.ToEncodedString());
			sb.AppendFormat("Date: {0}\n<br />", source.DateReceived ?? source.DateSent);
			sb.AppendFormat("Subject: {0}\n<br />", source.Context);
			sb.AppendFormat("To: {0}\n<br />", source.To.ToEncodedString());

			if (source.CC.Count > 0)
				sb.AppendFormat("CC: {0}\n<br />", source.CC.ToEncodedString());

			sb.AppendLine("<br />");
			sb.AppendLine("<br />");

			var access = new ClientMessageAccess(source);
			var sanitizer = new HtmlSanitizer();
			var sanitized = sanitizer.Sanitize(access.GetBestBodyMatch(TextConversion.ToHtml));

			sb.Append(sanitized);

			return sb.ToString();
		}
		
		public static string CreateBodyTextForForward(Message source, bool appendSignature)
		{
			StringBuilder sb = new StringBuilder();

			AppendSignature(appendSignature, sb);

			sb.AppendLine("<br /><br />");

			sb.AppendLine("---------- Forwarded message ----------<br />");
			sb.AppendFormat("From: {0}\n<br />", source.From.ToEncodedString());
			sb.AppendFormat("Date: {0}\n<br />", source.DateReceived ?? source.DateSent);
			sb.AppendFormat("Subject: {0}\n<br />", source.Context);
			sb.AppendFormat("To: {0}\n<br />", source.To.ToEncodedString());

			if (source.CC.Count > 0)
				sb.AppendFormat("CC: {0}\n<br />", source.CC.ToEncodedString());

			sb.Append("<br /><br />");

			var access = new ClientMessageAccess(source);
			var sanitizer = new HtmlSanitizer();
			var sanitized = sanitizer.Sanitize(access.GetBestBodyMatch(TextConversion.ToHtml));

			sb.Append(sanitized);

			return sb.ToString();
		}

		static void AppendSignature(bool appendSignature, StringBuilder sb)
		{
			if (appendSignature)
			{
				var signature = SettingsManager.ClientSettings.AppConfiguration.Signature ?? String.Empty;

				if (signature.Trim() != String.Empty)
				{
					sb.Append("<br /><br />");
					sb.Append(signature.NewlineToBreak());
				}
			}
		}
	}
}