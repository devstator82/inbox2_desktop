using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Channels.Text
{
	public class PersonName
	{
		public string Firstname { get; set; }

		public string Lastname { get; set; }

		public override string ToString()
		{
			return String.Format("{0} {1}", Firstname, Lastname).Trim();
		}

		public static PersonName Parse(string fullname)
		{
			PersonName result = new PersonName();

			fullname = fullname.Trim();

			// No spaces
			if (fullname.IndexOf(" ") < 0)
			{
				result.Firstname = Capitalize(fullname);

				return result;
			}

			int comma = fullname.IndexOf(",");

			// Comma, split on comma
			if (comma > -1)
			{
				string[] commaSep = fullname.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

				if (commaSep.Length != 2)
				{
					result.Firstname = Capitalize(fullname);

					return result;
				}

				result.Firstname = Capitalize(commaSep[0]);
				result.Lastname = Capitalize(commaSep[1]);

				return result;
			}

			// No comma, split on space
			string[] parts = fullname.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length > 2)
			{
				// Something like [jean-luc jesuis] [leleur]
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < parts.Length - 1; i++)
					sb.Append(parts[i] + " ");

				result.Firstname = Capitalize(sb.ToString().Trim());
				result.Lastname = Capitalize(parts[parts.Length - 1]);
				
				return result;
			}

			result.Firstname = Capitalize(parts[0]);
			result.Lastname = Capitalize(parts[1]);

			return result;
		}

		static string Capitalize(string source)
		{
			if (String.IsNullOrEmpty(source))
				return source;

			if (source.Length == 1)
				return source.ToUpper();

			return String.Concat(source[0].ToString().ToUpper(), source.Substring(1).ToLower());
		}
	}
}
