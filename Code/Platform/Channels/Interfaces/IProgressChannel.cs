using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Channels.Interfaces
{
	public interface IProgressChannel
	{
		ChannelProgressDelegate BytesRead { get; set; }
		ChannelProgressDelegate BytesWritten { get; set; }
	}
}
