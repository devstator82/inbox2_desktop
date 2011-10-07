using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Inbox2.Platform.Channels.Extensions
{
	/// <summary>
	/// From: http://www.codeproject.com/KB/recipes/rfc2822-date-parser.aspx
	/// </summary>
	public static class DateTimeExtensions
	{
		public static DateTime FromRFC2822String(string rfc2822date)
		{
			// First try to parse using regular .net code, if that fails try custom solution
			DateTime netDateTime;

			if (DateTime.TryParse(rfc2822date, out netDateTime))
				return netDateTime;

			string tmp;
			string[] resp;
			string dayName;
			string dpart;
			string hour, minute;
			string timeZone;
			System.DateTime dt = System.DateTime.Now;
			//--- strip comments
			//--- XXX : FIXME : how to handle nested comments ?
			tmp = Regex.Replace(rfc2822date, "(\\([^(].*\\))", "");

			// strip extra white spaces
			tmp = Regex.Replace(tmp, "\\s+", " ");
			tmp = Regex.Replace(tmp, "^\\s+", "");
			tmp = Regex.Replace(tmp, "\\s+$", "");

			// extract week name part
			resp = tmp.Split(new char[] { ',' }, 2);
			if (resp.Length == 2)
			{
				// there's week name
				dayName = resp[0];
				tmp = resp[1];
			}
			else dayName = "";

			try
			{
				// extract date and time
				int pos = tmp.LastIndexOf(" ");
				if (pos < 1) throw new FormatException("probably not a date");
				dpart = tmp.Substring(0, pos);
				timeZone = tmp.Substring(pos + 1);
				dt = Convert.ToDateTime(dpart);

				// check weekDay name
				// this must be done befor convert to GMT 
				if (dayName != string.Empty)
				{
					if ((dt.DayOfWeek == DayOfWeek.Friday && dayName != "Fri") ||
						(dt.DayOfWeek == DayOfWeek.Monday && dayName != "Mon") ||
						(dt.DayOfWeek == DayOfWeek.Saturday && dayName != "Sat") ||
						(dt.DayOfWeek == DayOfWeek.Sunday && dayName != "Sun") ||
						(dt.DayOfWeek == DayOfWeek.Thursday && dayName != "Thu") ||
						(dt.DayOfWeek == DayOfWeek.Tuesday && dayName != "Tue") ||
						(dt.DayOfWeek == DayOfWeek.Wednesday && dayName != "Wed")
						)
						throw new FormatException("invalide week of day");
				}

				// adjust to localtime
				if (Regex.IsMatch(timeZone, "[+\\-][0-9][0-9][0-9][0-9]"))
				{
					// it's a modern ANSI style timezone
					int factor = 0;
					hour = timeZone.Substring(1, 2);
					minute = timeZone.Substring(3, 2);
					if (timeZone.Substring(0, 1) == "+") factor = -1;
					else if (timeZone.Substring(0, 1) == "-") factor = +1;
					else throw new FormatException("incorrect tiem zone");
					dt = dt.AddHours(factor * Convert.ToInt32(hour));
					dt = dt.AddMinutes(factor * Convert.ToInt32(minute));
				}
				else
				{
					// it's a old style military time zone ?
					switch (timeZone)
					{
						case "A": dt = dt.AddHours(1); break;
						case "B": dt = dt.AddHours(2); break;
						case "C": dt = dt.AddHours(3); break;
						case "D": dt = dt.AddHours(4); break;
						case "E": dt = dt.AddHours(5); break;
						case "F": dt = dt.AddHours(6); break;
						case "G": dt = dt.AddHours(7); break;
						case "H": dt = dt.AddHours(8); break;
						case "I": dt = dt.AddHours(9); break;
						case "K": dt = dt.AddHours(10); break;
						case "L": dt = dt.AddHours(11); break;
						case "M": dt = dt.AddHours(12); break;
						case "N": dt = dt.AddHours(-1); break;
						case "O": dt = dt.AddHours(-2); break;
						case "P": dt = dt.AddHours(-3); break;
						case "Q": dt = dt.AddHours(-4); break;
						case "R": dt = dt.AddHours(-5); break;
						case "S": dt = dt.AddHours(-6); break;
						case "T": dt = dt.AddHours(-7); break;
						case "U": dt = dt.AddHours(-8); break;
						case "V": dt = dt.AddHours(-9); break;
						case "W": dt = dt.AddHours(-10); break;
						case "X": dt = dt.AddHours(-11); break;
						case "Y": dt = dt.AddHours(-12); break;
						case "Z":
						case "UT":
						case "GMT": break;    // It's UTC
						case "EST": dt = dt.AddHours(5); break;
						case "EDT": dt = dt.AddHours(4); break;
						case "CST": dt = dt.AddHours(6); break;
						case "CDT": dt = dt.AddHours(5); break;
						case "MST": dt = dt.AddHours(7); break;
						case "MDT": dt = dt.AddHours(6); break;
						case "PST": dt = dt.AddHours(8); break;
						case "PDT": dt = dt.AddHours(7); break;
					}
				}
			}
			catch (Exception e)
			{
				throw new FormatException(string.Format("Invalide date:{0}:{1}", e.Message, rfc2822date));
			}
			return dt;
		}

		public static string ToRelativeTime(this DateTime date)
		{
			DateTime now = DateTime.Now;
			TimeSpan span = now - date;

			// After 7 days show the full date
			if (span.TotalDays > 3)
			{
				if (date.Year != DateTime.Now.Year)
					return date.ToString("ddd d MMM yyyy");
				else
					date.ToString("ddd d MMM");
			}

			if (span < TimeSpan.FromSeconds(0))
			{
				if (span.TotalSeconds < -60)
				{
					return String.Format("in {0} min", Math.Abs(span.TotalMinutes).ToString("##.#"));
				}
				else
				{
					return String.Format("in {0} sec", Math.Abs(span.TotalSeconds).ToString("##"));
				}
			}

			if (span <= TimeSpan.FromSeconds(60))
			{
				return span.Seconds + " sec ago";
			}
			else if (span <= TimeSpan.FromMinutes(60))
			{
				if (span.Minutes > 1)
				{
					return span.Minutes + " min ago";
				}
				else
				{
					return "a minute ago";
				}
			}
			else if (span <= TimeSpan.FromHours(24))
			{
				if (span.Hours > 1)
				{
					return span.Hours + " hrs ago";
				}
				else
				{
					return "an hour ago";
				}
			}
			else
			{
				if (span.Days > 1)
				{
					return span.Days + " days ago";
				}
				else
				{
					return "a day ago";
				}
			}
		}

		/// <summary>
		/// From: http://stackoverflow.com/questions/249760/how-to-convert-unix-timestamp-to-datetime-and-vice-versa
		/// </summary>
		/// <param name="unixTimeStamp"></param>
		/// <returns></returns>
		public static DateTime ToUnixTime(this long unixTimeStamp)
		{
			// Unix timestamp is seconds past epoch
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return epoch.AddSeconds(unixTimeStamp).ToLocalTime();
		}

		public static long ToUnixTime(this DateTime dateTime)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
			var ts = dateTime.Subtract(epoch);

			return ((((((ts.Days * 24) + ts.Hours) * 60) + ts.Minutes) * 60) + ts.Seconds);
		}
	}
}