using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public class GetSyncStateResponse
	{
		[DataMember(EmitDefaultValue = false, Order = 1)]
		public List<MessageHeader> Messages { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 2)]
		public List<PersonHeader> Persons { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 3)]
		public List<UserStatusHeader> StatusUpdates { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 4)]
		public List<ChannelProgress> Progress { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 5)]
		public long CurrentSyncDate { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 6)]
		public int TotalPages { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 7)]
		public string ConfigHash { get; set; }
	}
}