using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Media.Imaging;

namespace Inbox2.Framework.UI.AsyncImage
{
	public static class AsyncImageQueue
	{
		/// <summary>
		/// Default image that gets shown when image is still loading.
		/// </summary>
		public static BitmapImage _HourGlass;
		public static BitmapImage _Profile;

		private static Thread readerThread;
		private static Timer cleanupTimer;
		private static Queue<IAsyncImage> queue;
		private static AutoResetEvent signal;
		private static object synclock;

		private static List<IAsyncImage> loaded;

		static AsyncImageQueue()
		{
			_HourGlass = new BitmapImage(new Uri("pack://application:,,,/Inbox2.UI.Resources;component/icons/HourGlass.png"));
			_HourGlass.Freeze();

			_Profile = new BitmapImage(new Uri("pack://application:,,,/Inbox2.UI.Resources;component/icons/icon_person.jpg"));
			_Profile.Freeze();

			signal = new AutoResetEvent(false);
			queue = new Queue<IAsyncImage>();
			loaded = new List<IAsyncImage>();
			synclock = new object();

			readerThread = new Thread(HeartBeat)
			    {
			        Name = "Background Image Loading Thread",
			        IsBackground = true,
			        Priority = ThreadPriority.BelowNormal
			    };
		    readerThread.Start();

			cleanupTimer = new Timer(Cleanup, null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
		}

		static void HeartBeat()
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

			do
			{
				// Wait for the signal to be set
				signal.WaitOne();

				IAsyncImage entry;

				lock (synclock)
					entry = queue.Count > 0 ? queue.Dequeue() : null;

				while (entry != null)
				{
					entry.Load();

					lock (synclock)
					{
						loaded.Add(entry);
						entry = queue.Count > 0 ? queue.Dequeue() : null;						
					}
				}
			}
			while (true);
		}

		static void Cleanup(object state)
		{
			List<IAsyncImage> cleanup;

			lock (synclock)
				cleanup = loaded.Where(i => i.Accessed <= DateTime.Now.AddMinutes(1).Ticks).ToList();

			cleanup.ForEach(i => i.Unload());

			lock (synclock)
				cleanup.ForEach(i => loaded.Remove(i));
		}

		internal static void Enqueue(IAsyncImage image)
		{
			lock(synclock)
				queue.Enqueue(image);

			signal.Set();
		}
	}
}


