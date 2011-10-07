using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.Interfaces.Enumerations
{
	public enum SyncStateState
	{
		Connecting,
		Completed,
		Synching,
		
		NoConnection,
		NoChannels,
		
		AuthError,
		NetError
	}
}
