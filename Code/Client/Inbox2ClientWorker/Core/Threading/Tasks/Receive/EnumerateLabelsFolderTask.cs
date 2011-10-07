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

namespace Inbox2ClientWorker.Core.Threading.Tasks.Receive
{
	public class EnumerateLabelsFolderTask : ReceiveMessagesTask
	{
		private readonly List<Message> changed;

		public EnumerateLabelsFolderTask(List<Message> changed, ChannelConfiguration config, IClientInputChannel channel, ChannelFolder folder, ReceiveRange range) 
			: base(config, channel, folder, range)
		{
			this.changed = changed;
		}

		protected override void PreProcess()
		{
			channel.Connect();
		}

		protected override List<ChannelMessageHeader> GetHeadersToDownload(List<ChannelMessageHeader> headers)
		{
			var headersToDownload = new List<ChannelMessageHeader>();

			foreach (var header in headers)
			{
				var message = dataService.SelectBy<Message>(
					String.Format("select * from Messages where (SourceChannelId='{0}' or TargetChannelId='{0}') and MessageIdentifier='{1}' and Size='{2}'",
						config.ChannelId, header.MessageIdentifier, header.Size));

				if (message != null)
				{
					Message message1 = message;

					if (changed.Any(m => m.MessageId == message1.MessageId))
					{
						message = changed.First(m => m.MessageId == message1.MessageId);
					}
					else
					{
						changed.Add(message);
					}

					message.ReceiveLabels.Add(new Label(folder.Name, ParseLabelType(folder.Name), header.MessageNumber));						
				}
				else
				{
					// Message not found, perform a download
					headersToDownload.Add(header);
				}
			}

			return headersToDownload;
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
