using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Platform.Channels.Entities
{
	[Serializable]
	[DataContract]
	public enum ProfileType
	{
		[EnumMember(Value = "1")]
		Default = 0,

		[EnumMember(Value = "2")]
		Social = 10,	
	}
}