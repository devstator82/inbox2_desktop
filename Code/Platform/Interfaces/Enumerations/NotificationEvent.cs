using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Platform.Interfaces.Enumerations
{
	[Serializable]
	public enum NotificationEvent
	{
		None,
		Receive,
		Send,
		Store,
		Update,
		Delete,
	}
}