using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;

namespace Inbox2.Platform.Channels.Interfaces
{
	public interface IClientOutputChannel : IClientChannel
	{
		void Send(ChannelMessage message);
	}
}