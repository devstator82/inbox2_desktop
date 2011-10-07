using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Plugins.ColumnSearch.Controls;

namespace Inbox2.Plugins.ColumnSearch
{
	public class ColumnSearchState : PluginStateBase
	{
		public DetailsView LastDetailsView { get; set; }
	}
}
