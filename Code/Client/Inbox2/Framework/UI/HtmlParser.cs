using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using HtmlAgilityPack;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Framework.UI
{
	public static class HtmlParser
	{
		public static void AppendTo(TextBlock text, string source)
		{
			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(source);

			if (doc.DocumentNode.HasChildNodes)
			{
				foreach (var childNode in doc.DocumentNode.ChildNodes)
					AppendTo(text, childNode);
			}
			else
			{
				AppendTo(text, doc.DocumentNode);
			}
		}

		static void AppendTo(TextBlock text, HtmlNode node)
		{
			try
			{
				if (node.NodeType == HtmlNodeType.Element 
				    && node.Name.ToLower() == "a" 
				    && !String.IsNullOrEmpty(node.InnerText))
				{
					var href = node.Attributes["href"].Value;

					if (Uri.IsWellFormedUriString(href, UriKind.Absolute))
					{
						var hyperlink = new WebHyperlink {Foreground = (Brush) Application.Current.FindResource("DefaultForegroundColor")};

						if (href.StartsWith("mailto:"))
						{
							hyperlink.Inlines.Add(node.InnerText);
							hyperlink.Command = Commands.New;
							hyperlink.CommandParameter = new SourceAddress(node.InnerText);
						}
						else
						{
							hyperlink.Inlines.Add(node.InnerText);
							hyperlink.NavigateUri = new Uri(href);
						}

						text.Inlines.Add(hyperlink);
						return;
					}
				}
			}
			catch
			{
				// ignore
			}

			text.Inlines.Add(node.InnerText.Replace(Environment.NewLine, String.Empty)
			                 	.Replace("\n", String.Empty)
			                 	.Replace("\r", String.Empty));
		}
	}
}


