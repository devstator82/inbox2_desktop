using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.Threading;
using Inbox2.Framework.Threading.Progress;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Logging;

namespace Inbox2.Core.Threading.Tasks.Receive
{
	public class ReceiveFoldersTask : ChannelTask
	{
		private readonly ChannelConfiguration config;
		private readonly IClientInputChannel channel;
		private readonly List<ChannelFolder> folders;
		private readonly VirtualMailBox mailbox = VirtualMailBox.Current;
        private readonly static string[] ignore = new[] { "[google mail]", "[gmail]" };

		public List<ChannelFolder> Folders
		{
			get { return folders; }
		}

		public override int MaxQueuedInstances
		{
			get { return 1; }
		}

		public override string SingletonKey
		{
			get { return config.ChannelId.ToString(); }
		}

		public ReceiveFoldersTask(ChannelConfiguration config, IClientInputChannel channel) : base(config, channel)
		{
			this.config = config;
			this.channel = channel;
			this.folders = new List<ChannelFolder>();
		}

		protected override void ExecuteChannelCore()
		{
			ProgressGroup = new ProgressGroup { SourceChannelId = config.ChannelId };

			ExecuteOnUIThread(() => ProgressManager.Current.Register(ProgressGroup));

			ProgressGroup.Status = "Connecting...";

			var result = channel.Connect();

			try
			{
				if (result == ConnectResult.Success)
				{
					ProgressGroup.Status = "Retreiving folders...";

				    var cf = channel.GetFolders().ToList();
                    folders.AddRange(cf.Where(f => !f.Name.ToLower().StartsWithAny(ignore)));

					foreach (var folder in folders)
						Logger.Debug("Folder: {0} {1}", LogSource.Receive, folder.FolderId, folder.FolderType);

					if (Channel is IReadStateChannel)
					{						
						List<Message> messages;

						// Get dirty messages
						using (mailbox.Messages.ReaderLock)
							messages = mailbox.Messages
								.Where(m => m.TargetChannel == config || m.SourceChannelId == config.ChannelId)
								.Where(m => m.TargetMessageState.HasValue)
								.Union(mailbox.Messages
									.Where(m => m.TargetChannel == config || m.SourceChannelId == config.ChannelId)
									.Where(m => m.PostLabels.Any()))
								.ToList();

						if (messages.Count > 0)
						{
							ProgressGroup.Status = "Syncing readstates...";

							foreach (var message in messages)
							{
								try
								{
									new UpdateMessageStateTask(config, (IReadStateChannel)Channel, folders, message).Execute();
								}
								catch (Exception ex)
								{
									Logger.Error("An error has occured while trying to update message state. Message = {0}, Exception = {1}", LogSource.BackgroundTask, message, ex);
								}
								finally
								{
									message.TargetMessageState = null;

									ClientState.Current.DataService.Update(message);
								}
							}
						}
					}					
				}
				else
				{
					ShowChannelError(result, false);
				}
			}
			catch (Exception)
			{
				ShowChannelError(result, true);

				throw;
			}
			finally
			{
				ProgressGroup.IsCompleted = true;
			}
		}

		void ShowChannelError(ConnectResult result, bool isException)
		{
			ClientState.Current.ShowMessage(
				new AppMessage(ExceptionHelper.BuildChannelError(channel, result, isException))
					{
						SourceChannelId = config.ChannelId
					}, MessageType.Error);
		}
	}
}