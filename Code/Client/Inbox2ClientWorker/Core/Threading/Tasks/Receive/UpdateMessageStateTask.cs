using System;
using System.Collections.Generic;
using System.Linq;
using Inbox2.Framework.Threading;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Logging;

namespace Inbox2ClientWorker.Core.Threading.Tasks.Receive
{
	public class UpdateMessageStateTask : BackgroundTask
	{
		private readonly List<ChannelFolder> availableFolders;
		private readonly ChannelConfiguration config;
		private readonly IReadStateChannel channel;		
		private readonly Message message;

		public override bool RequiresNetworkConnection
		{
			get { return true; }
		}

		public UpdateMessageStateTask(ChannelConfiguration config, IReadStateChannel channel, List<ChannelFolder> availableFolders, Message message)
		{
			this.config = config;
			this.channel = channel;
			this.availableFolders = availableFolders;
			this.message = message;
		}

		protected override void ExecuteCore()
		{
			ChannelMessageHeader header = message.DuckCopy<ChannelMessageHeader>();

			if (message.TargetMessageState == EntityStates.Deleted)
			{
				Delete(header);
			}

			if (message.TargetMessageState == EntityStates.Purged)
			{
				Delete(header);

				Logger.Debug("Permanantly deleting {0}", LogSource.Command, message);
				channel.Purge(header);
			}

			if (message.TargetMessageState == EntityStates.Read)
			{
				Logger.Debug("Setting message {0} as read", LogSource.Command, message);
				channel.MarkRead(header);
			}

			if (message.TargetMessageState == EntityStates.Unread)
			{
				Logger.Debug("Setting message {0} as unread", LogSource.Command, message);
				channel.MarkUnread(header);
			}

			if (message.TargetMessageState == EntityStates.Starred)
			{
				Logger.Debug("Setting message {0} as starred", LogSource.Command, message);
				channel.SetStarred(header, true);
			}

			if (message.TargetMessageState == EntityStates.Unstarred)
			{
				Logger.Debug("Setting message {0} as unstarred", LogSource.Command, message);
				channel.SetStarred(header, false);
			}

			if (message.TargetMessageState == EntityStates.Archived)
			{
				Archive(header);
			}

			if (message.TargetMessageState == EntityStates.Unarchived)
			{
				UnArchive(header);
			}

			if (message.TargetMessageState == EntityStates.Spam)
			{
				MarkAsSpam(header);
			}

			if (config.Charasteristics.SupportsLabels 
				&& channel is ILabelsChannel)
			{
				ChangeLabels(header);
			}

			Logger.Debug("Updated messagestate for message {0} successfully", LogSource.Command, message);
		}

		void ChangeLabels(ChannelMessageHeader header)
		{
			var labelsChannel = (ILabelsChannel) channel;

			if (!String.IsNullOrEmpty(message.SendLabels))
			{
				var postLabels = new List<Label>(
					message.SendLabels.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
						.Select(l => new Label(l)));

				// Check if this is an add or remove
				for (int i = 0; i < postLabels.Count; i++)
				{
					var label = postLabels[i];
					var label1 = label;

					if (labelsChannel.LabelsSupport == LabelsSupport.Folders)
					{
						// Make sure the given label exists as a folder since the implementation of the current 
						// channel requires this (this is the case with for example gmail).
						var folder = availableFolders.FirstOrDefault(
							f => f.Name.Equals(label1.Labelname, StringComparison.InvariantCultureIgnoreCase));

						if (folder == null)
						{
							// Folder doesn't exist, create it
							availableFolders.Add(channel.CreateFolder(label.Labelname));
						}
					}
					
					// Used for error reporting
					string action = String.Empty;

					try
					{
						if (message.LabelsList.Any(l => l.Equals(label1)))
						{
							// Add
							action = "Add label";
							labelsChannel.AddLabel(header, label1.Labelname);
						}
						else
						{
							// Remove label, sometimes labels have no Message number (for instance when 
							// adding and deleting labels before send/receive has been executed)
							if (!String.IsNullOrEmpty(label1.MessageNumber))
							{
								// If this is the last label left and the current channel has a folder based
								// label storage, don't remove the label but rather move the message to the inbox.
								// Otherwise we might end up losing the message with exchange.
								if (i == postLabels.Count - 1 &&
								    labelsChannel.GetType().ToString().Contains("Exchange") && 
								    message.LabelsList.Count == 0)
								{
									action = "Move to inbox";
									channel.MoveToFolder(header, availableFolders.First(f => f.FolderType == ChannelFolderType.Inbox));
								}
								else
								{
									action = "Remove label";
									labelsChannel.RemoveLabel(label1.MessageNumber, label1.Labelname);
								}							
							}
						}
					}
					catch (Exception ex)
					{
						Logger.Error("An error has occured while trying to apply label. Action = {0}, Exception = {1}", LogSource.Channel, action, ex);
					}
				}
			}
			
			// Clear all labels that have been processed
			message.SendLabels = String.Empty;
		}

		void Archive(ChannelMessageHeader header)
		{
			var archive = availableFolders.FirstOrDefault(a => a.FolderType == ChannelFolderType.Archive);

			if (archive == null)
			{
				Logger.Warn("Unable to find archive folder, ignoring channel archive action. Message = {0}", LogSource.Channel, message);
			}
			else
			{
				Logger.Debug("Setting message {0} as archived", LogSource.Command, message);
					
				channel.MoveToFolder(header, archive);
				message.SourceFolder = archive.FolderId;
			}
		}

		void UnArchive(ChannelMessageHeader header)
		{
			var inbox = availableFolders.FirstOrDefault(a => a.FolderType == ChannelFolderType.Inbox);

			if (inbox == null)
			{
				Logger.Warn("Unable to find inbox folder, ignoring channel unarchive action. Message = {0}", LogSource.Channel, message);
			}
			else
			{
				Logger.Debug("Setting message {0} as unarchived", LogSource.Command, message);
					
				channel.MoveToFolder(header, inbox);
				message.SourceFolder = inbox.FolderId;
			}
		}

		void MarkAsSpam(ChannelMessageHeader header)
		{
			var spam = availableFolders.FirstOrDefault(a => a.FolderType == ChannelFolderType.Spam);

			if (spam == null)
			{
				Logger.Warn("Unable to find spam folder, ignoring channel spam action. Message = {0}", LogSource.Channel, message);
			}
			else
			{
				Logger.Debug("Setting message {0} as spam", LogSource.Command, message);
					
				channel.MoveToFolder(header, spam);
				message.SourceFolder = spam.FolderId;
			}
		}

		void Delete(ChannelMessageHeader header)
		{
			Logger.Debug("Setting message {0} as deleted", LogSource.Command, message);

			var trash = availableFolders.FirstOrDefault(a => a.FolderType == ChannelFolderType.Trash);

			if (trash == null)
			{
				channel.MarkDeleted(header);
			}
			else
			{
				channel.MoveToFolder(header, trash);
				message.SourceFolder = trash.FolderId;
			}
		}
	}
}
