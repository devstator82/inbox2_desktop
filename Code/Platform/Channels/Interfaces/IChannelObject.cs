using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Platform.Channels.Interfaces
{
	public interface IChannelObject
	{
		bool IsDefault { get; }

		IClientInputChannel InputChannel { get; }

		IClientOutputChannel OutputChannel { get; }

		IClientContactsChannel ContactsChannel { get; }
	}
}