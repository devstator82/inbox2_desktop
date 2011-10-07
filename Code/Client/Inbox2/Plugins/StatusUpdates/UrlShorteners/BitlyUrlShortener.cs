using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Logging;

namespace Inbox2.Plugins.StatusUpdates.UrlShorteners
{
	[Export(typeof(IUrlShortener))]
	public class BitlyUrlShortener : IUrlShortener
	{
		public string Name
		{
			get { return "bit.ly"; }
		}

		public string Shorten(string url)
		{
			try
			{				
				string bitlyUrl = string.Format("http://api.bit.ly/shorten?version=2.0.1&format=xml&longUrl={0}&login={1}&apiKey={2}",
					HttpUtility.UrlEncode(url), "waseem", "R_b36db4f298fcd25827e589aaadd0e0b6");

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(bitlyUrl);

				using (HttpWebResponse loWebResponse = (HttpWebResponse)request.GetResponse())
				using (var xmlStream = loWebResponse.GetResponseStream())
				{
					XDocument xmlFeed = XDocument.Parse(xmlStream.ReadString());

					var element = xmlFeed.Descendants("nodeKeyVal").First();
					var value =  element.Element("shortUrl").Value;

					if (String.IsNullOrEmpty(value))
					{
						Logger.Error("Unable to shorten url. Result element = {0}", LogSource.UI, element);

						return url;
					}

					return value;
				}
			}
			catch (Exception ex)
			{
				Logger.Error("An error has occured while trying to shorten url. Exception = {0}", LogSource.UI, ex);

				return url;
			}
		}
	}
}
