using System;
using System.Runtime.Serialization;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi
{
	[DataContract(Namespace = Settings.DefaultNamespace, Name="Channel")]
	public class ChannelHeader
	{
		[DataMember(EmitDefaultValue = false, Order = 1)]
		public string ChannelKey { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 2)]
		public string DisplayName { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 3)]
		public string CustomSignature { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 4)]
		public string AvatarUrl { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 5)]
		public string SmallIconUrl { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 6)]
		public string MediumIconUrl { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 7)]
		public string LargeIconUrl { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 8)]
		public Server Incoming { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 9)]
		public Server Outgoing { get; set; }
	}
}
