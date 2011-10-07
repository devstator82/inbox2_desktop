using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Channels.Exchange
{
	[Serializable]
	[Export(typeof(ChannelConfiguration))]
	public class ExchangeConfiguration : ChannelConfiguration
	{
		public ExchangeConfiguration()
		{
			InnerInputChannel = new Channel { Type = typeof(ExchangeClientChannel), Port = 80, IsSecured = false, MaxConcurrentConnections = 2 };
			InnerOutputChannel = new Channel { Type = typeof(ExchangeClientChannel), Port = 80, IsSecured = false, MaxConcurrentConnections = 2 };
			InnerContactsChannel = new Channel { Type = typeof(ExchangeClientChannel), Port = 80, IsSecured = false, MaxConcurrentConnections = 2 };
		}

		public override string DisplayName
		{
			get { return "Exchange"; }
		}

		public override DisplayStyle DisplayStyle
		{
			get { return DisplayStyle.Advanced; }
		}

		public override ChannelCharasteristics Charasteristics
		{
			get
			{
				var charasteristics = ChannelCharasteristics.Default;

				charasteristics.SupportsReadStates = true;
				charasteristics.SupportsLabels = true;

				return charasteristics;
			}
		}

		public override ChannelConfiguration Clone()
		{
			return this.DeepCopy(new XmlSerializer(typeof(ExchangeConfiguration)));
		}
	}
}