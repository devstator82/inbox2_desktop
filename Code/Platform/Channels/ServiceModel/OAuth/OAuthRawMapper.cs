using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;

namespace Inbox2.Platform.Channels.ServiceModel.OAuth
{
	public class OAuthRawMapper : WebContentTypeMapper
	{
		public override WebContentFormat GetMessageFormatForContentType(string contentType)
		{
			return WebContentFormat.Raw; // always
		}

		public static Binding GetBinding(bool useHttps)
		{
			CustomBinding binding = new CustomBinding();
			binding.Elements.Add(new OAuthInterceptorBindingElement());
			binding.Elements.Add(new WebMessageEncodingBindingElement());

			if (useHttps)
				binding.Elements.Add(new HttpsTransportBindingElement { ManualAddressing = true, MaxReceivedMessageSize = (1024 * 1024) * 3 }); // MaxReceivedMessageSize = 3 Mb
			else
				binding.Elements.Add(new HttpTransportBindingElement { ManualAddressing = true, MaxReceivedMessageSize = (1024 * 1024) * 3 }); // MaxReceivedMessageSize = 3 Mb

			WebMessageEncodingBindingElement element = binding.Elements.Find<WebMessageEncodingBindingElement>();
			element.ContentTypeMapper = new OAuthRawMapper();

			return binding;
		}
	}
}
