using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public class Occupation
	{
		[DataMember(EmitDefaultValue = false, Order = 1)]
		public string Period { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 2)]
		public string Company { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 3)]
		public string Title { get; set; }

		public int ProviderPriority { get; set; }
	}
}
