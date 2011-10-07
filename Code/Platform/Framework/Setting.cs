using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public class Setting
	{
		[DataMember(Order = 1)]
		public string Key { get; set; }

		[DataMember(Order = 2)]
		public string ClientId { get; set; }

		[DataMember(Order = 3)]
		public string Value { get; set; }
	}
}
