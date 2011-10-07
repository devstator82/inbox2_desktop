using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Framework.Interfaces.Enumerations
{
	[DataContract]
	public enum ViewType
	{
		[EnumMember]
		Overview,

		[EnumMember]
		DetailsView,

		[EnumMember]
		NewItemView
	}
}
