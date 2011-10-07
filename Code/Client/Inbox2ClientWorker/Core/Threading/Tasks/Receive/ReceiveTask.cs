using System;
using System.Collections.Generic;
using System.Linq;
using Inbox2.Core;
using Inbox2.Core.Configuration;
using Inbox2.Core.Threading.Repeat;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Persistance;
using Inbox2.Framework.Threading;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Connections;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Interfaces.ValueTypes;

namespace Inbox2ClientWorker.Core.Threading.Tasks.Receive
{
	public class ReceiveTask : BackgroundTask
	{
		private readonly IDataService dataService;
		private readonly ReceiveRange range;		
		
		public override bool RequiresNetworkConnection
		{
			get { return true; }
		}

		public ReceiveTask()
		{
			dataService = ClientState.Current.DataService;
		}

		public ReceiveTask(ReceiveRange range) : this()
		{
			this.range = range;
		}

		public override int MaxQueuedInstances
		{
			get { return 1; }
		}

		public override string SingletonKey
		{
			get { return "Receive"; }
		}

		protected override void ExecuteCore()
		{
			ClientState.Current.Messages.Errors.Clear();

			var channels = dataService.SelectAll<ChannelConfig>()
				.Select(ChannelFactory.Create)
				.Select(s => new ChannelInstance(s))
				.Where(c => !c.Configuration.IsConnected)
				.ToList();

			if (channels.Count == 0)
				return;

			var gate = new ConcurrencyGate(2);

			foreach (var channelInstance in channels)
			{
				var inputChannel = channelInstance.InputChannel;
				var config = channelInstance.Configuration;

				var foldersTask = new ReceiveFoldersTask(channelInstance.Configuration, inputChannel.Clone());

				foldersTask.FinishedSuccess += ((sender, e) =>
					ReceiveFoldersTask_FinishedSuccess((ReceiveFoldersTask)sender, config, inputChannel));

				gate.Add(foldersTask);
			}

			// Wait for all receive tasks to complete, keeping the queue filled for the ReceiveTask.
			// This prevents any new receive task from getting enqueued.
			gate.Execute();	
		
			// Clean up connection scavangers
			ConnectionPoolScavenger.Shutdown();

			GC.Collect(GC.MaxGeneration);
		}

		void ReceiveFoldersTask_FinishedSuccess(ReceiveFoldersTask foldersTask, ChannelConfiguration config, IClientInputChannel inputChannel)
		{
			var gate = new ConcurrencyGate(config.InputChannel.MaxConcurrentConnections);
			var folders = foldersTask.Folders.OrderByDescending(f => (int) f.FolderType);
			var changed = new List<Message>();

			// Folders are pushed by descending order into concurrency gate and then popped again,
			// this way the Inbox folder is processed before the sent items folder etc.
			foreach (var folder in folders)
			{
				if (folder.FolderType == ChannelFolderType.Inbox
					|| folder.FolderType == ChannelFolderType.SentItems)
				{
					gate.Add(new ReceiveMessagesTask(config, inputChannel.Clone(), folder, range));							
				} 
				else if (folder.FolderType == ChannelFolderType.Trash || folder.FolderType == ChannelFolderType.Spam)
				{
					gate.Add(new EnumerateMessagesFolderTask(config, inputChannel.Clone(), folder, range));
				}
				else if (folder.FolderType == ChannelFolderType.Label)
				{
					if (config.Charasteristics.SupportsLabels)
						gate.Add(new EnumerateLabelsFolderTask(changed, config, inputChannel.Clone(), folder, range));
				}
			}

			gate.Execute();

			// Process any labels that we might have received
			if (config.Charasteristics.SupportsLabels)
				new ProcessLabelsTask(changed, config).Execute();

			if (inputChannel is IPoolableChannel)
				((IPoolableChannel)inputChannel).FreeAllConnections();
		}

		public override void OnCompleted()
		{
			EventBroker.Publish(AppEvents.ReceiveFinished);

			// Schedule next execution
			new Run("Receive").After(SettingsManager.ClientSettings.AppConfiguration.ReceiveConfiguration.ReceiveInterval).Minutes().Call(Tasks.Receive);

			base.OnCompleted();
		}
	}
}