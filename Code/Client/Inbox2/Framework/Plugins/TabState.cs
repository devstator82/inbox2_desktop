using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Framework.Interfaces.Enumerations;

namespace Inbox2.Framework.Plugins
{
	[DataContract]
	public class TabState
	{
		[DataMember]
		public Type PluginType { get; set; }

		[DataMember]
		public ViewType ViewType { get; set; }

		[DataMember]
		public object DataInstance { get; set; }
	}
}