using System;
using System.Collections.Generic;
using System.Linq;
using Inbox2.Core.Configuration;
using Inbox2.Core.Threading.Repeat;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Threading;
using Inbox2.Framework.Threading.Progress;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Connections;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Interfaces.ValueTypes;

namespace Inbox2.Core.Threading.Tasks.Receive
{
	public class ReceiveTask : BackgroundTask
	{
		private readonly ReceiveRange range;
		private ChannelInstance channel;

		public override bool RequiresNetworkConnection
		{
			get { return true; }
		}

		public override int MaxQueuedInstances
		{
			get { return 1; }
		}

		public override string SingletonKey
		{
			get { return "Receive"; }
		}

		public ReceiveTask()
		{
		}

		public ReceiveTask(ReceiveRange range)
		{
			this.range = range;
		}

		public ReceiveTask(ReceiveRange range, ChannelInstance channel)
		{
			this.range = range;
			this.channel = channel;
		}		

		protected override void ExecuteCore()
		{
			ClientState.Current.Messages.Errors.Clear();

			if (ChannelsManager.Channels.Count == 0)
				return;

			var channels = ListOrSingle<ChannelInstance>.Get(channel, ChannelsManager.Channels);
			var gate = new ConcurrencyGate(2);

			foreach (var channelInstance in channels.Where(c => !c.Configuration.IsConnected))
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
			var group = new ProgressGroup { SourceChannelId = config.ChannelId };

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
					gate.Add(new EnumerateMessagesFolderTask(config, inputChannel.Clone(), folder, range) { ProgressGroup = group });
				}
				else if (folder.FolderType == ChannelFolderType.Label)
				{
					if (config.Charasteristics.SupportsLabels)
						gate.Add(new EnumerateLabelsFolderTask(config, inputChannel.Clone(), folder, range) { ProgressGroup = group });
				}
			}

			gate.Execute();
			group.IsCompleted = true;

			// Process any labels that we might have received
			if (config.Charasteristics.SupportsLabels)
				new ProcessLabelsTask(config).Execute();

			if (inputChannel is IPoolableChannel)
				((IPoolableChannel)inputChannel).FreeAllConnections();
		}

		public override void OnCompleted()
		{
			channel = null;

			EventBroker.Publish(AppEvents.ReceiveFinished);

			// Schedule next execution
			new Run("Receive").After(SettingsManager.ClientSettings.AppConfiguration.ReceiveConfiguration.ReceiveInterval).Minutes().Call(Tasks.Receive);

			base.OnCompleted();
		}
	}
}