using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace Inbox2.Channels.Contacts.REST
{
	internal class RawMapper : WebContentTypeMapper
	{
		public override WebContentFormat GetMessageFormatForContentType(string contentType)
		{
			return WebContentFormat.Raw; // always
		}

		internal static Binding GetBinding()
		{
			CustomBinding result = new CustomBinding(new WebHttpBinding());
			WebMessageEncodingBindingElement element = result.Elements.Find<WebMessageEncodingBindingElement>();
			element.ContentTypeMapper = new RawMapper();
			return result;
		}
	}
}
