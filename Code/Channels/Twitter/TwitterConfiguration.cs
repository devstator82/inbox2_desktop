using System;
using System.ComponentModel.Composition;
using System.Xml.Serialization;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Channels.Twitter
{
	[Serializable]
	[Export(typeof(ChannelConfiguration))]
	public class TwitterConfiguration : ChannelConfiguration
	{
		public TwitterConfiguration()
		{
			InnerInputChannel = new Channel { Type = typeof(TwitterClientChannel) };
			InnerOutputChannel = new Channel { Type = typeof(TwitterClientChannel) };
			InnerContactsChannel = new Channel { Type = typeof(TwitterClientChannel) };
			InnerStatusUpdatesChannel = new Channel { Type = typeof(TwitterClientChannel) };
		}

		public override DisplayStyle DisplayStyle
		{
			get { return DisplayStyle.Redirect; }
		}

		public override string DisplayName
		{
			get { return "Twitter"; }
		}

		public override ChannelCharasteristics Charasteristics
		{
			get
			{
				var charasteristics = base.Charasteristics;

				charasteristics.SupportsEmail = false;
				charasteristics.SupportsHtml = false;
				charasteristics.SupportsPublicMessage = true;
				charasteristics.SupportsSubject = false;
				charasteristics.SupportsStatusUpdates = true;
				charasteristics.CanSendFiles = false;
				charasteristics.SupportsReadStates = false;
				charasteristics.SupportsStatusUpdateMentions = true;
				charasteristics.SupportsStatusUpdatesSearch = true;
				charasteristics.SupportsStatusUpdatesReply = true;
				charasteristics.SupportsProfiles = true;
				charasteristics.MaxBodyChars = 140;

				return charasteristics;
			}
		}

		public override IWebRedirectBuilder RedirectBuilder
		{
			get { return new TwitterRedirectBuilder(); }
		}

		public override ChannelProfileInfoBuilder ProfileInfoBuilder
		{
			get { return new TwitterProfileInfoBuilder(); }
		}

		public override ChannelConfiguration Clone()
		{
			return this.DeepCopy(new XmlSerializer(typeof(TwitterConfiguration)));
		}
	}
}
