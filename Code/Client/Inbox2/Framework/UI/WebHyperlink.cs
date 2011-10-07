using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace Inbox2.Framework.UI
{
	public class WebHyperlink : Hyperlink
	{
		protected override void OnClick()
		{
			if (NavigateUri != null)
			{
				FocusHelper.Focus(Application.Current.MainWindow);

				new Process {StartInfo = new ProcessStartInfo(NavigateUri.ToString())}.Start();
			}

			base.OnClick();
		}
	}
}
