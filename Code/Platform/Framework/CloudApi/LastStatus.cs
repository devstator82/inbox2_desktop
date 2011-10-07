using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Framework.CloudApi.Enumerations;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public class LastStatus
	{
		[DataMember(EmitDefaultValue = false, Order = 1)]
		public long Posted { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 2)]
		public string Status { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 3)]
		public string ChannelName { get; set; }
	}
}
