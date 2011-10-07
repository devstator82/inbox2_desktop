using System;
using System.ComponentModel.Composition;
using System.Xml.Serialization;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Channels.Facebook
{
	[Serializable]
	[Export(typeof(ChannelConfiguration))]
	public class FacebookConfiguration : ChannelConfiguration
	{
		public FacebookConfiguration()
		{
			InnerInputChannel = new Channel { Type = typeof(FacebookClientChannel), Port = 80, IsSecured = false, MaxConcurrentConnections = 2 };
			InnerOutputChannel = new Channel { Type = typeof(FacebookClientChannel), Port = 80, IsSecured = false, MaxConcurrentConnections = 1 };
			InnerContactsChannel = new Channel { Type = typeof(FacebookClientChannel), Port = 80, IsSecured = false, MaxConcurrentConnections = 1 };
			InnerStatusUpdatesChannel = new Channel { Type = typeof(FacebookClientChannel), Port = 80, IsSecured = false, MaxConcurrentConnections = 1 };
		}

		public override string DisplayName
		{
			get { return "Facebook"; }
		}

		public override DisplayStyle DisplayStyle
		{
			get { return DisplayStyle.FbConnect; }
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
				charasteristics.CanReply = false;
				charasteristics.SupportsReadStates = false;
				charasteristics.SupportsStatusUpdates = true;
				charasteristics.SupportsProfiles = true;
				charasteristics.SupportsStatusUpdatesReply = true;

				return charasteristics;
			}
		}

		public override IWebRedirectBuilder RedirectBuilder
		{
			get { return new FacebookRedirectBuilder(); }
		}

		public override ChannelProfileInfoBuilder ProfileInfoBuilder
		{
			get { return new FacebookProfileInfoBuilder(); }
		}

		public override ChannelConfiguration Clone()
		{
			return this.DeepCopy(new XmlSerializer(typeof(FacebookConfiguration)));
		}
	}
}
