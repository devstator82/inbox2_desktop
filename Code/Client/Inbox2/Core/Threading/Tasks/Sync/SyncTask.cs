using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Inbox2.Core.Configuration;
using Inbox2.Core.Threading.Repeat;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Threading;
using Inbox2.Platform.Channels;

namespace Inbox2.Core.Threading.Tasks.Sync
{
	class SyncTask : BackgroundTask
	{
		private ChannelInstance channel;

		public override int MaxQueuedInstances
		{
			get { return 1; }
		}

		public override bool RequiresNetworkConnection
		{
			get { return true; }
		}

		public SyncTask()
		{
		}

		public SyncTask(ChannelInstance channel)
		{
			this.channel = channel;
		}

		protected override void ExecuteCore()
		{
			ClientState.Current.Messages.Errors.Clear();

			if (ChannelsManager.Channels.Count == 0)
				return;

			var gate = new ConcurrencyGate(2);
			var channels = ListOrSingle<ChannelInstance>.Get(channel, ChannelsManager.Channels);

			foreach (var instance in channels.Where(c => !c.Configuration.IsConnected))
			{
				if (instance.ContactsChannel != null)
					gate.Add(new SyncContactsTask(instance.Configuration, instance.ContactsChannel.Clone()) { OverrideCanExecute = OverrideCanExecute });

				if (instance.StatusUpdatesChannel != null)
					gate.Add(new SyncStatusUpdatesTask(instance.Configuration, instance.StatusUpdatesChannel.Clone()) { OverrideCanExecute = OverrideCanExecute });

				//if (instance.CalendarChannel != null)
				//    new SyncCalendarTask(instance.Configuration, instance.CalendarChannel) { OverrideCanExecute = OverrideCanExecute }.ExecuteAsync();
			}

			if (channel == null)
				gate.Add(new SyncSearchStreamTask { OverrideCanExecute = OverrideCanExecute });

			gate.Execute();

			EventBroker.Publish(AppEvents.SyncFinished);
		}

		public override void OnCompleted()
		{
			// Schedule next execution
			new Run("Sync").After(1).Minutes().Call(Tasks.Sync);

			base.OnCompleted();
		}
	}
}
