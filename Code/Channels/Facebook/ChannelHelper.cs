using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using Inbox2.Channels.Facebook.REST;

namespace Inbox2.Channels.Facebook
{
	internal static class ChannelHelper
	{		
		internal static IFacebookClient BuildChannel()
		{
			EndpointAddress address = new EndpointAddress("http://api.facebook.com/restserver.php");
			WebHttpBinding binding = new WebHttpBinding(WebHttpSecurityMode.None);

			binding.ReaderQuotas.MaxDepth = 2147483647;
			binding.ReaderQuotas.MaxStringContentLength = 2147483647;
			binding.ReaderQuotas.MaxArrayLength = 2147483647;
			binding.ReaderQuotas.MaxBytesPerRead = 2147483647;
			binding.ReaderQuotas.MaxNameTableCharCount = 2147483647;
			binding.MaxReceivedMessageSize = Int32.MaxValue;

			ChannelFactory<IFacebookClient> cf = new ChannelFactory<IFacebookClient>(binding, address);
			cf.Endpoint.Behaviors.Add(new WebHttpBehavior());

			return cf.CreateChannel();
		}

		internal static DateTime ConvertFromUnixTimestamp(double timestamp)
		{
			DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return origin.AddSeconds(timestamp);
		}
	}
}
