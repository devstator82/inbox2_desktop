using System;
using System.Net;
using ExchangeServicesWsdlClient;

namespace Inbox2.Channels.Exchange
{
	internal static class ChannelHelper
	{
		internal static ExchangeServiceBinding BuildChannel(string hostname, string username, string password)
		{
			// First, set up the binding to Exchange Web Services.
			ExchangeServiceBinding binding = new ExchangeServiceBinding();

			Uri epUri = new Uri(hostname);

			// Add prefix unless user specifies one himself
			if (!hostname.EndsWith(".asmx", StringComparison.InvariantCultureIgnoreCase))
				epUri = new Uri(epUri, "/EWS/Exchange.asmx");

			binding.Credentials = new NetworkCredential(username, password);
			binding.Url = epUri.ToString();
			binding.RequestServerVersionValue = new RequestServerVersion { Version = ExchangeVersionType.Exchange2007_SP1 };

			return binding;
		}
	}
}
