using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi.Enumerations
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public enum Entity
	{
		[EnumMember(Value = "Message")]
		Message,

		[EnumMember(Value = "Document")]
		Document,

		[EnumMember(Value = "Person")]
		Person,		

		[EnumMember(Value = "Profile")]
		Profile,

		[EnumMember(Value = "Contact")]
		Contact,

		[EnumMember(Value = "StatusUpdate")]
		StatusUpdate
	}
}
