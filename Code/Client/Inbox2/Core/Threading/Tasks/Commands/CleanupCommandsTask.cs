using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.Threading;

namespace Inbox2.Core.Threading.Tasks.Commands
{
	public class CleanupCommandsTask : BackgroundTask
	{
		protected override void ExecuteCore()
		{
			// Delete completed tasks or tasks with errors and retries >= MaxRetries
			ClientState.Current.DataService.ExecuteNonQuery("delete from CommandQueue where Status='Success' or (Status='Error' and ActualRetries >= MaxRetries) ");

			// Unblock pending commands whose execution has exceeded 3 minutes and is still pending	
			var command = ClientState.Current.DataService.CreateCommand();
			var parameter = ClientState.Current.DataService.CreateParameter();

			parameter.ParameterName = "@DateScheduled";
			parameter.Value = DateTime.Now.AddMinutes(-3);

			command.CommandText = "update CommandQueue set Status='Pending' where Status='Running' and DateScheduled > @DateScheduled";
			command.Parameters.Add(parameter);

			ClientState.Current.DataService.ExecuteNonQuery(command);
		}
	}
}
