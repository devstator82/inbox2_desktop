using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework.Interfaces.Enumerations;

namespace Inbox2.Framework.Interfaces.Plugins
{
	public interface IPersistableTab : IControllableTab
	{
		PluginPackage Plugin { get; }

		ViewType ViewType { get; }

		void LoadData(object data);

		object SaveData();
	}
}
