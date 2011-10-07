using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Inbox2.Framework.Interfaces.Plugins
{
	public interface IViewPlugin
	{
		UIElement CreateView();
	}
}
