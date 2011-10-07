using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Localization;
using Inbox2.Framework.Threading;
using Inbox2.Framework.Threading.Progress;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Platform.Interfaces.ValueTypes;
using Inbox2.Platform.Logging;

namespace Inbox2.Core.Threading.Tasks.Receive
{
	public class ReceiveMessagesTask : ChannelTask
	{
		protected readonly ChannelConfiguration config;
		protected readonly IClientInputChannel channel;
		protected readonly ChannelFolder folder;
		protected readonly ReceiveRange range;
		protected readonly VirtualMailBox mailbox;
		
		private Timer progressTimer;

		public override int MaxQueuedInstances
		{
			get { return 1; }
		}

		public override string SingletonKey
		{
			get { return String.Format("{0}-{1}", config.ChannelId, folder.FolderId); }
		}

		public ReceiveMessagesTask(ChannelConfiguration config, IClientInputChannel channel, ChannelFolder folder, ReceiveRange range)
			: base(config, channel)
		{
			this.config = config;
			this.channel = channel;
			this.folder = folder;
			this.range = range;
			
			mailbox = VirtualMailBox.Current;
		}

		protected virtual void PreProcess()
		{
			ProgressGroup.Status = "Connecting...";
			Logger.Debug("Connecting to {0}", LogSource.Receive, config.DisplayName);

			channel.Connect();

			ProgressGroup.Status = "Checking for new messages...";			
		}

		protected override void ExecuteChannelCore()
		{
			if (ProgressGroup == null)
				ProgressGroup = new ProgressGroup { SourceChannelId = config.ChannelId };
			
			ExecuteOnUIThread(() => ProgressManager.Current.Register(ProgressGroup));

			try
			{
				PreProcess();

				channel.SelectFolder(folder);

				SetSelectionRange();

				var headers = channel.GetHeaders().ToList();

				// Channel returned oldest first, turn around and use newest first
				if (Channel is IReversePagableChannel)
					headers.Reverse();

				var headersToDownload = GetHeadersToDownload(headers);

				if (headersToDownload != null)
				{
					Logger.Debug("Got {0} headers from {1}", LogSource.Receive, headersToDownload.Count, config.DisplayName);

					if (headersToDownload.Count > 0)
					{
						ProgressGroup.Status = String.Format("Downloading {0}...", LocalizedFolderNames.GetName(folder.ToStorageFolder()));
						ProgressGroup.SetMaximum(headersToDownload.Count);

						// Download details for each header
						headersToDownload.ForEach(DownloadHeader);
					}
				}

				EventBroker.Publish(AppEvents.ReceiveMessagesFinished);
			}
			finally
			{
				PostProcess();
			}
		}

		protected virtual void PostProcess()
		{
			// Use a timer to have a smoother transition betwee the different
			// ReceiveMessagesTasks, this prevents flickering of the progress indicator.
			progressTimer = new Timer(delegate
			{
				ProgressGroup.IsCompleted = true;
				progressTimer.Dispose();

			}, null, 300, -1);
		}

		protected virtual void DownloadHeader(ChannelMessageHeader header)
		{
			Logger.Debug("Starting task for header {0}", LogSource.Receive, header);

			var task = new ReceiveMessageDetailsTask(config, channel, header, folder) { ProgressGroup = ProgressGroup };

			try
			{
				// Execute receive details task synchronously
				task.Execute();
			}
			catch (Exception ex)
			{
				Logger.Error("An exception has occured while getting message data. Exception = {0}", LogSource.Channel, ex);
			}
			finally
			{
				ProgressGroup.IncrementProgress();
			}
		}

		/// <summary>
		/// Gets the headers to download based on the ones that are allready in the database.
		/// </summary>
		/// <param name="headers">The headers.</param>
		/// <returns>List of headers to download.</returns>
		protected virtual List<ChannelMessageHeader> GetHeadersToDownload(List<ChannelMessageHeader> headers)
		{
			var headersToDownload = new List<ChannelMessageHeader>();

			foreach (var header in headers)
			{
				Message message = null;
				ChannelMessageHeader header1 = header;

				// The sent messages case ensures that we don't retrieve the item that has been sent from Inbox2 but is also
				// in (for example) your GMail sent items. We do this by appending the i2mpMessageid when sending the message.
				if (folder.ToStorageFolder() == Folders.SentItems)
				{
					if (!String.IsNullOrEmpty(header.Metadata.i2mpMessageId))
						using (mailbox.Messages.ReaderLock)
							message = mailbox.Messages.FirstOrDefault(m => m.MessageKey == header1.Metadata.i2mpMessageId);							
				}

				// Message can still be null (message did not have a match or we are looking at some other folder)
				if (message == null)
					using (mailbox.Messages.ReaderLock)
						message = mailbox.Messages.FirstOrDefault(m => (m.SourceChannelId == config.ChannelId || m.TargetChannelId == config.ChannelId)
							&& m.MessageNumber == header1.MessageNumber && m.Size == header1.Size);

				// Try to find message based on message identifier (not all channels support this)
				if (message == null && !String.IsNullOrEmpty(header1.MessageIdentifier))
					using (mailbox.Messages.ReaderLock)
						message = mailbox.Messages.FirstOrDefault(m => (m.SourceChannelId == config.ChannelId || m.TargetChannelId == config.ChannelId)
							&& m.MessageIdentifier == header1.MessageIdentifier && m.Size == header1.Size);

				if (message == null)
				{
					headersToDownload.Add(header);
				}
				else
				{
					if (CheckReadStates(message, header1))
					{
						// Check folder of message
						if (message.MessageFolder != Folders.Archive &&
						    message.MessageFolder != Folders.Trash &&
						    message.MessageFolder != folder.ToStorageFolder())
							message.MoveToFolder(folder.ToStorageFolder());
					}
				}
			}

			return headersToDownload;
		}

		protected virtual bool CheckReadStates(Message message, ChannelMessageHeader header)
		{
			// Channel doesn't support readstates, no point in checking
			if (!config.Charasteristics.SupportsReadStates)
				return false;

			// Message has been marked for channel update, ignore for now
			if (message.TargetMessageState.HasValue)
				return false;

			// Check readstate of message
			if (header.IsRead)
			{
				if (!message.IsRead)
					message.MarkRead();
			}
			else
			{
				if (message.IsRead)
					message.MarkUnread();
			}

			if (header.IsStarred)
			{
				// Star
				if (!message.IsStarred)
					message.SetStarred();
			}
			else
			{
				// Unstar
				if (message.IsStarred)
					message.SetUnstarred();
			}

			return true;
		}

		/// <summary>
		/// Sets the selection range and pagination properties on the given channel.
		/// </summary>
		protected virtual void SetSelectionRange()
		{
			// Page channel if channel supports it
			if (Channel is IPagableChannel)
			{
				var pagable = (IPagableChannel)Channel;
				var max = pagable.GetNumberOfItems();

				// ReversePagableChannels have the newest member on top
				if (Channel is IReversePagableChannel && !range.OnlyNew)
				{
					long startIndex = max - range.PageSize;

					pagable.PageSize = range.PageSize;
					pagable.StartIndex = startIndex < 0 ? 0 : startIndex;
					pagable.EndIndex = startIndex + range.PageSize;
				}
				else
				{
					// We only want the new items
					if (Channel is IReversePagableChannel)
					{
						// Loads last x items
						pagable.StartIndex = max - range.PageSize;

						// Forces everything starting from startIndex
						pagable.EndIndex = -1;

						// Max is less the pageSize, load everything
						if (pagable.StartIndex < 0)
							pagable.StartIndex = 0;
					}
					else
					{
						long startIndex = max;

						startIndex = startIndex - range.PageSize;

						pagable.PageSize = range.PageSize;
						pagable.StartIndex = startIndex < 0 ? 0 : startIndex;
						pagable.EndIndex = startIndex + range.PageSize;
					}
				}
			}
		}
	}
}