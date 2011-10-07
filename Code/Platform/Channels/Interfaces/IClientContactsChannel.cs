using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Platform.Channels.Interfaces
{
	public interface IClientContactsChannel : IClientChannel
	{
		/// <summary>
		/// Gets the headers.
		/// </summary>
		/// <returns></returns>
		IEnumerable<ChannelContact> GetContacts();

		/// <summary>
		/// Clones the existing contacts channel.
		/// </summary>
		/// <returns></returns>
		IClientContactsChannel Clone();
	}
}