using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Plugins.StatusUpdates.Helpers;

namespace Inbox2.Plugins.StatusUpdates
{
	[Export(typeof(PluginPackage))]
	public class StatusUpdatesSearchPlugin : PluginPackage
	{
		public override string Name
		{
			get { return "StatusUpdatesSearch"; }
		}

		public override IStatePlugin State
		{
			get { return StatusUpdatesState.Current; }
		}

		public override IEnumerable<IToolbarPlugin> ToolbarItems
		{
			get
			{
				if (SearchKeywordsHelper.HasSearchChannel())
				{
					var twitter = SearchKeywordsHelper.GetSearchChannel();

					yield return new ChannelSearchStreamToolbarPlugin(twitter);
				}
			}
		}
	}
}
