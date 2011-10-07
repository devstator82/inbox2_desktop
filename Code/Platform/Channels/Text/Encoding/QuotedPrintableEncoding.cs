using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
// -----------------------------------------------------------------------
//
//   Copyright (C) 2003-2006 Angel Marin
// 
//   This file is part of SharpMimeTools
//
//   SharpMimeTools is free software; you can redistribute it and/or
//   modify it under the terms of the GNU Lesser General Public
//   License as published by the Free Software Foundation; either
//   version 2.1 of the License, or (at your option) any later version.
//
//   SharpMimeTools is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//   Lesser General Public License for more details.
//
//   You should have received a copy of the GNU Lesser General Public
//   License along with SharpMimeTools; if not, write to the Free Software
//   Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
//
// -----------------------------------------------------------------------

namespace Inbox2.Platform.Channels.Text.Encoding
{
	/// <summary>
	/// This class is based on the QuotedPrintable class written by Bill Gearhart
	/// found at http://www.aspemporium.com/classes.aspx?cid=6
	/// </summary>
	public static class QuotedPrintableEncoding
	{
		private const string Equal = "=";

		private const string HexPattern = "(\\=([0-9A-F][0-9A-F]))";

		public static string Decode(string contents)
		{
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}

			QuotedPrintable2Unicode(System.Text.Encoding.Default, ref contents);

			return contents;

			//using (StringWriter writer = new StringWriter())
			//{
			//    using (StringReader reader = new StringReader(contents))
			//    {
			//        string line;
			//        while ((line = reader.ReadLine()) != null)
			//        {
			//            /*remove trailing line whitespace that may have
			//             been added by a mail transfer agent per rule
			//             #3 of the Quoted Printable section of RFC 1521.*/
			//            line.TrimEnd();

			//            if (line.EndsWith(Equal))
			//            {
			//                string decodedLine = DecodeLine(line);

			//                writer.Write(decodedLine);
			//            } //handle soft line breaks for lines that end with an "="
			//            else
			//            {
			//                string decodedLine = DecodeLine(line);

			//                writer.WriteLine(decodedLine);
			//            }
			//        }
			//    }
			//    writer.Flush();

			//    return writer.ToString();
			//}
		}

		private static string DecodeLine(string line)
		{
			if (line == null)
			{
				throw new ArgumentNullException("line");
			}

			Regex hexRegex = new Regex(HexPattern, RegexOptions.IgnoreCase);

			return hexRegex.Replace(line, new MatchEvaluator(HexMatchEvaluator));
		}

		private static string HexMatchEvaluator(Match m)
		{
			int dec = Convert.ToInt32(m.Groups[2].Value, 16);
			char character = Convert.ToChar(dec);
			return character.ToString();
		}

		/// <summary>
		/// Decode rfc 2047 definition of quoted-printable
		/// </summary>
		/// <param name="enc"><see cref="System.Text.Encoding" /> to use</param>
		/// <param name="orig"><c>string</c> to decode</param>
		public static void QuotedPrintable2Unicode(System.Text.Encoding enc, ref String orig)
		{
			if (enc == null || orig == null)
				return;

			StringBuilder decoded = new StringBuilder(orig);
			int i = 0;
			System.String hexNumber;
			System.Byte[] ch = new System.Byte[1];
			while (i < decoded.Length - 2)
			{
				System.String decodedItem = null;
				if (decoded[i] == '=')
				{
					hexNumber = decoded.ToString(i + 1, 2);
					if (hexNumber.Equals(Environment.NewLine))
					{
						decodedItem = System.String.Empty;
						// Do not replace 3D(=)
					}
					else if (hexNumber.ToUpper().Equals("3D"))
					{
						decodedItem = null;
					}
					else
					{
						try
						{
							//TODO: this ugly workaround should disapear
							ch[0] = System.Convert.ToByte(hexNumber, 16);
							decodedItem = enc.GetString(ch);
						}
						catch (System.Exception) { }
					}
					if (decodedItem != null)
						decoded.Replace("=" + hexNumber, decodedItem);
				}
				if (decodedItem != null)
					i += decodedItem.Length;
				else
					i++;
			}
			decoded.Replace("=3D", "=");
			decoded.Replace("=3d", "=");
			orig = decoded.ToString();
			return;
		}
	}
}