using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Inbox2.Channels.Pop3;
using Inbox2.Channels.Smtp;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Framework.Extensions;

namespace Inbox2.Channels.Hotmail
{
	[Serializable]
	[Export(typeof(ChannelConfiguration))]
	public class HotmailConfiguration : ChannelConfiguration
	{
		public HotmailConfiguration()
		{
			InnerInputChannel = new Channel { Type = typeof(Pop3ClientChannel), Hostname = "pop3.live.com", Port = 995, IsSecured = true, MaxConcurrentConnections = 1 };
			InnerOutputChannel = new Channel { Type = typeof(SmtpClientChannel), Hostname = "smtp.live.com", Port = 25, IsSecured = false, MaxConcurrentConnections = 1 };
            //InnerContactsChannel = new Channel { Type = typeof(HotmailContactsChannel) };
        }

		public override string DefaultDomain
		{
			get { return "hotmail.com"; }
		}

		public override string DisplayName
		{
			get { return "Hotmail"; }
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
			return this.DeepCopy(new XmlSerializer(typeof(HotmailConfiguration)));
		}
	}
}
