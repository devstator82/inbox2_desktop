using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Channels.Interfaces
{
    public delegate void ChannelProgressDelegate(long bytesWritten);

	public delegate void ChannelDataDelegate(string data);
}
