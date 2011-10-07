using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Framework;

namespace Inbox2.Core.Configuration.Channels
{
	public class ChannelLoadHelper
	{
		[ImportMany]
		public List<ChannelConfiguration> AvailableChannels { get; set; }

		public ChannelLoadHelper()
		{
			using (new CodeTimer("Startup/LoadChannels"))
			{
				// Build MEF catalog of channels
				var catalog = new DirectoryCatalog(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "*.channels.*.dll");
				var container = new CompositionContainer(catalog);

				container.ComposeParts(this);

				// Do a reverse sort
				AvailableChannels.Sort(new Comparison<ChannelConfiguration>(
				                       	(left, right) => right.PreferredSortOrder.CompareTo(left.PreferredSortOrder)));
			}
		}
	}
}
