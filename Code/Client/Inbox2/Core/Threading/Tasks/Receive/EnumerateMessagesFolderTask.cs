using System;
using System.Collections.Generic;
using System.Linq;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Interfaces.ValueTypes;

namespace Inbox2.Core.Threading.Tasks.Receive
{
	public class EnumerateMessagesFolderTask : ReceiveMessagesTask
	{
		public EnumerateMessagesFolderTask(ChannelConfiguration config, IClientInputChannel channel, ChannelFolder folder, ReceiveRange range) 
			: base(config, channel, folder, range)
		{
		}

		protected override void PreProcess()
		{
			channel.Connect();

			ProgressGroup.Status = String.Format("Synchronizing '{0}' folder", folder.FolderType);
		}

		protected override List<ChannelMessageHeader> GetHeadersToDownload(List<ChannelMessageHeader> headers)
		{
			foreach (var header in headers)
			{
				Message message;
				ChannelMessageHeader header1 = header;

				using (mailbox.Messages.ReaderLock)
					message = mailbox.Messages.FirstOrDefault(m => (m.SourceChannelId == config.ChannelId || m.TargetChannelId == config.ChannelId)
						&& m.MessageIdentifier == header1.MessageIdentifier && m.Size == header1.Size);

				if (message != null)
				{
					// Check if the message is in the folder we are currently enumerating
					if (message.MessageFolder != folder.ToStorageFolder())
						message.MoveToFolder(folder.ToStorageFolder());
				}
			}

			// Return empty list
			return null;
		}

		protected override void PostProcess()
		{
		}
	}
}
