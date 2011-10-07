using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Inbox2.Channels.Imap2;
using Inbox2.Channels.Pop3;
using Inbox2.Channels.Smtp;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Framework.Extensions;

namespace Inbox2.Channels.Yahoo
{
	[Serializable]
	[Export(typeof(ChannelConfiguration))]
	public class YahooConfiguration : ChannelConfiguration
	{
		public YahooConfiguration()
		{
			InnerInputChannel = new Channel { Type = typeof(Imap2ClientChannel), Hostname = "imap-ssl.mail.yahoo.com", Port = 993, IsSecured = true, MaxConcurrentConnections = 1 };
			InnerOutputChannel = new Channel { Type = typeof(SmtpClientChannel), Hostname = "smtp.mail.yahoo.com", Port = 465, IsSecured = true, MaxConcurrentConnections = 1 };
		}

		public override string DisplayName
		{
			get { return "Yahoo"; }
		}		

		public override string DefaultDomain
		{
			get { return "yahoo.com"; }
		}

		public override int PreferredSortOrder
		{
			get { return 20; }
		}

		public override ChannelCharasteristics Charasteristics
		{
			get
			{
				var charasteristics = base.Charasteristics;

				charasteristics.SupportsReadStates = false;
				charasteristics.CanCustomize = true;

				return charasteristics;
			}
		}

		public override ChannelConfiguration Clone()
		{
			return this.DeepCopy(new XmlSerializer(typeof(YahooConfiguration)));
		}
	}
}
