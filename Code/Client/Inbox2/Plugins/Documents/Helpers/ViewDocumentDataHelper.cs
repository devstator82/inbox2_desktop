using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Plugins.Documents.Helpers
{
	[DataContract]
	public class ViewDocumentDataHelper
	{
		[DataMember]
		public long DocumentId { get; set; }
	}
}
