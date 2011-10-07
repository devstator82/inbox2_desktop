using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public class Sender
	{
		[DataMember(EmitDefaultValue = false, Order = 1)]
		public string SenderKey { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 2)]
		public string DisplayName { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 3)]
		public string Address { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 4)]
		public string AvatarUrl { get; set; }

		public override string ToString()
		{
			return new SourceAddress(Address, DisplayName, AvatarUrl).ToString();
		}
	}
}
