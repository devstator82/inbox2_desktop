using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Inbox2.Platform.Logging;

namespace Inbox2.Platform.Channels.Extensions
{
	public static class StringExtensions
	{
		/// <summary>
		/// Returns the subject without re: fw: etc
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static string ToClearSubject(this string source)
		{
			if (String.IsNullOrEmpty(source))
				return String.Empty;

			return Regex.Replace(source, @"re:|fw:|fw(.{1}):", "",
			                     RegexOptions.IgnoreCase | RegexOptions.Compiled).Trim();
		}

		/// <summary>
		/// Converts the newlines to html break tags.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static string NewlineToBreak(this string source)
		{
			if (String.IsNullOrEmpty(source))
				return String.Empty;

			return source
				.Replace(Environment.NewLine, "<br />")
				.Replace("\r", "<br />")
				.Replace("\n", "<br />");
		}

		/// <summary>
		/// Removes newlines and replaces multiple spaces with a single space.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string StripWhitespace(this string str)
		{
			var stripped = str
				.Replace(Environment.NewLine, String.Empty)
				.Replace("\n", String.Empty)
				.Replace("\r", String.Empty)
				.Replace("\t", String.Empty);

			// Replace multiple spaces with a single space
			Regex regex = new Regex(@"[ ]{2,}", RegexOptions.Compiled);

			return regex.Replace(stripped, " ");
		}

		/// <summary>
		/// Converts the mailto and urls to clickable links
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static string MakeLinksClickable(this string source)
		{
			if (String.IsNullOrEmpty(source))
				return String.Empty;

			return Regex.Replace(source, "(?<url>(mailto\\:|(news|(ht|f)tp(s?))\\://){1}\\S+)", 
				"<a href=\"${url}\">${url}</a>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}

		/// <summary>
		/// Converts the mailto and urls to clickable links
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static string MakeLinksClickableIncludingMailto(this string source)
		{
			if (String.IsNullOrEmpty(source))
				return String.Empty;

			source = Regex.Replace(source, "(?<url>(mailto\\:|(news|(ht|f)tp(s?))\\://){1}\\S+)",
				"<a href=\"${url}\">${url}</a>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

			source = Regex.Replace(source, "(['\"]{1,}.+['\"]{1,}\\s+)?<?[\\w\\.\\-]+@[^\\.][\\w\\.\\-]+\\.[a-z]{2,}>?",
			   "<a href=\"mailto:${0}${1}\">${0}${1}</a>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

			return source;
		}

		/// <summary>
		/// Replaces @replies and #hastags with twitter links
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string MakeTwitterLinksClickable(this string source)
		{
			if (String.IsNullOrEmpty(source))
				return String.Empty;

			var words = source.Split(' ');
			var sb = new StringBuilder();

			foreach (var word in words)
			{
				if (word.StartsWith("@"))
				{
					sb.AppendFormat("<a href=\"http://www.twitter.com/{0}\">{1}</a> ", word.Substring(1), word);
				}
				else if (word.StartsWith("#"))
				{
					sb.AppendFormat("<a href=\"http://twitter.com/search?q=%23{0}\">{1}</a> ", word.Substring(1), word);
				}
				else
				{
					sb.AppendFormat("{0} ", word);
				}
			}

			return sb.ToString().Trim();
		}

        /// <summary>
        /// Converts the mailto and urls to clickable links, implemented for the activity view
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static string MakeLinksClickableForActivityView(this string source)
        {
            if (String.IsNullOrEmpty(source))
                return String.Empty;

            //Convert the URLS
            source = Regex.Replace(source, "(?<url>((news|(ht|f)tp(s?))\\://){1}\\S+)",
                "<a href=\"${url}\" target=\"_blank\">${url}</a>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            //Convert the e-mailaddresses to 'inbox2 new message link'
            source = Regex.Replace(source, "(['\"]{1,}.+['\"]{1,}\\s+)?<?[\\w\\.\\-]+@[^\\.][\\w\\.\\-]+\\.[a-z]{2,}>?",
               "<a onclick=\"newmessagemodal.emptymessage('${0}${1}')\">${0}${1}</a>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            return source;

            
        }

		public static string StripHtml(this string source)
		{
			if (String.IsNullOrEmpty(source))
				return String.Empty;

			// Remove HTML Development formatting
			// Replace line breaks with space
			// because browsers inserts space
			string result = source.Replace("\r", " ");
			// Replace line breaks with space
			// because browsers inserts space
			result = result.Replace("\n", " ");
			// Remove step-formatting
			result = result.Replace("\t", string.Empty);
			// Remove repeating spaces because browsers ignore them
			result = Regex.Replace(result,
			                                                      @"( )+", " ");

			// Remove the header (prepare first by clearing attributes)
			result = Regex.Replace(result,
			                                                      @"<( )*head([^>])*>", "<head>",
			                                                      RegexOptions.IgnoreCase);
			result = Regex.Replace(result,
			                                                      @"(<( )*(/)( )*head( )*>)", "</head>",
			                                                      RegexOptions.IgnoreCase);
			result = Regex.Replace(result,
			                                                      "(<head>).*(</head>)", string.Empty,
			                                                      RegexOptions.IgnoreCase);

			// remove all scripts (prepare first by clearing attributes)
			result = Regex.Replace(result,
			                                                      @"<( )*script([^>])*>", "<script>",
			                                                      RegexOptions.IgnoreCase);
			result = Regex.Replace(result,
			                                                      @"(<( )*(/)( )*script( )*>)", "</script>",
			                                                      RegexOptions.IgnoreCase);
			//result = Regex.Replace(result,
			//         @"(<script>)([^(<script>\.</script>)])*(</script>)",
			//         string.Empty,
			//         RegexOptions.IgnoreCase);
			result = Regex.Replace(result,
			                                                      @"(<script>).*(</script>)", string.Empty,
			                                                      RegexOptions.IgnoreCase);

			// remove all styles (prepare first by clearing attributes)
			result = Regex.Replace(result,
			                                                      @"<( )*style([^>])*>", "<style>",
			                                                      RegexOptions.IgnoreCase);
			result = Regex.Replace(result,
			                                                      @"(<( )*(/)( )*style( )*>)", "</style>",
			                                                      RegexOptions.IgnoreCase);
			result = Regex.Replace(result,
			                                                      "(<style>).*(</style>)", string.Empty,
			                                                      RegexOptions.IgnoreCase);

			// insert tabs in spaces of <td> tags
			result = Regex.Replace(result,
			                                                      @"<( )*td([^>])*>", "\t",
			                                                      RegexOptions.IgnoreCase);

			// insert line breaks in places of <BR> and <LI> tags
			result = Regex.Replace(result,
			                                                      @"<( )*br( )*>", "\r\n",
			                                                      RegexOptions.IgnoreCase);
			result = Regex.Replace(result,
			                                                      @"<( )*li( )*>", "\r\n",
			                                                      RegexOptions.IgnoreCase);

			// insert line paragraphs (double line breaks) in place
			// if <P>, <DIV> and <TR> tags
			result = Regex.Replace(result,
			                                                      @"<( )*div([^>])*>", "\r\r",
			                                                      RegexOptions.IgnoreCase);
			result = Regex.Replace(result,
			                                                      @"<( )*tr([^>])*>", "\r\r",
			                                                      RegexOptions.IgnoreCase);
			result = Regex.Replace(result,
			                                                      @"<( )*p([^>])*>", "\r\r",
			                                                      RegexOptions.IgnoreCase);

			// Remove remaining tags like <a>, links, images,
			// comments etc - anything that's enclosed inside < >
			result = Regex.Replace(result,
			                                                      @"<[^>]*>", string.Empty,
			                                                      RegexOptions.IgnoreCase);

			// replace special characters:
			result = Regex.Replace(result,
			                                                      @" ", " ",
			                                                      RegexOptions.IgnoreCase);

			result = Regex.Replace(result,
			                                                      @"&bull;", " * ",
			                                                      RegexOptions.IgnoreCase);
			result = Regex.Replace(result,
			                                                      @"&lsaquo;", "<",
			                                                      RegexOptions.IgnoreCase);
			result = Regex.Replace(result,
			                                                      @"&rsaquo;", ">",
			                                                      RegexOptions.IgnoreCase);
			result = Regex.Replace(result,
			                                                      @"&trade;", "(tm)",
			                                                      RegexOptions.IgnoreCase);
			result = Regex.Replace(result,
			                                                      @"&frasl;", "/",
			                                                      RegexOptions.IgnoreCase);
			result = Regex.Replace(result,
			                                                      @"&lt;", "<",
			                                                      RegexOptions.IgnoreCase);
			result = Regex.Replace(result,
			                                                      @"&gt;", ">",
			                                                      RegexOptions.IgnoreCase);
			result = Regex.Replace(result,
			                                                      @"&copy;", "(c)",
			                                                      RegexOptions.IgnoreCase);
			result = Regex.Replace(result,
			                                                      @"&reg;", "(r)",
			                                                      RegexOptions.IgnoreCase);
			// Remove all others. More can be added, see
			// http://hotwired.lycos.com/webmonkey/reference/special_characters/
			result = Regex.Replace(result,
			                                                      @"&(.{2,6});", string.Empty,
			                                                      RegexOptions.IgnoreCase);

			// for testing
			//Regex.Replace(result,
			//       this.txtRegex.Text,string.Empty,
			//       RegexOptions.IgnoreCase);

			// make line breaking consistent
			//result = result.Replace("\n", "\r");

			// Remove extra line breaks and tabs:
			// replace over 2 breaks with 2 and over 4 tabs with 4.
			// Prepare first to remove any whitespaces in between
			// the escaped characters and remove redundant tabs in between line breaks
			result = Regex.Replace(result,
			                                                      "(\r)( )+(\r)", "\r\r",
			                                                      RegexOptions.IgnoreCase);
			result = Regex.Replace(result,
			                                                      "(\t)( )+(\t)", "\t\t",
			                                                      RegexOptions.IgnoreCase);
			result = Regex.Replace(result,
			                                                      "(\t)( )+(\r)", "\t\r",
			                                                      RegexOptions.IgnoreCase);
			result = Regex.Replace(result,
			                                                      "(\r)( )+(\t)", "\r\t",
			                                                      RegexOptions.IgnoreCase);
			// Remove redundant tabs
			result = Regex.Replace(result,
			                                                      "(\r)(\t)+(\r)", "\r\r",
			                                                      RegexOptions.IgnoreCase);
			// Remove multiple tabs following a line break with just one tab
			result = Regex.Replace(result,
			                                                      "(\r)(\t)+", "\r\t",
			                                                      RegexOptions.IgnoreCase);
			// Initial replacement target string for line breaks
			string breaks = "\r\r\r";
			// Initial replacement target string for tabs
			string tabs = "\t\t\t\t\t";
			for (int index = 0; index < result.Length; index++)
			{
				result = result.Replace(breaks, "\r\r");
				result = result.Replace(tabs, "\t\t\t\t");
				breaks = breaks + "\r";
				tabs = tabs + "\t";
			}

			// That's it.
			return result.Trim();			
		}

		public static string GetStringBoundary(this string source, int maxChars)
		{
			if (String.IsNullOrEmpty(source))
				return String.Empty;

			var trimmed = source.Trim();

			if (trimmed.Length <= maxChars)
				return source;

			return trimmed.Substring(0, maxChars) + "...";
		}

		public static string GetUrlBoundary(this string source, int maxChars)
		{
			if (String.IsNullOrEmpty(source))
				return String.Empty;

			try
			{
				Uri uri = new Uri(source);

				StringBuilder sb = new StringBuilder();
				sb.Append(uri.Host);
				sb.Append(uri.AbsolutePath);

				string value = sb.ToString();

				return GetStringBoundary(value, maxChars);
			}
			catch
			{
				Logger.Error("Unable to parse url. Url = {0}", LogSource.None, source);

				return GetStringBoundary(source, maxChars);
			}
		}

        public static string RemoveReplyText(this string source, bool messageIsInReplyTo)
        {
            //Only use when the message is InReplyTo something
            if (!messageIsInReplyTo) return source;

            // Search for reply text for messages from inbox2: ------Reply Message-----
            int indexOfStartReplyText = source.IndexOf("---------- Reply message ----------");
            if (indexOfStartReplyText >= 0)
                return source.Substring(0, indexOfStartReplyText);

            // Search for reply text for messages from GMail: <blockquote class="gmail_quote"
            indexOfStartReplyText = source.IndexOf(@"<blockquote class=""gmail_quote""");
            if (indexOfStartReplyText >= 0)
                return source.Substring(0, indexOfStartReplyText);

            // Search for reply text for messages from Hotmail: <br><br><hr id="stopSpelling">
            indexOfStartReplyText = source.IndexOf(@"<br><br><hr id=""stopSpelling"">");
            if (indexOfStartReplyText >= 0)
                return source.Substring(0, indexOfStartReplyText);

            // Search for reply text for messages from Outook: <div style='border:none;border-top:solid #B5C4DF
            indexOfStartReplyText = source.IndexOf(@"<div style='border:none;border-top:solid #B5C4DF");
            if (indexOfStartReplyText >= 0)
                return source.Substring(0, indexOfStartReplyText);

            // TODO: Search for reply text for messages from BlackBerry: ?

            // Search for reply text for messages from Iphone Default Mail Client: On [DATE AND MORE] [NAME] wrote:[BODY]</blockquote>
            return Regex.Replace(source, @"\bOn\W+(?:\w+\W+){1,200}?wrote\W+(?:\w+\W+){1,}?</blockquote>",
               "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        //TODO: this method transforms urls like http://www.google.com to www google com - fix it, then use it
		public static string GetWordsBoundary(this string source, int maxChars)
		{
			if (String.IsNullOrEmpty(source))
				return String.Empty;

			// All words + the dot character
			Regex re = new Regex(@"\b(\w+|\.)\b", RegexOptions.None);
			MatchCollection mc = re.Matches(source);

			StringBuilder sb = new StringBuilder();
			foreach (Match ma in mc)
			{
				sb.AppendFormat("{0} ", ma.Value);

				if (sb.Length > maxChars)
					break;
			}

			return sb.ToString().Trim() + "...";
		}
	}
}