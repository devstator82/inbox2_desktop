using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Core.Threading.Commands;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Threading;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Platform.Logging;

namespace Inbox2.Core.Threading.Tasks.Commands
{
	public class ExecuteCommandsTask : BackgroundTask
	{
		private readonly ExecutionTrigger trigger;

		public ExecuteCommandsTask(ExecutionTrigger trigger)
		{
			this.trigger = trigger;
		}

		protected override void ExecuteCore()
		{
			var commands =
				ClientState.Current.DataService.SelectAllBy<QueuedCommand>(
					new { TriggerType = trigger.ToString(), Status = ExecutionStatus.Pending.ToString() }).ToList();

			if (commands.Count == 0)
				return;

			Logger.Debug("{0} commands queued with triggger {1}", LogSource.Command, commands.Count, trigger);

			foreach (var command in commands)
			{
				if (DateTime.Now > command.DateScheduled)
				{
					var cmdObject = CommandFactory.CreateCommand(command);

					if (cmdObject.CanExecute)
					{
						// Lock this task (prevents execution by any other task)
						command.Status = ExecutionStatus.Submitted;
						ClientState.Current.DataService.Update(command);

						var task = new BackgroundActionTask(cmdObject.Execute);

						// Create new task for command
						Logger.Debug("Creating task for command {0}", LogSource.Command, command);

						// Update timestamp on started
						QueuedCommand command1 = command;

						task.Started += delegate
						{
							Logger.Debug("Command {0} started", LogSource.Command, command1);

							command1.Status = ExecutionStatus.Submitted;
							command1.DateStarted = DateTime.Now;

							ClientState.Current.DataService.Update(command1);
						};

						// Update status on success
						task.FinishedSuccess += delegate
						{
							Logger.Debug("Command {0} finished successfully", LogSource.Command, command1);

							command1.Status = ExecutionStatus.Success;

							ClientState.Current.DataService.Update(command1);
						};

						// Update status on failure
						task.FinishedFailure += delegate
						{
							Logger.Debug("Command {0} finished with failure", LogSource.Command, command1);

							command1.Status = (command1.ActualRetries < command1.MaxRetries) ? ExecutionStatus.Pending : ExecutionStatus.Error;
							command1.ActualRetries++;

							ClientState.Current.DataService.Update(command1);
						};

						task.ExecuteAsync();
					}
				}
			}
		}
	}
}