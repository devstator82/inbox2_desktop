using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace Inbox2.Platform.Channels.ServiceModel
{
    public class RawMapper : WebContentTypeMapper
    {
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {
            return WebContentFormat.Raw; // always
        }

		public static CustomBinding GetBinding()
        {
            CustomBinding result = new CustomBinding(new WebHttpBinding());
            WebMessageEncodingBindingElement element = result.Elements.Find<WebMessageEncodingBindingElement>();
            element.ContentTypeMapper = new RawMapper();
            return result;
        }

        //Used by live contacts channel and Solr search and RelatedItems
		public static CustomBinding GetCustomBinding(Binding source)
        {
            CustomBinding result = new CustomBinding(source);
            WebMessageEncodingBindingElement element = result.Elements.Find<WebMessageEncodingBindingElement>();
            element.ContentTypeMapper = new RawMapper();
            return result;
        }
    }
}
