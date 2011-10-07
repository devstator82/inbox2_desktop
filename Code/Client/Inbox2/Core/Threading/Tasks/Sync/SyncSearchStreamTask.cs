using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Core.Configuration;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Threading;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Platform.Logging;

namespace Inbox2.Core.Threading.Tasks.Sync
{
	public class SyncSearchStreamTask : BackgroundTask
	{
		public override bool RequiresNetworkConnection
		{
			get { return true; }
		}
	
		public override int MaxQueuedInstances
		{
			get { return 1; }
		}


		protected override bool CanExecute()
		{
			var lastRun = ChannelContext.Current.ClientContext.GetSetting("LastSyncSearchStreamDate");

			return String.IsNullOrEmpty(lastRun.ToString()) ||
					DateTime.Now.Subtract(((DateTime)lastRun)).TotalMinutes >
						SettingsManager.ClientSettings.AppConfiguration.ReceiveConfiguration.SyncSearchStreamInterval;
		}

		protected override void ExecuteCore()
		{
			// Get the keywords we need to perform a search for
			foreach (var kw in VirtualMailBox.Current.StreamSearchKeywords.GetKeyWords())
			{
				#region Parse channelname and keyword

				var parts = kw.Split('|');

				if (parts.Length != 2)
				{
					Logger.Warn("Invalid search keyword. Keyword = {0}", LogSource.Sync, kw);
					return;
				}

				var channelname = parts[0];
				var keyword = parts[1];				

				// Find channel with given name
				var channels = ChannelsManager.GetStatusChannels();
				var channel = channels.FirstOrDefault(c => c.Configuration.DisplayName == channelname);

				if (channel == null)
				{
					Logger.Warn("Could not find channel for search keyword. ChannelName = {0}, Keyword = {1}", LogSource.Sync, channelname, keyword);
					return;
				}

				#endregion

				// Perform keyword search
				try
				{
					var parser = new UserStatusParser(channel.Configuration, kw);
					var updates = channel.StatusUpdatesChannel.GetUpdates(keyword, 50);

					foreach (var statusupdate in updates)
					{
						Logger.Debug("Received search update. ChannelStatusKey = {0}, DatePosted = {1}", LogSource.Sync, statusupdate.ChannelStatusKey, statusupdate.DatePosted);

						parser.ProcessStatusUpdate(statusupdate, StatusTypes.SearchUpdate);
					}
				}
				catch (Exception ex)
				{
					Logger.Error("An error has occured while getting search result from {0}. Keyword = {1} Exception = {2}", LogSource.Sync, channelname, keyword, ex);
				}
			}

			// Save last run time
			ChannelContext.Current.ClientContext.SaveSetting("LastSyncSearchStreamDate", DateTime.Now);

			EventBroker.Publish(AppEvents.SyncStatusUpdatesFinished);
		}
	}
}
