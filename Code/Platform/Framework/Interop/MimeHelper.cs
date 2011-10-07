using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Inbox2.Platform.Logging;
using Microsoft.Win32;

namespace Inbox2.Platform.Framework.Interop
{
	public static class MimeHelper
	{
		public static string GetMimeType(string filename)
		{
			string mime = "application/octetstream";
			string ext = Path.GetExtension(filename).ToLower();

			try
			{
				RegistryKey rk = Registry.ClassesRoot.OpenSubKey(ext);

				if (rk != null && rk.GetValue("Content Type") != null)
					mime = rk.GetValue("Content Type").ToString();
			}
			catch (Exception ex)
			{
				Logger.Warn("An error occured while trying to read mime-type from registry. Exception = {0}", LogSource.Storage, ex);
			}

			return mime;
		}
	}
}
