using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Inbox2.Channels.Imap2;
using Inbox2.Channels.Smtp;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Framework.Extensions;

namespace Inbox2.Channels.GMail
{
	[Serializable]
	[Export(typeof(ChannelConfiguration))]
	public class GMailConfiguration : ChannelConfiguration
	{
		public GMailConfiguration()
		{
			InnerInputChannel = new Channel { Type = typeof(Imap2ClientChannel), Hostname = "imap.gmail.com", Port = 993, IsSecured = true, MaxConcurrentConnections = 1 };
			InnerOutputChannel = new Channel { Type = typeof(SmtpClientChannel), Hostname = "smtp.gmail.com", Port = 465, IsSecured = true, MaxConcurrentConnections = 1 };
			InnerContactsChannel = new Channel { Type = typeof(GoogleContactsChannel) };
			InnerCalendarChannel = new Channel { Type = typeof(GoogleCalendarChannel) };
		}

		public override string DisplayName
		{
			get { return "GMail"; }
		}

		public override string DefaultDomain
		{
			get { return "gmail.com"; }
		}

		public override int PreferredSortOrder
		{
			get { return 10; }
		}		

		public override ChannelCharasteristics Charasteristics
		{
			get
			{
				var charasteristics = ChannelCharasteristics.Default;

				charasteristics.SupportsReadStates = true;
				charasteristics.SupportsLabels = true;
				charasteristics.CanCustomize = true;

				return charasteristics;
			}
		}

		public override ChannelConfiguration Clone()
		{
			return this.DeepCopy(new XmlSerializer(typeof(GMailConfiguration)));
		}
	}
}
