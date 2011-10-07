using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Inbox2.Framework.Interfaces.Plugins
{
	public interface IStreamViewPlugin
	{
		ImageSource StreamIcon { get; }

		string Header { get; }

		UIElement CreateStreamView();

		bool CanSwitchToView { get; }

		void SwitchToView();
	}
}
