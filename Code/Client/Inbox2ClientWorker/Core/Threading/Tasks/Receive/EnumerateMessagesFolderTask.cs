using System;
using System.Collections.Generic;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Interfaces.ValueTypes;

namespace Inbox2ClientWorker.Core.Threading.Tasks.Receive
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
		}

		protected override List<ChannelMessageHeader> GetHeadersToDownload(List<ChannelMessageHeader> headers)
		{
			foreach (var header in headers)
			{
				var message = dataService.SelectBy<Message>(
					String.Format("select * from Messages where (SourceChannelId='{0}' or TargetChannelId='{0}') and MessageIdentifier='{1}' and Size='{2}'",
						config.ChannelId, header.MessageIdentifier, header.Size));

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
	}
}
