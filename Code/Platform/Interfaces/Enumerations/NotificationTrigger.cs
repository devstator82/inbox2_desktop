using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Platform.Interfaces.Enumerations
{
	[Serializable]
	public enum NotificationTrigger
	{
		None,
		Config,
		Message,
		Document,
		Person,
		Profile,
		StatusUpdate
	}
}