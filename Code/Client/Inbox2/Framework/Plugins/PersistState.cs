using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Framework.Plugins
{
	[DataContract]
	public class PersistState
	{
		[DataMember]
		public List<TabState> Tabs { get; private set; }

		[DataMember]
		public int SelectedTabIndex { get; set; }

		public PersistState()
		{
			Tabs = new List<TabState>();
		}
	}
}
