using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Platform.Channels;
using Inbox2.Plugins.StatusUpdates.Controls;

namespace Inbox2.Plugins.StatusUpdates.Helpers
{
	class ChannelSearchStreamToolbarPlugin : IToolbarPlugin
	{
		private readonly ChannelInstance channel;
        private ChannelSearchStreamToolbarElement element;

		public ChannelSearchStreamToolbarPlugin(ChannelInstance channel)
		{
			this.channel = channel;
		}

		public ToolbarAlignment ToolbarAlignment
		{
			get { return ToolbarAlignment.Left; }
		}

		public UIElement CreateToolbarElement()
		{
            if (element == null)
            {
                element = new ChannelSearchStreamToolbarElement(channel);
            }
			
            return element;
		}
	}
}
