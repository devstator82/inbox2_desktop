using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Inbox2.Core.Configuration
{
	[XmlRoot("channels")]
	public class PreChannels
	{
		private static object _SyncLock = new object();
		private static List<PreChannel> _Channels;

		public static List<PreChannel> GetChannels()
		{
			if (!File.Exists("c:\\PreChannels.xml"))
				return null;

			if (_Channels == null)
			{
				lock (_SyncLock)
				{
					using (FileStream fs =
						new FileStream("c:\\PreChannels.xml", FileMode.Open, FileAccess.Read))
					{
						XmlSerializer ser = new XmlSerializer(typeof (PreChannels));
						PreChannels result = (PreChannels) ser.Deserialize(fs);

						_Channels = result.Channels;
					}
				}				
			}

			return _Channels;
		}


		[XmlElement("channel")]
		public List<PreChannel> Channels { get; set; }
	}

	
	public class PreChannel
	{
		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("hostname")]
		public string Hostname { get; set; }

		[XmlAttribute("username")]
		public string Username { get; set; }

		[XmlAttribute("password")]
		public string Password { get; set; }
	}
}
