using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi
{
	[DataContract(Namespace = Settings.DefaultNamespace, Name = "Conversation")]
	public class ConversationHeader
	{
		[DataMember(EmitDefaultValue = false, Order = 1)]
		public string ConversationIdentifier { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 2)]
		public string Subject { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 3)]
		public string Participants { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 4)]
		public int TotalMessageCount { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 5)]
		public long SortDate { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 6)]
		public bool IsLast { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 7)]
		public bool Inbox { get; set; }
	}
}
