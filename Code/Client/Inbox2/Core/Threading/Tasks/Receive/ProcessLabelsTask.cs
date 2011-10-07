using System;
using System.Collections.Generic;
using System.Linq;
using Inbox2.Framework.Threading;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Configuration;

namespace Inbox2.Core.Threading.Tasks.Receive
{
	public class ProcessLabelsTask : BackgroundTask
	{
		private readonly ChannelConfiguration config;

		private readonly VirtualMailBox mailbox = VirtualMailBox.Current;

		public ProcessLabelsTask(ChannelConfiguration config)
		{
			this.config = config;
		}

		protected override void ExecuteCore()
		{
			List<Message> messages;

			using (mailbox.Messages.ReaderLock)
				messages = mailbox.Messages
					.Where(m => m.SourceChannelId == config.ChannelId || m.TargetChannelId == config.ChannelId)
					.Where(m => m.ReceiveLabels.Any() || m.LabelsList.Any())
					.ToList();

			foreach (var message in messages)
			{
				var removed = new List<Label>();
				var added = new List<Label>();

				// Check if any labels have been removed from the backend service
				foreach (var label in message.LabelsList)
				{
					var label1 = label;

					if (!message.ReceiveLabels.Any(l => l.Equals(label1)))
						removed.Add(label1);
				}

				// Check if any labels have been added by the backend service
				foreach (var label in message.ReceiveLabels)
				{
					var label1 = label;
					var messageLabel = message.LabelsList.FirstOrDefault(l => l.Equals(label1));

					if (messageLabel == null)
						added.Add(label1);
					else
					{
						// Check if the messagenumber matches with what we have
						if (messageLabel.MessageNumber != label1.MessageNumber)
						{
							// If it doesn't match, remove and re-add the label
							message.RemoveLabel(messageLabel, false);
							message.AddLabel(label1, false);
						}
					}
				}

				if (removed.Any() || added.Any())
				{
					foreach (var label in removed)
						message.RemoveLabel(label, false);

					foreach (var label in added)
						message.AddLabel(label, false);
				}

				// Clear sessionlabels
				message.ReceiveLabels.Clear();			
			}
		}
	}
}
