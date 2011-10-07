using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Threading;
using Inbox2.Platform.Logging;

namespace Inbox2.Core.Threading
{
	public class TaskProcessor : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected readonly AutoResetEvent signal;
		protected readonly Thread runnerThread;

		protected bool isFree = true;
		protected bool shutdown = false;

		protected BackgroundTask queuedBackgroundTask;

		protected Action<IBackgroundTask> processingStarted;
		protected Action<IBackgroundTask> processingFinished;

		public string Name
		{
			get { return runnerThread.Name; }
		}

		public BackgroundTask QueuedBackgroundTask
		{
			get { return queuedBackgroundTask; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is processing.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is processing; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsFree
		{
			get { return isFree; }
		}

		/// <summary>
		/// Gets a value indicating whether requires the STA appartment state.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if requires STA appartment state; otherwise, <c>false</c>.
		/// </value>
		public virtual bool RequiresSTAAppartmentState
		{
			get { return false; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskProcessor"/> class.
		/// </summary>
		public TaskProcessor(Action<IBackgroundTask> processingStarted, Action<IBackgroundTask> processingFinished, int threadNr)
		{
			this.processingStarted = processingStarted;
			this.processingFinished = processingFinished;

			this.signal = new AutoResetEvent(false);

			this.runnerThread = new Thread(Heartbeat);
			this.runnerThread.Name = "Processing Thread " + threadNr;
			this.runnerThread.IsBackground = true;
			this.runnerThread.Priority = ThreadPriority.AboveNormal;

			if (this.RequiresSTAAppartmentState)
				this.runnerThread.SetApartmentState(ApartmentState.STA);

			this.runnerThread.Start();
		}

		/// <summary>
		/// Processes the specified work item.
		/// </summary>
		/// <param name="backgroundTask">The work item.</param>
		public void Process(BackgroundTask backgroundTask)
		{
			if (backgroundTask == null)
			{
				Debug.Fail("Not allowed to be null");

				return;
			}

			Trace.WriteLine("Signaling processing thread to process the queued task");

			queuedBackgroundTask = backgroundTask;
			OnPropertyChanged("QueuedBackgroundTask");

			ToggleStatus(false);

			// Signal our processing thread to process the queued task
			signal.Set();
		}

		/// <summary>
		/// Shutdowns this instance.
		/// </summary>
		public void Shutdown()
		{
			if (!isFree)
			{
				// Kill running task
				if (queuedBackgroundTask is IDisposable)
					(queuedBackgroundTask as IDisposable).Dispose();

				if (runnerThread.IsAlive)
					runnerThread.Abort();

				return;
			}

			shutdown = true;
			signal.Set();
		}

		/// <summary>
		/// Heartbeat thread, waits for signal that something has been queued for processing.
		/// </summary>
		protected virtual void Heartbeat()
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

			do
			{
				if (shutdown)
					return;

				Trace.WriteLine("Processor idle, waiting for signal...");

				// Wait for the signal to be set
				signal.WaitOne();

				Trace.WriteLine("Got signal, processing task");

				// Exit heartbeat
				if (shutdown)
					return;

				try
				{
					queuedBackgroundTask.OnStarted();

					queuedBackgroundTask.Execute();

					queuedBackgroundTask.OnSuccess();					
				}
				//catch (ThreadAbortException)
				//{
				//    Thread.ResetAbort();
				//}
				catch (Exception ex)
				{
					try
					{
						Logger.Error("An error has occured while executing the task {0}, Exception = {1}", LogSource.TaskQueue, this, ex);

						queuedBackgroundTask.OnFailure();										
					}
					catch (Exception ex1)
					{
						Logger.Error("A fatal background task exception has occured. Exception = {0}", LogSource.TaskQueue, ex1);
					}
				}
				finally
				{
					try
					{
						queuedBackgroundTask.OnCompleted();

						ToggleStatus(true);

						queuedBackgroundTask = null;
						OnPropertyChanged("QueuedBackgroundTask");
					}
					catch (Exception ex)
					{
						Logger.Error("A fatal background task exception has occured. Exception = {0}", LogSource.TaskQueue, ex);
					}
				}
			}
			while (true);
		}

		/// <summary>
		/// Toggles the status.
		/// </summary>
		/// <param name="status">if set to <c>true</c> [status].</param>
		private void ToggleStatus(bool status)
		{
			if (status)
				processingFinished(queuedBackgroundTask);
			else
				processingStarted(queuedBackgroundTask);

			isFree = status;
		}

		/// <summary>
		/// Called when [propertyproperty changed].
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				Thread.CurrentThread.RaiseUIPropertyChanged(this, propertyName, PropertyChanged);
			}
		}
	}
}
