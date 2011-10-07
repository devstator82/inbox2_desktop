using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Plugins.ColumnSearch.Helpers;

namespace Inbox2.Plugins.ColumnSearch
{
	class Controller
	{
		private readonly ColumnSearchState state;

		public Controller(ColumnSearchState state)
		{
			this.state = state;
		}

		public void RequestNewSearch(string query)
		{
			ClientState.Current.ViewController.MoveTo(
				PluginsManager.Current.GetPlugin<ColumnSearchPlugin>().DetailsView,
				new ColumnSearchDataHelper { SearchQuery = query });
		}

		public void RequestUpdateSearch(string query)
		{
			state.LastDetailsView.LoadData(new ColumnSearchDataHelper { SearchQuery = query });
		}
	}
}
