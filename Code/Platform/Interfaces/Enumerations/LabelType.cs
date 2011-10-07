using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Platform.Interfaces.Enumerations
{
	[DataContract]
	public enum LabelType : short
	{
		[EnumMember(Value = "0")]
		Custom = 0,

		[EnumMember(Value = "1")]
		Todo = 1,

		[EnumMember(Value = "2")]
		WaitingFor = 2,

		[EnumMember(Value = "3")]
		Someday = 3,		
	}
}
