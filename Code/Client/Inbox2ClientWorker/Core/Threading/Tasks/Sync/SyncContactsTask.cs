using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Inbox2.Core.Configuration;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Threading;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2ClientWorker.Core.Threading.Handlers.Matchers;

namespace Inbox2ClientWorker.Core.Threading.Tasks.Sync
{
	class SyncContactsTask : BackgroundTask
	{
		private readonly ChannelConfiguration config;
		private IClientContactsChannel channel;

		public override int MaxQueuedInstances
		{
			get { return 1; }
		}

		public override string SingletonKey
		{
			get { return config.ChannelId.ToString(); }
		}

		public SyncContactsTask(ChannelConfiguration config, IClientContactsChannel channel)
		{
			this.config = config;
			this.channel = channel;
		}

		protected override bool CanExecute()
		{
			ChannelContext.Current.ClientContext = new ChannelClientContext(ClientState.Current.Context, config);

			var lastRun = ChannelContext.Current.ClientContext.GetSetting("LastSyncContactsDate");

			return String.IsNullOrEmpty(lastRun.ToString()) ||
					DateTime.Now.Subtract(((DateTime)lastRun)).TotalMinutes >
						SettingsManager.ClientSettings.AppConfiguration.ReceiveConfiguration.SyncContactsInterval;
		}

		protected override void ExecuteCore()
		{
			try
			{
				foreach (var channelContact in channel.GetContacts())
				{
					channelContact.Person.SourceChannelId = config.ChannelId;
					channelContact.Profile.SourceChannelId = config.ChannelId;

					new ContactMatcher(channelContact).Execute();
				}

				channel.Dispose();

				// Save last run time
				ChannelContext.Current.ClientContext.SaveSetting("LastSyncContactsDate", DateTime.Now);

				EventBroker.Publish(AppEvents.SyncContactsFinished);
			}
			finally
			{
				channel = null;
			}
		}
	}
}
