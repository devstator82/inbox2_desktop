using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Inbox2.Framework.UI
{
	internal static class AsyncHttpQueue
	{
		private static Thread readerThread;
		private static Queue<AsyncHttpImage> queue;
		private static AutoResetEvent signal;
		private static object synclock;

		static AsyncHttpQueue()
		{
			signal = new AutoResetEvent(false);
			queue = new Queue<AsyncHttpImage>();
			synclock = new object();

			readerThread = new Thread(HeartBeat);
			readerThread.Name = "Background Image Loading Thread";
			readerThread.IsBackground = true;
			readerThread.Priority = ThreadPriority.BelowNormal;
			readerThread.Start();
		}

		static void HeartBeat()
		{
			do
			{
				// Wait for the signal to be set
				signal.WaitOne();

				AsyncHttpImage entry;

				lock (synclock)
					entry = queue.Count > 0 ?
						queue.Dequeue() : null;

				while (entry != null)
				{
					entry.Load();

					lock (synclock)
						entry = queue.Count > 0 ?
							queue.Dequeue() : null;
				}
			}
			while (true);
		}

		internal static void Enqueue(AsyncHttpImage image)
		{
			lock(synclock)
				queue.Enqueue(image);

			signal.Set();
		}
	}
}
