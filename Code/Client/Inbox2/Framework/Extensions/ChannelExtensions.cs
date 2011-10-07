using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;

namespace Inbox2.Framework.Extensions
{
	public static class ChannelExtensions
	{
		public static SourceAddress GetSourceAddress(this IClientInputChannel channel)
		{
			return new SourceAddress(channel.SourceAdress, ClientState.Current.Context.DisplayName);
		}
	}
}
