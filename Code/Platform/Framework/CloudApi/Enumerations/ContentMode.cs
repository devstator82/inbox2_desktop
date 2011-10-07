using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi.Enumerations
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public enum ContentMode
	{
		[EnumMember(Value = "Shallow")]
		Shallow,

		[EnumMember(Value = "Deep")]
		Deep,

		[EnumMember(Value = "Text")]
		Text,

		[EnumMember(Value = "Html")]
		Html,

		[EnumMember(Value = "Binary")]
		Binary
	}
}
