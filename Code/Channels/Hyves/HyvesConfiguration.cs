using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Channels.Hyves
{
	[Export(typeof(ChannelConfiguration))]
	[Serializable]
	public class HyvesConfiguration : ChannelConfiguration
	{
		public HyvesConfiguration()
		{
			InnerInputChannel = new Channel { Type = typeof(HyvesClientChannel), Port = 80, IsSecured = false, MaxConcurrentConnections = 1 };
			InnerOutputChannel = new Channel { Type = typeof(HyvesClientChannel), Port = 80, IsSecured = false, MaxConcurrentConnections = 1 };
			InnerContactsChannel = new Channel { Type = typeof(HyvesClientChannel), Port = 80, IsSecured = false, MaxConcurrentConnections = 1 };
		}

		public override DisplayStyle DisplayStyle
		{
			get { return DisplayStyle.Redirect; }
		}

		public override string DisplayName
		{
			get { return "Hyves"; }
		}

		public override IWebRedirectBuilder RedirectBuilder
		{
			get { return new HyvesRedirectBuilder(); }
		}

		public override ChannelCharasteristics Charasteristics
		{
			get
			{
				var actions = base.Charasteristics;

				actions.SupportsEmail = false;
				actions.SupportsHtml = false;
				actions.SupportsPublicMessage = true;
				actions.SupportsMobileMessage = true;
				actions.CanSendFiles = false;
				actions.CanReply = "/Settings/Channels/Codebase".AsKey("") == "cloud";
				actions.SupportsReadStates = false;

				return actions;
			}
		}

		public override ChannelConfiguration Clone()
		{
			return this.DeepCopy(new XmlSerializer(typeof(HyvesConfiguration)));
		}
	}
}
