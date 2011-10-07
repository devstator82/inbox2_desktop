using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Inbox2.Platform.Channels.Entities
{
	[DataContract]
	public class ChannelContact
	{
		[DataMember(Order = 1)]
		public ChannelPerson Person { get; private set; }

		[DataMember(Order = 2)]
		public ChannelProfile Profile { get; private set; }

		[DataMember(Order = 3)]
		public bool IsSoft { get; set; }

		public ChannelContact()
		{
			Person = new ChannelPerson();
			Profile = new ChannelProfile();
		}
	}
}