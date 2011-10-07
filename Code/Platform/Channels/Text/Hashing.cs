using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Channels.Text
{
	public static class Hashing
	{
		public static string ComputeSHA512(string s)
		{
			if (string.IsNullOrEmpty(s)) throw new ArgumentNullException();
			byte[] buffer = System.Text.Encoding.UTF8.GetBytes(s);
			
			buffer = System.Security.Cryptography.SHA512Managed.Create().ComputeHash(buffer);
			
			return Convert.ToBase64String(buffer).Substring(0, 86); // strip padding
		}
	}
}
