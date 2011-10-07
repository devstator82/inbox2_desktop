using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Threading;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Core.Threading
{
	[Export(typeof(ITaskQueue))]
	public class TaskQueue : ITaskQueue
	{
		public ProcessingPool ProcessingPool { get; private set; }
		public List<IBackgroundTask> Tasks { get; private set; }

		protected object syncroot = new object();

		public int Count
		{
			get
			{
				lock (syncroot)
				{
					return Tasks.Count(t => t.ExecutionStatus == ExecutionStatus.Pending
						&& t.ExecuteAfter < DateTime.Now);
				}
			}
		}

		public TaskQueue()
		{
			Tasks = new List<IBackgroundTask>();
			ProcessingPool = new ProcessingPool(this);
		}

		/// <summary>
		/// Enqueues the specified work item.
		/// </summary>
		/// <param name="backgroundTask">The work item.</param>
		public void Enqueue(IBackgroundTask backgroundTask)
		{
			var task = (BackgroundTask)backgroundTask;

			lock (syncroot)
			{
				if (backgroundTask.MaxQueuedInstances == GetQueuedInstances(backgroundTask.GetType(), (BackgroundTask)backgroundTask))
				{
					Trace.WriteLine(String.Format("Task {0} has allready reached maximum allowed instances in the queue, ignoring enqueue...", backgroundTask.GetType()));

					task.OnFinished();
					return;
				}

				// Else
				Tasks.Add(task);
			}

			// Signal the processing pool to start (if it wasn't allready running)
			ProcessingPool.Process();
		}

		/// <summary>
		/// Dequeues this instance.
		/// </summary>
		/// <returns></returns>
		public IBackgroundTask Dequeue()
		{
			IBackgroundTask task = null;

			lock (syncroot)
			{
				if (Tasks.Count(t => t.ExecutionStatus == ExecutionStatus.Pending
					&& t.ExecuteAfter < DateTime.Now) > 0)
				{
					task = Tasks.First(t => t.ExecutionStatus == ExecutionStatus.Pending
						&& t.ExecuteAfter < DateTime.Now);

					task.ExecutionStatus = ExecutionStatus.Submitted;
				}
			}

			// Might be null, by design
			return task;
		}		

		public void Remove(IBackgroundTask task)
		{
			lock (syncroot)
			{
				Tasks.Remove(task);
			}
		}

		public T Find<T>() where T : IBackgroundTask
		{
			lock (syncroot)
			{
				return (T)Tasks.FirstOrDefault(t => t.GetType() == typeof(T));
			}
		}

		/// <summary>
		/// Determines whether the specified task type is queued.
		/// </summary>
		/// <param name="taskType">Type of the task.</param>
		/// <returns>
		/// 	<c>true</c> if the specified task type is queued; otherwise, <c>false</c>.
		/// </returns>
		public int GetQueuedInstances(Type taskType, BackgroundTask instance)
		{
			return Tasks.Count(t => t.GetType() == taskType
				&& t.SingletonKey == instance.SingletonKey
				&& (t.ExecutionStatus == ExecutionStatus.Pending
				|| t.ExecutionStatus == ExecutionStatus.Submitted
				|| t.ExecutionStatus == ExecutionStatus.Running));
		}


		/// <summary>
		/// Gets the currently running instances.
		/// </summary>
		/// <param name="taskType">Type of the task.</param>
		/// <param name="instance">The instance.</param>
		/// <returns></returns>
		public int GetRunningInstances(Type taskType, BackgroundTask instance)
		{
			return Tasks.Count(t => t.GetType() == taskType
				&& t.SingletonKey == instance.SingletonKey
				&& (t.ExecutionStatus == ExecutionStatus.Running));
		}
	}
}
