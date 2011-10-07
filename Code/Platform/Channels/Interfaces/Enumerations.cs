using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Platform.Channels.Interfaces
{
	[DataContract]
	public enum ConnectResult
	{
		[EnumMember(Value = "1")]
		Success,
		
		// User not able to login (password or username incorrect)
		[EnumMember(Value = "2")]
		AuthFailure,

		// Service not available (hotmail user not allowed pop3 for instance)
		[EnumMember(Value = "3")]
		ServiceNotAvailable,

		// Channel error occured, something unexpected
		[EnumMember(Value = "4")]
		ChannelFailure,

		// Language not supported (for instance on facebook)
		[EnumMember(Value = "5")]
		LanguageNotSupported,
		
		// Login successfull but processing is delayed
		[EnumMember(Value = "6")]
		Delay
	}

	public enum ChannelType
	{
		Input,
		Output,
		Contacts,
		StatusUpdates,
	}
}
