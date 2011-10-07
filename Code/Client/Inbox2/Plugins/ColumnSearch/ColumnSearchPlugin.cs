using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Plugins.ColumnSearch.Helpers;

namespace Inbox2.Plugins.ColumnSearch
{
	[Export(typeof(PluginPackage))]
	public class ColumnSearchPlugin : PluginPackage
	{
		private readonly ColumnSearchState state;
		private readonly Controller controller;

		public override string Name
		{
			get { return "ColumnSearch"; }
		}

		public override IStatePlugin State
		{
			get { return state; }
		}

		public override IDetailsViewPlugin DetailsView
		{
			get { return new PluginHelper(state); }
		}

		public ColumnSearchPlugin()
		{
			state = new ColumnSearchState();
			controller = new Controller(state);

			EventBroker.Subscribe<string>(AppEvents.RequestNewSearch, controller.RequestNewSearch);
		}
	}
}
