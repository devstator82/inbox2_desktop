using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Inbox2.Framework.Interfaces.Enumerations;

namespace Inbox2.Framework.Interfaces.Plugins
{
	public interface IToolbarPlugin
	{
		ToolbarAlignment ToolbarAlignment { get; }

		UIElement CreateToolbarElement();
	}
}
