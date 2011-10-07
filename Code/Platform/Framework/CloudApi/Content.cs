using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public class Content
	{
		[DataMember(EmitDefaultValue = false, Order = 1)]
		public string Url { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 2)]
		public long ValidUtil { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 3)]
		public string ContentType { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 4)]
		public string PayLoad { get; set; }

		public DateTime ValidUntilDate
		{
			get
			{
				return ValidUtil.ToUnixTime();
			}
		}
	}
}
