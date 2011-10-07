using System;
using Inbox2.Core.Configuration;
using Inbox2ClientWorker.Core.Threading.Tasks.Receive;
using Inbox2ClientWorker.Core.Threading.Tasks.Sync;
using Inbox2.Platform.Interfaces.ValueTypes;

namespace Inbox2ClientWorker.Core.Threading.Tasks
{
	public class Tasks
	{
		public static void Receive()
		{		
			new ReceiveTask(new ReceiveRange { OnlyNew = true, PageSize = DebugKeys.DefaultPageSize }).ExecuteAsync();
		}

		public static void ReceivePrio()
		{		
			new ReceiveTask(new ReceiveRange { OnlyNew = true, PageSize = DebugKeys.DefaultPageSize }) { OverrideCanExecute = true }.ExecuteAsync();
		}

		public static void ReceivePage(int pageSize)
		{
			new ReceiveTask(new ReceiveRange { OnlyNew = false, PageSize = pageSize }).ExecuteAsync();
		}

		public static void Sync()
		{
			new SyncTask().ExecuteAsync();
		}

		public static void SyncPrio()
		{
			new SyncTask { OverrideCanExecute = true }.ExecuteAsync();			 
		}		
	}
}