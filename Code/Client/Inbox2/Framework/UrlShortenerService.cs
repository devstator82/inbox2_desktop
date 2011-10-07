using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Inbox2.Framework
{
	public static class UrlShortenerService
	{
		public static string Shorten(string text)
		{
			if (String.IsNullOrEmpty(text))
				return String.Empty;

			if (String.IsNullOrEmpty(text.Trim()))
				return String.Empty;

			var sb = new StringBuilder();
			var re = new Regex(@"(?<url>((news|(ht|f)tp(s?))\://){1}\S+)", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
			var lines = re.Split(text);			

			foreach (string line in lines)
			{
				if ((line.StartsWith("http://") || line.StartsWith("https://")) && line.Length > 20)
				{
					var shortener = PluginsManager.Current.UrlShorteners.FirstOrDefault();

					if (shortener != null)
					{
						var result = shortener.Shorten(line.Trim());

						sb.AppendFormat("{0} ", result);
					}
				}
				else
				{
					sb.Append(line);
				}
			}

			return sb.ToString();
		}
	}
}
