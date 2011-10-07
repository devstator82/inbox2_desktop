using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Logging
{
	public enum LogSource
	{
		None,
		ServiceCall,
		Startup,
		Interop,
		TaskQueue,
		BackgroundTask,
		Channel,
		Command,
		Configuration,
		Storage,
		Send,
		Receive,
		Sync,
		Search,
		MessageMatcher,
		Layout,
		UI,
		Security,
        Performance,
		Analytics,
		Notifications,
		Actions,
		AppServer,
		ForwardServer
	}
}