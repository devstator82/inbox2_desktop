using System;
using System.Collections.Generic;
using System.Linq;
using Timer=System.Timers.Timer;

namespace Inbox2.Core.Threading.Repeat
{
	public static class ScheduledTaskRunner
	{
		private static Timer _Timer;
		private static List<ScheduledItem> _Runs = new List<ScheduledItem>();
		private static object SyncLock = new object();

		static ScheduledTaskRunner()
		{
			_Timer = new Timer(500);
			_Timer.Elapsed += Timer_Elapsed;
			_Timer.Start();
		}

		static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			var runs = new List<ScheduledItem>();

			lock (SyncLock)
			{
				runs.AddRange(_Runs.Where(r => DateTime.Now > r.DateScheduled));

				runs.ForEach(r => _Runs.Remove(r));
			}

			runs.ForEach(r => r.Execute());
		}

		public static void Enqueue(ScheduledItem scheduledItem)
		{
			lock (SyncLock)
			{
				_Runs.RemoveAll(r => r.Key == scheduledItem.Key);

				_Runs.Add(scheduledItem);
			}
		}

		public static void Dequeue(ScheduledItem scheduledItem)
		{
			lock (SyncLock)
			{
				_Runs.Remove(scheduledItem);
			}
		}
	}
}