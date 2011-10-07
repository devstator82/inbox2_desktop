using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Inbox2.Framework.Collections;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Framework.Plugins.SharedControls
{
	public class ChannelGroup : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private readonly CustomCompositeCollection<SourceAddress> recipients;

		public string ChannelGroupName { get; private set; }

		public List<ChannelInstance> Channels { get; private set; }

		public bool IsVisible { get; private set; }

		public ChannelGroup(CustomCompositeCollection<SourceAddress> recipients, List<ChannelInstance> channels, string channelGroupName)
		{
			this.recipients = recipients;
			this.recipients.CollectionChanged += RecipientsCollection_Changed;

			Channels = channels;
			ChannelGroupName = channelGroupName;
			IsVisible = false;
		}

		void RecipientsCollection_Changed(object sender, EventArgs e)
		{
			// Recalculate visibility of this group
			IsVisible = recipients.Any(c => c.ChannelName == ChannelGroupName);

			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs("IsVisible"));
		}
	}
}
