using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Inbox2.Core.Configuration;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Threading;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Core.Threading
{
	public class ProcessingPool
	{
		private readonly TaskQueue _queue;
		protected AutoResetEvent signal;
		protected Thread schedulingThread;
		protected List<TaskProcessor> processors;
		protected bool shutdown;
		protected object syncroot = new object();

		public int Free
		{
			get
			{
				lock(syncroot)
					return processors.Count(p => p.IsFree);
			}
		}

		public List<TaskProcessor> Processors
		{
			get { return processors; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessingPool"/> class.
		/// </summary>
		public ProcessingPool(TaskQueue queue)
		{
			_queue = queue;
			processors = new List<TaskProcessor>(DebugKeys.DefaultNrOfProcessors * 2);

			for (int i = 0; i < DebugKeys.DefaultNrOfProcessors; i++)
				processors.Add(new TaskProcessor(OnProcessingStarted, OnProcessingFinished, i));

			signal = new AutoResetEvent(false);

			schedulingThread = new Thread(Scheduler);
			schedulingThread.Name = "Background Scheduling Thread";
			schedulingThread.IsBackground = true;
			schedulingThread.Priority = ThreadPriority.BelowNormal;
			schedulingThread.Start();
		}

		/// <summary>
		/// Processes this instance.
		/// </summary>
		public void Process()
		{
			signal.Set();
		}

		public bool HasRunning<T>()
		{
			lock (syncroot)
			{
				return Processors.Any(p => p.QueuedBackgroundTask != null &&
										   p.QueuedBackgroundTask.GetType() == typeof(T));
			}
		}

		/// <summary>
		/// Kills all processors that are running tasks of the given type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public void Kill<T>()
		{
			lock(syncroot)
			{
				foreach (var processor in
					Processors.Where(p => p.QueuedBackgroundTask != null && 
							p.QueuedBackgroundTask.GetType() == typeof(T))
						.ToList())
				{
					try
					{
						processor.Shutdown();
					}
					catch (Exception)
					{
					}
					finally
					{
						// Add a new processor to replace the one we just killed
						processors.Add(new TaskProcessor(OnProcessingStarted, OnProcessingFinished, processors.Count));
					}
				}
			}
		}

		/// <summary>
		/// Shutdowns this instance.
		/// </summary>
		public void Shutdown()
		{
			shutdown = true;
			signal.Set();

			lock (syncroot)
				foreach (var processor in processors)
					processor.Shutdown();
		}

		/// <summary>
		/// Scheduler thread, waits for signal that something has been queued for processing.
		/// </summary>
		private void Scheduler()
		{
			do
			{
				// Wait for the signal to be set
				signal.WaitOne();

				if (shutdown)
					return;

				TaskProcessor processor = GetAvailableProcessor();

				while (processor != null)
				{
					// If no more items left for processing, break out of inner loop
					if (ClientState.Current.TaskQueue.Count == 0)
						break;

					// Process segment into free slot
					processor.Process((BackgroundTask) ClientState.Current.TaskQueue.Dequeue());

					// Get the next available slot
					processor = GetAvailableProcessor();
				}
			}
			while (true);
		}

		/// <summary>
		/// Gets the available processor.
		/// </summary>
		/// <returns></returns>
		protected TaskProcessor GetAvailableProcessor()
		{
			lock (syncroot)
			{
				// If we have a free processor, return it
				if (processors.Count(p => p.IsFree) > 0)
				{
					return processors.First(p => p.IsFree);
				}

				// Else return null
				return null;
			}
		}

		/// <summary>
		/// Called when [processing started].
		/// </summary>
		protected void OnProcessingStarted(IBackgroundTask task)
		{
			
		}

		/// <summary>
		/// Called when [processing finished].
		/// </summary>
		protected void OnProcessingFinished(IBackgroundTask task)
		{
			// Remove item from queue to cleanup any held references
			if (task.ExecutionStatus == ExecutionStatus.Error || task.ExecutionStatus == ExecutionStatus.Success)
				_queue.Remove(task);
		}
	}
}
