using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Channels.Yammer
{
	[Serializable]
	[Export(typeof(ChannelConfiguration))]
	public class YammerConfiguration : ChannelConfiguration
	{
		public YammerConfiguration()
		{
			InnerInputChannel = new Channel { Type = typeof(YammerClientChannel) };
			InnerContactsChannel = new Channel { Type = typeof(YammerClientChannel) };
			InnerStatusUpdatesChannel = new Channel { Type = typeof(YammerClientChannel) };
		}

		public override DisplayStyle DisplayStyle
		{
			get { return DisplayStyle.RedirectWithPin; }
		}

		public override string DisplayName
		{
			get { return "Yammer"; }
		}

		public override IWebRedirectBuilder RedirectBuilder
		{
			get { return new YammerRedirectBuilder(); }
		}

		public override Platform.Channels.ChannelProfileInfoBuilder ProfileInfoBuilder
		{
			get { return new YammerProfileInfoBuilder(); }
		}

		public override ChannelCharasteristics Charasteristics
		{
			get
			{
				return new ChannelCharasteristics
			       	{
						SupportsPrivateMessage = false,
			       		SupportsStatusUpdates = true,
			       		SupportsProfiles = true,
						SupportsStatusUpdatesReply = true,
						MaxBodyChars = -1						
			       	};
			}
		}		

		public override ChannelConfiguration Clone()
		{
			return this.DeepCopy(new XmlSerializer(typeof(YammerConfiguration)));
		}
	}
}
