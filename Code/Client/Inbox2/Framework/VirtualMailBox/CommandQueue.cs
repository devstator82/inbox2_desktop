using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Framework.CloudApi.Enumerations;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Framework.VirtualMailBox
{
	public static class CommandQueue
	{
		public static void Enqueue(AppCommands commandType, IEntityBase entity)
		{
			Enqueue(commandType, entity, ExecutionTrigger.Send, String.Empty, ModifyAction.None);
		}

		public static void Enqueue(AppCommands commandType, IEntityBase entity, ExecutionTrigger trigger, object value, ModifyAction modifyAction)
		{
			var command = new QueuedCommand
			{
				CommandType = commandType,
				Target = entity.UniqueId,
				Status = ExecutionStatus.Pending,
				TriggerType = trigger,
				Value = value.ToString(),
				ModifyAction = modifyAction,
				DateCreated = DateTime.Now,
				DateScheduled = DateTime.Now
			};

			ThreadPool.QueueUserWorkItem(delegate
			{
				ClientState.Current.DataService.Save(command);

				switch (trigger)
				{
					case ExecutionTrigger.Connection:
						EventBroker.Publish(AppEvents.RequestCommands);
						break;

					case ExecutionTrigger.Send:
						EventBroker.Publish(AppEvents.RequestSend);
						break;

					case ExecutionTrigger.Receive:
						EventBroker.Publish(AppEvents.RequestReceive);
						break;
				}
			});
		}
	}
}
