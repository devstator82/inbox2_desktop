using System;
using System.Linq;
using System.Xml;
using HtmlAgilityPack;
using Inbox2.Framework;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Text.Html;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Plugins.Conversations.Helpers
{
	internal class MessageBuilder
	{
		private readonly Message source;

		public MessageBuilder(Message source)
		{
			this.source = source;
		}

		internal string GetMessageHtmlView()
		{
			var access = new ClientMessageAccess(source);
			var sanitizer = new HtmlSanitizer();

			sanitizer.NodeVisited += SanitizerNodeVisited;

			var sanitized = sanitizer.Sanitize(access.GetBestBodyMatch(TextConversion.ToHtml));

			return sanitized;
		}

		void SanitizerNodeVisited(string nodeName, HtmlNode node, XmlWriter writer)
		{
			if (nodeName == "img" && node.Attributes["src"] != null && node.Attributes["src"].Value.StartsWith("cid:"))
			{
				// split src
				var src = node.Attributes["src"].Value.Split(new[] { ':' }, 2);

				if (src.Length == 2)
				{
					// Find inline attachment with given contentid
					var document = source.Documents.FirstOrDefault(d => d.ContentType == ContentType.Inline && d.ContentId == src[1]);

					if (document != null)
					{
						// Replace content-id url with filename
						var filename = ClientState.Current.Storage.ResolvePhysicalFilename(".", document.StreamName);

						node.Attributes["src"].Value = String.Format("file://{0}", filename);
					}
				}
			}
			else if (nodeName == "a" && node.Attributes["href"] != null)
			{
				var url = node.Attributes["href"].Value;

				// Clean href and inject javascript hook
				node.Attributes["href"].Value = String.Empty;

				writer.WriteAttributeString("onclick", String.Format("javascript:window.external.JsNavigate('{0}')", url));
			}
		}
	}
}
