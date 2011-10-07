using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Logging;
using Logger=Inbox2.Platform.Logging.Logger;

namespace Inbox2.Platform.Channels.Web
{
	public class WebContentStreamHelper
	{
		private readonly string url;
		private readonly Action<HttpWebRequest> prepareRequest;

		public WebContentStreamHelper(string url)
		{
			this.url = url;
		}

		public WebContentStreamHelper(string url, Action<HttpWebRequest> prepareRequest)
		{
			this.url = url;
			this.prepareRequest = prepareRequest;
		}

		public MemoryStream GetContentStream()
		{
			HttpWebResponse response = null;

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

				if (prepareRequest != null)
					prepareRequest(request);

				response = (HttpWebResponse)request.GetResponse();

				MemoryStream ms = new MemoryStream();

				var stream = response.GetResponseStream();

				stream.CopyTo(ms);
				stream.Dispose();

				return ms;
			}
			catch (Exception ex)
			{
				if (response != null && response.StatusCode != HttpStatusCode.NotFound)
				{
					// Don't log 404
					Logger.Error("An error has occured while trying to retrieve the web resource, Exception = {0}", LogSource.Channel, ex);
				}

				return null;
			}
		}
		                    
		public static MemoryStream GetContentStream(string url)
		{
			return new WebContentStreamHelper(url).GetContentStream();
		}

		public static MemoryStream GetContentStream(string url, Action<HttpWebRequest> prepareRequest)
		{
			return new WebContentStreamHelper(url, prepareRequest).GetContentStream();
		}
	}
}
