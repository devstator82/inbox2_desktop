using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Channels.LinkedIn
{
	[Serializable]
	[Export(typeof(ChannelConfiguration))]
	public class LinkedInConfiguration : ChannelConfiguration
	{
		public LinkedInConfiguration()
		{
			InnerInputChannel = new Channel { Type = typeof(LinkedInClientChannel) };
			InnerContactsChannel = new Channel { Type = typeof(LinkedInClientChannel) };
			InnerStatusUpdatesChannel = new Channel { Type = typeof(LinkedInClientChannel) };
		}

        public override DisplayStyle DisplayStyle
        {
            get { return DisplayStyle.Redirect; }
        }

		public override string DisplayName
		{
			get { return "LinkedIn"; }
		}

        public override IWebRedirectBuilder RedirectBuilder
        {
            get { return new LinkedInRedirectBuilder(); }
        }

		public override ChannelProfileInfoBuilder ProfileInfoBuilder
		{
			get { return new LinkedInProfileInfoBuilder(); }
		}

		public override ChannelCharasteristics Charasteristics
		{
			get
			{
				var charasteristics = base.Charasteristics;

				charasteristics.SupportsEmail = false;
				charasteristics.SupportsHtml = false;
				charasteristics.SupportsPrivateMessage = false;
				charasteristics.SupportsPublicMessage = true;
				charasteristics.CanSendFiles = false;
				charasteristics.SupportsReadStates = false;
				charasteristics.SupportsProfiles = true;
                charasteristics.SupportsStatusUpdates = true;
				charasteristics.SupportsStatusUpdatesReply = false;

				return charasteristics;
			}
		}

		public override ChannelConfiguration Clone()
		{
			return this.DeepCopy(new XmlSerializer(typeof(LinkedInConfiguration)));
		}
	}
}
