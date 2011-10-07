using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi.Enumerations
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public enum DeviceType
	{
		[EnumMember(Value = "DesktopWin")]
		DesktopWin,

		[EnumMember(Value = "DesktopMac")]
		DesktopMac,

		[EnumMember(Value = "iPhone")]
		iPhone,

		[EnumMember(Value = "Android")]
		Android,

		[EnumMember(Value = "BlackBerry")]
		BlackBerry,

		[EnumMember(Value = "WindowsPhone")]
		WindowsPhone,		
	}
}
