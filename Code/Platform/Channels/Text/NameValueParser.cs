using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Channels.Text
{
	public static class NameValueParser
	{
		public static NameValueCollection GetCollection(string source, params string[] delimiters)
		{
			string[] parts = source.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

			var col = new NameValueCollection();

			if (parts.Length == 0)
				return col;

			foreach (var part in parts)
			{
				string[] kv = part.Split('=');

				string key;
				string value;

				if (kv.Length == 2)
				{
					key = kv[0];
					value = kv[1];
				}
				else
				{
					key = kv[0];
					kv[0] = String.Empty;
					value = String.Join("", kv);
				}

				col.Add(key, value);
			}

			return col;
		}
	}
}
