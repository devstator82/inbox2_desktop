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
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Channels.Other
{
    [Serializable]
    [Export(typeof(ChannelConfiguration))]
    public class OtherConfiguration : ChannelConfiguration
    {
        public OtherConfiguration()
        {
            InnerInputChannel = new Channel { MaxConcurrentConnections = 1 };
            InnerOutputChannel = new Channel { Type = typeof(SmtpClientChannel), MaxConcurrentConnections = 1 };
        }

        public override string DisplayName
        {
            get { return "Other"; }
        }

        public override DisplayStyle DisplayStyle
        {
            get { return DisplayStyle.Other; }
        }

		public override ChannelCharasteristics Charasteristics
		{
			get
			{
				var baseChars = ChannelCharasteristics.Default;

				if (InnerInputChannel != null)
					baseChars.SupportsReadStates = 
						(InnerInputChannel.TypeSurrogate != null && InnerInputChannel.Type == typeof(Imap2ClientChannel));

				return baseChars;
			}
		}

        public override ChannelConfiguration Clone()
        {
            return this.DeepCopy(new XmlSerializer(typeof(OtherConfiguration)));
        }
    }
}
