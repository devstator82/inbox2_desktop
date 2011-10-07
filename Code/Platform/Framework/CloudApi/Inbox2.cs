using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Platform.Framework.CloudApi
{
	/// <summary>
	/// Container class; only used for serializing proto documentation.
	/// </summary>
	[DataContract(Namespace = "http://api.inbox2.com/v1/")]
	public class Inbox2
	{
		[DataMember(Order = 1, EmitDefaultValue = false)]
		public GetSyncStateResponse GetSyncStateResponse { get; set; }

		[DataMember(Order = 2, EmitDefaultValue = false)]
		public GetAccountResponse GetAccountResponse { get; set; }

		[DataMember(Order = 3, EmitDefaultValue = false)]
		public GetProfileResponse GetProfileResponse { get; set; }
	}
}
