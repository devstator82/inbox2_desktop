using System;
using System.Collections.Generic;
using System.Linq;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Platform.Interfaces.ValueTypes;
using Inbox2.Framework.Localization;

namespace Inbox2.Core.Threading.Tasks.Receive
{
	public class EnumerateLabelsFolderTask : ReceiveMessagesTask
	{
		public EnumerateLabelsFolderTask(ChannelConfiguration config, IClientInputChannel channel, ChannelFolder folder, ReceiveRange range) 
			: base(config, channel, folder, range)
		{
		}

		protected override void PreProcess()
		{
			channel.Connect();

			ProgressGroup.Status = String.Format("Updating label '{0}'", folder.Name);
		}

		protected override List<ChannelMessageHeader> GetHeadersToDownload(List<ChannelMessageHeader> headers)
		{
			var headersToDownload = new List<ChannelMessageHeader>();

			foreach (var header in headers)
			{
				Message message;
				ChannelMessageHeader header1 = header;

				using (mailbox.Messages.ReaderLock)
					message = mailbox.Messages.FirstOrDefault(m => (m.SourceChannelId == config.ChannelId || m.TargetChannelId == config.ChannelId)
						&& m.MessageIdentifier == header1.MessageIdentifier && m.Size == header1.Size);

				if (message != null)
					message.ReceiveLabels.Add(new Label(folder.Name, ParseLabelType(folder.Name), header.MessageNumber));
				else
				{
					// Message not found, perform a download
					headersToDownload.Add(header1);
				}
			}

			return headersToDownload;
		}

		protected override void PostProcess()
		{
		}

		LabelType ParseLabelType(string label)
		{
			if (label == Strings.Todo)
				return LabelType.Todo;
			else if (label == Strings.WaitingFor)
				return LabelType.WaitingFor;
			else if (label == Strings.Someday)
				return LabelType.Someday;
			else
				return LabelType.Custom;
		}
	}
}
