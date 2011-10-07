using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using Inbox2.Framework.Interop;
using Inbox2.Framework.UI;

namespace Inbox2.UI.Launcher
{
	public static class Keyboard
	{
		private static Flipper _Flipper;

		public static void Initialize()
		{
			_Flipper = new Flipper(TimeSpan.FromMilliseconds(300), delegate { });

			InterceptKeys.Hook();
			InterceptKeys.KeyPressed += InterceptKeys_KeyPressed;
		}

		public static void Shutdown()
		{
			InterceptKeys.Unhook();
		}

		static void InterceptKeys_KeyPressed(object sender, EventArgs e)
		{
			// If the _flipper is not running, start it
			// Otherwise continue with showing the launcher
			if (!_Flipper.IsRunning)
			{
				_Flipper.Delay();
				return;
			}

			// Execute on UI thread
			Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate
			{
				LauncherWindow window = new LauncherWindow();

				window.Show();
				window.Topmost = true;
				window.Activate();
			});
		}
	}
}
