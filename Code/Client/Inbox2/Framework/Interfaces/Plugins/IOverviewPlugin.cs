using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Inbox2.Framework.Interfaces.Enumerations;

namespace Inbox2.Framework.Interfaces.Plugins
{
	public interface IOverviewPlugin : IViewPlugin
	{
		string Header { get; }

		ImageSource Icon { get; }

		WellKnownView WellKnownView { get; }
	}
}
