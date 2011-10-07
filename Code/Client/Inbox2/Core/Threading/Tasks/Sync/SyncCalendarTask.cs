using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Threading;
using Inbox2.Framework.Threading.Progress;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Core.Threading.Tasks.Sync
{
	class SyncCalendarTask : BackgroundTask
	{
		private readonly ChannelConfiguration config;
		private readonly IClientCalendarChannel channel;

		public override int MaxQueuedInstances
		{
			get { return 1; }
		}

		public override string SingletonKey
		{
			get { return channel.ChannelName; }
		}

		protected IClientCalendarChannel Channel
		{
			get { return channel; }
		}

		public SyncCalendarTask(ChannelConfiguration config, IClientCalendarChannel channel)
		{
			this.config = config;
			this.channel = channel;
		}

		protected override void ExecuteCore()
		{
			ExecuteOnUIThread(() => ProgressManager.Current.Register(ProgressGroup));

			ProgressGroup.Status = "Connecting...";
			ProgressGroup.SourceChannelId = config.ChannelId;

			try
			{
				foreach (var calendar in channel.GetCalendars())
				{
					foreach (var channelEvent in channel.GetEvents(calendar))
					{
						Event evt = channelEvent.DuckCopy<Event>();

					    evt.EventFolder = Folders.Inbox;

						if (!HasEventBy(config.ChannelId, evt.ChannelEventKey))
						{
							evt.SourceChannelId = config.ChannelId;

							EventBroker.Publish(AppEvents.CalendarEventReceived, evt);
						}
					}
				}
			}
			finally
			{
				ProgressGroup.IsCompleted = true;
			}
		}

		bool HasEventBy(long sourceChannelId, string id)
		{
			var command = ClientState.Current.DataService.CreateCommand();

			command.CommandText = "SELECT COUNT(*) FROM Events WHERE [SourceChannelId]=@SourceChannelId AND [ChannelEventKey]=@ChannelEventKey";
			command.Parameters.Add(ClientState.Current.DataService.CreateParameter("@SourceChannelId", sourceChannelId));
			command.Parameters.Add(ClientState.Current.DataService.CreateParameter("@ChannelEventKey", id));

			return ClientState.Current.DataService.ExecuteScalar<long>(command) > 0;
		}
	}
}
