using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Inbox2ClientWorker.Core.Threading.Tasks.Sync
{
	class SyncTask : BackgroundTask
	{
		private readonly IDataService dataService;		

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
			dataService = ClientState.Current.DataService;
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

			foreach (var instance in channels)
			{
				if (instance.ContactsChannel != null)
					gate.Add(new SyncContactsTask(instance.Configuration, instance.ContactsChannel.Clone()) { OverrideCanExecute = OverrideCanExecute });

				if (instance.StatusUpdatesChannel != null)
					gate.Add(new SyncStatusUpdatesTask(instance.Configuration, instance.StatusUpdatesChannel.Clone()) { OverrideCanExecute = OverrideCanExecute });

				//if (instance.CalendarChannel != null)
				//    new SyncCalendarTask(instance.Configuration, instance.CalendarChannel) { OverrideCanExecute = OverrideCanExecute }.ExecuteAsync();
			}

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
