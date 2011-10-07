using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace Inbox2.Framework
{
	public class JoeCulture : IDisposable
	{
		private readonly CultureInfo culture;
		private readonly CultureInfo uiCulture;

		public JoeCulture()
		{
			culture = Thread.CurrentThread.CurrentCulture;
			uiCulture = Thread.CurrentThread.CurrentUICulture;

			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
		}

		public void Dispose()
		{
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = uiCulture;
		}
	}
}
