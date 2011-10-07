using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using HtmlAgilityPack;
using Inbox2.Platform.Channels.Extensions;

namespace Inbox2.Platform.Channels.Text.Html
{
	public delegate void HtmlSanitizerNodeVisited(string nodeName, HtmlNode node, XmlWriter writer);

	public class HtmlSanitizer
	{
		private static List<string> _nodesToSkip = new[] { "style", "script", "html", "body", "head", "iframe" }.ToList();

		public event HtmlSanitizerNodeVisited NodeVisited;

		public string Sanitize(string source)
		{
			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(source);

			HtmlNode node = doc.DocumentNode.SelectSingleNode("//body");

			// No body, try to select the html node directly
			if (node == null)
				node = doc.DocumentNode.SelectSingleNode("//html");

			// If body not found (html contains no body tag), use the DocumentNode
			if (node == null)
				node = doc.DocumentNode;

			using (MemoryStream ms = new MemoryStream())
			{
				XmlTextWriter writer = new XmlTextWriter(ms, System.Text.Encoding.UTF8);

				writer.WriteStartElement("div");

				if (node.HasChildNodes)
				{
					foreach (var childNode in node.ChildNodes)
						WriteTo(writer, childNode);
				}
				else
				{
					WriteTo(writer, node);
				}

				writer.WriteEndElement();
				writer.Flush();

				// our source html is allready double encoded, so the following decode
				// will put it in proper order
				return HttpUtility.HtmlDecode(ms.ReadString());
			}
		}

		void WriteTo(XmlWriter writer, HtmlNode node)
		{
			// The semicolon is for crap that outlook produces
			if (_nodesToSkip.Contains(node.Name.ToLower()) || node.Name.Contains(":"))
				return;

			switch (node.NodeType)
			{
				case HtmlNodeType.Text:
					writer.WriteString(((HtmlTextNode)node).Text);
					break;

				case HtmlNodeType.Element:
					writer.WriteStartElement(node.Name);

					if (NodeVisited != null)
						NodeVisited(node.Name.ToLower(), node, writer);

					// Make links external
					if (node.Name.ToLower() == "a")
					{
						node.Attributes.Remove("target");
						node.Attributes.Append("target", "_blank");											
					}
					else if (node.Name.ToLower() == "table")
					{
						if (node.Attributes["align"] != null)
							node.Attributes.Remove("align");

						if (node.Attributes["width"] != null)
							node.Attributes.Remove("width");
					}

					if (node.HasAttributes)
					{
						// we use _hashitems to make sure attributes are written only once
						foreach (string key in node.Attributes.Hashitems.Keys)
						{
							if (key.StartsWith("on"))
								continue;

							HtmlAttribute att = (HtmlAttribute)node.Attributes.Hashitems[key];
							writer.WriteAttributeString(att.XmlName, att.Value);
						}
					}

					if (node.HasChildNodes)
					{
						foreach (HtmlNode subnode in node.ChildNodes)
						{
							WriteTo(writer, subnode);
						}
					}
					writer.WriteEndElement();
					break;
			}
		}
	}
}
