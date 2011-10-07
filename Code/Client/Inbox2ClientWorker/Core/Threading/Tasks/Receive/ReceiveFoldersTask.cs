using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.Persistance;
using Inbox2.Framework.Threading;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Logging;

namespace Inbox2ClientWorker.Core.Threading.Tasks.Receive
{
	public class ReceiveFoldersTask : ChannelTask
	{
		private readonly ChannelConfiguration config;
		private readonly IClientInputChannel channel;
		private readonly List<ChannelFolder> folders;
		private readonly IDataService dataService;
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
			this.dataService = ClientState.Current.DataService;
		}

		protected override void ExecuteChannelCore()
		{
			var result = channel.Connect();

			if (result == ConnectResult.Success)
			{
				var cf = channel.GetFolders().ToList();
				folders.AddRange(cf.Where(f => !f.Name.ToLower().StartsWithAny(ignore)));

				foreach (var folder in folders)
					Logger.Debug("Folder: {0} {1}", LogSource.Receive, folder.FolderId, folder.FolderType);

				if (Channel is IReadStateChannel)
				{
					// Get dirty messages
					var messages = dataService.SelectAll<Message>(
						String.Format("select * from Messages where (SourceChannelId='{0}' or TargetChannelId='{0}') and (TargetMessageState != '' or SendLabels != '')", config.ChannelId)).ToList();

					if (messages.Count > 0)
					{
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
				throw new ApplicationException("Unable to connect");
			}			
		}
	}
}