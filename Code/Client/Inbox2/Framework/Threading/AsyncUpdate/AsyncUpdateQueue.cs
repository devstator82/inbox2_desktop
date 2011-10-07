using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Inbox2.Framework.Interfaces;

namespace Inbox2.Framework.Threading.AsyncUpdate
{
	public static class AsyncUpdateQueue
	{
		private static Thread readerThread;
		private static Queue<IEntityBase> queue;
		private static AutoResetEvent signal;
		private static object synclock;

		static AsyncUpdateQueue()
		{
			signal = new AutoResetEvent(false);
			queue = new Queue<IEntityBase>();
			synclock = new object();

			readerThread = new Thread(HeartBeat)
				{
					Name = "Background Entity Update Thread",
					IsBackground = true,
					Priority = ThreadPriority.BelowNormal
				};
			readerThread.Start();
		}

		static void HeartBeat()
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

			do
			{
				// Wait for the signal to be set
				signal.WaitOne();

				IEntityBase entry;

				lock (synclock)
					entry = queue.Count > 0 ? queue.Dequeue() : null;

				while (entry != null)
				{
					ClientState.Current.DataService.Update(entry);

					lock (synclock)
					{
						entry = queue.Count > 0 ? queue.Dequeue() : null;						
					}
				}
			}
			while (true);
		}

		public static void Enqueue(IEntityBase entity)
		{
			lock(synclock)
				queue.Enqueue(entity);

			signal.Set();
		}
	}
}


