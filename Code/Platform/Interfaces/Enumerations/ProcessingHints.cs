using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Framework.Interfaces.Enumerations
{
	[Serializable]
	[DataContract]
	public enum ProcessingHints
	{
		[EnumMember]
		Unset = -1,

		[EnumMember]
        None = 0,

        [EnumMember]
        QuickMessage = 1,

		[EnumMember]
		Ignore = 100,
	}
}
