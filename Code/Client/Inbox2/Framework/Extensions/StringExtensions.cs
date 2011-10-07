using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Inbox2.Framework.Extensions
{
	public static class StringExtensions
	{
		/// <summary>
		/// Returns the given string as an instance of a SecureString.
		/// </summary>
		/// <param name="password"></param>
		/// <returns></returns>
		public static SecureString ToSecureString(this string password)
		{
			SecureString secureString = new SecureString();

			foreach (char c in password)
				secureString.AppendChar(c);

			return secureString;
		}

		/// <summary>
		/// Returns the clear-text password from the secure string.
		/// </summary>
		/// <param name="secureString">The secure string.</param>
		/// <returns></returns>
		public static string ToClearText(this SecureString secureString)
		{
			IntPtr ptr = Marshal.SecureStringToBSTR(secureString);

			return Marshal.PtrToStringUni(ptr);
		}
	}
}
