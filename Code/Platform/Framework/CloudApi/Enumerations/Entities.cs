using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi.Enumerations
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public enum Entities
	{
		[EnumMember(Value = "Messages")]
		Messages,

		[EnumMember(Value = "Documents")]
		Documents,

		[EnumMember(Value = "Persons")]
		Persons,		

		[EnumMember(Value = "Profiles")]
		Profiles,

		[EnumMember(Value = "Contacts")]
		Contacts,

		[EnumMember(Value = "StatusUpdates")]
		StatusUpdates
	}
}
