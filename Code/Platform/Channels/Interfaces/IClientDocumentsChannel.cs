using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Platform.Channels.Interfaces
{
	public interface IClientDocumentsChannel : IClientChannel
	{
		/// <summary>
		/// Gets the documents.
		/// </summary>
		/// <returns></returns>
		IEnumerable<ChannelAttachment> GetDocuments();
	}
}
