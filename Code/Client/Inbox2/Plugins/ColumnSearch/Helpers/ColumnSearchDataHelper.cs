using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Plugins.ColumnSearch.Helpers
{
	[DataContract]
	public class ColumnSearchDataHelper
	{
		[DataMember]
		public string SearchQuery { get; set; }
	}
}
