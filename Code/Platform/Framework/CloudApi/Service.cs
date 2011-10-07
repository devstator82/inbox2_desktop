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
	public class Service
	{
		[DataMember(EmitDefaultValue = false, Order = 1)]
		public string ServiceType { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 2)]
		public string IconUrl { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 3)]
		public string AvatarUrl { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 5)]
		public string ProfileKey { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 6)]
		public string ProfileUrl { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 7)]
		public bool IsFriend { get; set; }
	}
}
