using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Plugins.Contacts.Helpers
{
    [DataContract]
    public class ViewContactDataHelper
    {
        [DataMember]
        public long PersonId { get; set; }
    }
}
