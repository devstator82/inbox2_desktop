using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Platform.Interfaces.Enumerations
{
	[DataContract]
	public enum ResultCode
	{
		[EnumMember(Value = "Success")]
		Success = 0,

		[EnumMember(Value = "NotAvailable")]
		NotAvailable = 1,

		[EnumMember(Value = "Error")]
		Error = -1
	}
}
