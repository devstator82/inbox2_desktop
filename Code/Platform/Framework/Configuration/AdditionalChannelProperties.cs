using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Inbox2.Platform.Framework.Configuration
{
	[XmlRoot("channelProperties")]
	public class AdditionalChannelProperties
	{
		[XmlArray("channels")]
		[XmlArrayItem("channel")]
		public List<ChannelProperties> Channels { get; set; }

		public static List<ChannelProperties> GetConfiguration()
		{
			var config = (AdditionalChannelProperties)ConfigurationManager.GetSection("channelProperties");

			return config.Channels;
		}
	}

	public class ChannelProperties
	{
		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("helpUrl")]
		public string HelpUrl { get; set; }

		[XmlAttribute("preferedColour")]
		public string PreferedColour { get; set; }

		[XmlAttribute("usernameHint")]
		public string UsernameHint { get; set; }

		[XmlAttribute("hostnameHint")]
		public string HostnameHint { get; set; }

		[XmlAttribute("viralUpdate")]
		public bool ViralUpdate { get; set; }
	}
}