using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Inbox2.Platform.Interfaces;

namespace Inbox2.Platform.Channels.Entities
{
	[Serializable]
	[DataContract]
	public class ChannelMetadata : IPersistXml
	{
		[DataMember(Order = 1)]
		public string i2mpMessageId { get; set; }

		[DataMember(Order = 2)]
		public string i2mpFlow { get; set; }

		[DataMember(Order = 3)]
		public string i2mpReference { get; set; }

		[DataMember(Order = 4)]
		public string i2mpSequence { get; set; }

		[DataMember(Order = 5)]
		public string i2mpRelation { get; set; }

		[DataMember(Order = 6)]
		public string i2mpRelationId { get; set; }
	}
}
