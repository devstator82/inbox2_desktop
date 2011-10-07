using System;
using System.Net.NetworkInformation;
using Inbox2.Core.Threading.Tasks.Application;
using Inbox2.Core.Threading.Tasks.Commands;
using Inbox2.Core.Threading.Tasks.Receive;
using Inbox2.Core.Threading.Tasks.Sync;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Interfaces.ValueTypes;
using DebugKeys=Inbox2.Core.Configuration.DebugKeys;

namespace Inbox2.Core.Threading.Tasks
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

		public static void Receive(ChannelInstance channel)
		{
			new ReceiveTask(new ReceiveRange { OnlyNew = true, PageSize = DebugKeys.DefaultPageSize }, channel) { OverrideCanExecute = true }.ExecuteAsync();
		}

		public static void ReceivePage()
		{
			ReceivePage(DebugKeys.DefaultPageSize);
		}

		public static void ReceivePage(int pageSize)
		{
			new ReceiveTask(new ReceiveRange { OnlyNew = false, PageSize = pageSize }).ExecuteAsync();
		}

		public static void Commands()
		{
			if (NetworkInterface.GetIsNetworkAvailable())
				new ExecuteCommandsTask(ExecutionTrigger.Connection).ExecuteAsync();
		}

		public static void Send()
		{
			new ExecuteCommandsTask(ExecutionTrigger.Send).ExecuteAsync();
		}

		public static void Sync()
		{
			new SyncTask().ExecuteAsync();
		}

		public static void SyncPrio()
		{
			new SyncTask { OverrideCanExecute = true }.ExecuteAsync();			 
		}

		public static void Sync(ChannelInstance channel)
		{
			new SyncTask(channel) { OverrideCanExecute = true }.ExecuteAsync();
		}

		public static void Cleanup()
		{
			new CleanupCommandsTask().ExecuteAsync();
		}

		public static void CheckForUpdate()
		{
			new CheckForUpdateTask().ExecuteAsync();
		}

		public static void ShipLogs()
		{
			new ShipLogTask().ExecuteAsync();
		}

		public static void PurgeData()
		{
			new PurgeDataTask().ExecuteAsync();
		}
	}
}