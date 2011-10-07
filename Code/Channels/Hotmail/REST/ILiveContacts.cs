using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Inbox2.Channels.Hotmail.REST
{
    [ServiceContract]
    [XmlSerializerFormat]
    public interface ILiveContacts
    {
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Wrapped,
            //RequestFormat = WebMessageFormat.Xml,
            ResponseFormat = WebMessageFormat.Xml)
        ]
        Stream Contacts();
    }
}