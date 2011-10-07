using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Inbox2.Framework.Threading
{
	public class ConcurrencyGate
	{
		private int concurrency;
		private int running;
		private readonly Stack<BackgroundTask> tasks;
		private readonly object synclock;
		private readonly ManualResetEvent waitHandle;

		public ConcurrencyGate(int concurrency)
		{
			this.concurrency = concurrency;

			tasks = new Stack<BackgroundTask>();
			waitHandle = new ManualResetEvent(false);
			synclock = new object();
		}

		public void Add(BackgroundTask task)
		{
			task.Finished += TaskFinished;			

			lock (synclock)
				tasks.Push(task);
		}

		void TaskFinished(object sender, EventArgs e)
		{
			((BackgroundTask) sender).Finished -= TaskFinished;

			CheckGate(); 
		}

		public void Execute()
		{
			WaitHandle.WaitAll(new WaitHandle[] { ExecuteAsync() });
		}

		ManualResetEvent ExecuteAsync()
		{
			concurrency = concurrency <= 0 ? tasks.Count : concurrency;

			if (tasks.Count == 0)
			{
				waitHandle.Set();

				return waitHandle;
			}

			for (int i = 0; i < concurrency; i++)
			{
				if (tasks.Count > 0)
				{
					Interlocked.Increment(ref running);

					lock (synclock)
						tasks.Pop().ExecuteAsync();
				}
			}

			return waitHandle;
		}

		void CheckGate()
		{
			if (Interlocked.Decrement(ref running) < concurrency)
			{
				int count;

				lock (synclock)
					count = tasks.Count;
				
				if ((count + running) == 0)
				{
					waitHandle.Set();

					return;
				}

				if (count > 0)
				{
					Interlocked.Increment(ref running);

					lock (synclock)
						tasks.Pop().ExecuteAsync();
				}
			}
		}
	}
}
