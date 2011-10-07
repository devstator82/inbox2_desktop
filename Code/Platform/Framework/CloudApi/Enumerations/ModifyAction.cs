using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi.Enumerations
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public enum ModifyAction
	{
		[EnumMember(Value = "None")]
		None,

		[EnumMember(Value = "Add")]
		Add,

		[EnumMember(Value = "Update")]
		Update,

		[EnumMember(Value = "Delete")]
		Delete,

		[EnumMember(Value = "IsRead")]
		IsRead,

		[EnumMember(Value = "Star")]
		Star,

		[EnumMember(Value = "Label")]
		Label,

		[EnumMember(Value = "Archive")]
		Archive,

		[EnumMember(Value = "Folder")]
		Folder,

		[EnumMember(Value = "ShareState")]
		ShareState,

		[EnumMember(Value = "IsSoft")]
		Soft,

		[EnumMember(Value = "Join")]
		Join,
	}
}