using System;
using System.Collections.Generic;
using System.Linq;
using Inbox2.Framework;
using Inbox2.Framework.Persistance;
using Inbox2.Framework.Threading;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Configuration;

namespace Inbox2ClientWorker.Core.Threading.Tasks.Receive
{
	public class ProcessLabelsTask : BackgroundTask
	{
		private readonly List<Message> changed;
		private readonly ChannelConfiguration config;
		private readonly IDataService dataService;

		public ProcessLabelsTask(List<Message> changed, ChannelConfiguration config)
		{
			this.changed = changed;
			this.config = config;
			this.dataService = ClientState.Current.DataService;
		}

		protected override void ExecuteCore()
		{
			var messages = dataService.SelectAll<Message>(
				String.Format("select * from Messages where SourceChannelId = '{0}' or TargetChannelId = '{0}' and Labels != ''", config.ChannelId)).ToList();

			// Add all messages with labels for comparison
			foreach (var message in messages)
			{
				Message message1 = message;

				if (!changed.Any(m => m.MessageId == message1.MessageId))
					changed.Add(message);
			}

			foreach (var message in changed)
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
			}
		}
	}
}
