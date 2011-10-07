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

namespace Inbox2.Channels.AOL
{
	[Serializable]
	[Export(typeof(ChannelConfiguration))]
	public class AolConfiguration : ChannelConfiguration
	{
		public AolConfiguration()
		{
			InnerInputChannel = new Channel { Type = typeof(Imap2ClientChannel), Hostname = "imap.aol.com", Port = 143, IsSecured = false, MaxConcurrentConnections = 1 };
            InnerOutputChannel = new Channel { Type = typeof(SmtpClientChannel), Hostname = "smtp.aol.com", Port = 587, IsSecured = false, MaxConcurrentConnections = 1 };
		}

		public override string DisplayName
		{
			get { return "AOL"; }
		}

		public override string DefaultDomain
		{
			get { return "aol.com"; }
		}

		public override int PreferredSortOrder
		{
			get { return 30; }
		}

		public override ChannelCharasteristics Charasteristics
		{
			get
			{
				var charasteristics = ChannelCharasteristics.Default;

				charasteristics.SupportsReadStates = true;
				charasteristics.CanCustomize = true;

				return charasteristics;
			}
		}

		public override ChannelConfiguration Clone()
		{
			return this.DeepCopy(new XmlSerializer(typeof(AolConfiguration)));
		}
	}
}
