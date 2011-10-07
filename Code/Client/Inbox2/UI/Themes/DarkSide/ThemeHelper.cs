using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Inbox2.UI.Themes.DarkSide
{
	internal static class ThemeHelper
	{
		public static string Version
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}
	}
}
