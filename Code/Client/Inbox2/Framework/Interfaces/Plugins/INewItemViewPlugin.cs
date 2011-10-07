using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Inbox2.Framework.Interfaces.Plugins
{
	public interface INewItemViewPlugin : IViewPlugin
	{
		ImageSource Icon { get; }

		string Header { get; }

		bool ForceSingle { get; }
	}
}
