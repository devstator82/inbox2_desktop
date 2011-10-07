using System;
using System.Linq;
using Inbox2.Core.Configuration;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Threading;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Platform.Logging;

namespace Inbox2.Core.Threading.Tasks.Sync
{
	public class SyncStatusUpdatesTask : BackgroundTask
	{
		private readonly ChannelConfiguration config;
		private readonly IClientStatusUpdatesChannel channel;

		public override int MaxQueuedInstances
		{
			get { return 1; }
		}

		public override string SingletonKey
		{
			get { return config.ChannelId.ToString(); }
		}

		public SyncStatusUpdatesTask(ChannelConfiguration config, IClientStatusUpdatesChannel channel)
		{
			this.config = config;
			this.channel = channel;
		}

		protected override bool CanExecute()
		{
			ChannelContext.Current.ClientContext = new ChannelClientContext(ClientState.Current.Context, config);

			var lastRun = ChannelContext.Current.ClientContext.GetSetting("LastSyncStatusUpdatesDate");

			return String.IsNullOrEmpty(lastRun.ToString()) ||
					DateTime.Now.Subtract(((DateTime)lastRun)).TotalMinutes >
						SettingsManager.ClientSettings.AppConfiguration.ReceiveConfiguration.SyncStatusUpdatesInterval;
		}

		protected override void ExecuteCore()
		{
			GetFriendUpdates();
			GetMentions();

			channel.Dispose();

			// Save last run time
			ChannelContext.Current.ClientContext.SaveSetting("LastSyncStatusUpdatesDate", DateTime.Now);

			EventBroker.Publish(AppEvents.SyncStatusUpdatesFinished);
		}

		void GetFriendUpdates()
		{
			try
			{
				// Get updates for the last 7 days
				var parser = new UserStatusParser(config);
				var updates = channel.GetUpdates(50).Where(u => u.DatePosted > DateTime.Now.AddDays(-7));

				foreach (var statusupdate in updates)
				{
					Logger.Debug("Received friend update. ChannelStatusKey = {0}, DatePosted = {1}", LogSource.Sync, statusupdate.ChannelStatusKey, statusupdate.DatePosted);

					parser.ProcessStatusUpdate(statusupdate, StatusTypes.FriendUpdate);
				}
			}
			catch (Exception ex)
			{
				Logger.Error("An error has occured while trying to get friend updates. Exception = {0}", LogSource.Sync, ex);
			}
		}

		void GetMentions()
		{
			try
			{
				// Get updates for the last 7 days
				var parser = new UserStatusParser(config);
				var updates = channel.GetMentions(50).Where(u => u.DatePosted > DateTime.Now.AddDays(-7));

				foreach (var statusupdate in updates)
				{
					Logger.Debug("Received mention. ChannelStatusKey = {0}, DatePosted = {1}", LogSource.Sync, statusupdate.ChannelStatusKey, statusupdate.DatePosted);

					parser.ProcessStatusUpdate(statusupdate, StatusTypes.Mention);
				}
			}
			catch (Exception ex)
			{
				Logger.Error("An error has occured while trying to get mentions. Exception = {0}", LogSource.Sync, ex);
			}
		}		
	}
}
