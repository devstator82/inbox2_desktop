using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public class ChannelSearch
	{
		[DataMember(EmitDefaultValue = false, Order = 1)]
		public string ChannelName { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 2)]
		public string Keyword { get; set; }
	}
}
