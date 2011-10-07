using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Inbox2.Platform.Channels.Extensions;

namespace Inbox2.Platform.Framework.Extensions
{
	public static class StringExtensions
	{
		/// <summary>
		/// Looks up the given key in the configuration file and casts it if found.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns></returns>
		public static T AsKey<T>(this string key, T defaultValue)
		{
			string value = ConfigurationManager.AppSettings[key];

			if (String.IsNullOrEmpty(value))
				return defaultValue;

			var converter = TypeDescriptor.GetConverter(typeof(T));

			return (T)converter.ConvertFromInvariantString(value);
		}		

        private static char[] ignoredChars = "!@#$%^&*()+=-[]\\';,./{}|\":<>?".ToArray();

		/// <summary>
		/// Sanitizes the given string. Warning: this method is quite performance intenstive, so use it smart.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
        public static string Sanitize(this string str)
        {
            if (String.IsNullOrEmpty(str))
                return String.Empty;

            StringBuilder sb = new StringBuilder();

            Regex re = new Regex(@"\A(((re):+)|((fwd):+)|((fw):+))\s*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex re2 = new Regex(@"(\.){2,}", RegexOptions.Compiled);

            // Removes trailing and leading whitespace
            str = str.Trim();

            // Removes re: fw: etc
            str = re.Replace(str, String.Empty);

            // Replaces double and triple dots as in 'Hello world...' with single dots ('Hello world.')
            str = re2.Replace(str, ".").Trim(ignoredChars).Trim();

            foreach (var c in str)
            {
                if (ignoredChars.Contains(c))
                    continue;

                if (Char.IsWhiteSpace(c))
                {
                    if (sb[sb.Length - 1] != '-')
                        sb.Append('-');

                    continue;
                }

                sb.Append(c);
            }

            return sb.ToString().ToLower();
        }

        public static string HtmlEncode(this string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

		public static string Base64Encode(this string str)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(str);
			return Convert.ToBase64String(bytes);
		}

		public static string Base64Decode(this string str)
		{
			byte[] bytes = Convert.FromBase64String(str);
			return Encoding.UTF8.GetString(bytes);
		}

        public static string HtmlEncodeForActivityView(this string str)
        {
            return HttpUtility.HtmlEncode(str).MakeLinksClickableForActivityView();
        }

        public static string Nl2Br(this string str)
        {
			if (String.IsNullOrEmpty(str))
				return String.Empty;

            string breakedString = str.Replace(Environment.NewLine, "<br />");
            return breakedString.Replace("\n", "<br />");
        }

		public static string AddSlashes(this string source)
		{
			if (String.IsNullOrEmpty(source))
				return String.Empty;

			return source.Replace("\\", "\\\\").Replace("'", "\\'");
		}

		public static string AddSQLiteSlashes(this string source)
		{
			if (String.IsNullOrEmpty(source))
				return source;

			return source.Replace("'", "''");
		}

		public static string JsEncode(this string source)
		{
			if (String.IsNullOrEmpty(source))
				return "\"\"";

			StringBuilder sb = new StringBuilder();
			sb.Append("\"");
			foreach (char c in source)
			{
				switch (c)
				{
					case '\"':
						sb.Append("\\\"");
						break;
					case '\\':
						sb.Append("\\\\");
						break;
					case '\b':
						sb.Append("\\b");
						break;
					case '\f':
						sb.Append("\\f");
						break;
					case '\n':
						sb.Append("\\n");
						break;
					case '\r':
						sb.Append("\\r");
						break;
					case '\t':
						sb.Append("\\t");
						break;
					default:
						int i = (int)c;
						if (i < 32 || i > 127)
						{
							sb.AppendFormat("\\u{0:X04}", i);
						}
						else
						{
							sb.Append(c);
						}
						break;
				}
			}
			sb.Append("\"");

			return sb.ToString();

		}

		public static string Capitalize(this string source)
		{
			if (String.IsNullOrEmpty(source))
				return source;

			if (source.Length == 1)
				return source.ToUpper();

			return String.Concat(source[0].ToString().ToUpper(), source.Substring(1).ToLower());
		}

        public static bool StartsWithAny(this string source, IEnumerable<string> matches)
        {
            foreach (var match in matches)
            {
                if (source.StartsWith(match))
                    return true;
            }

            return false;
        }

		public static string[] SmartSplit(this string source)
		{
			return source.SmartSplit(",");
		}

		public static string[] SmartSplit(this string source, params string[] delim)
		{
			if (String.IsNullOrEmpty(source))
				return new[] { source };

			return source
				.Split(delim, StringSplitOptions.RemoveEmptyEntries)
				.Where(s => s != null)
				.Select(s => s.Trim())
				.ToArray();
		}

		public static int[] SmartSplitInt32(this string source, params string[] delim)
		{
			return SmartSplit(source, delim).Select(Int32.Parse).ToArray();
		}

		public static long[] SmartSplitInt64(this string source, params string[] delim)
		{
			return SmartSplit(source, delim).Select(Int64.Parse).ToArray();
		}

		public static string CamelSplit(this string source)
		{
			if (String.IsNullOrEmpty(source))
				return String.Empty;

			var sb = new StringBuilder();

			foreach (var c in source)
			{
				if (Char.IsUpper(c))
					sb.Append(" ");

				sb.Append(c);
			}

			return sb.ToString().Trim();
		}
	}
}
