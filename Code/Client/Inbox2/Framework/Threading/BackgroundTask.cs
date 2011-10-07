using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Threading.Progress;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Logging;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Framework.Threading
{
	public abstract class BackgroundTask : INotifyPropertyChanged, IBackgroundTask, IDisposable
	{
		#region Fields

		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler<EventArgs> Started;
		public event EventHandler<EventArgs> Finished;
		public event EventHandler<EventArgs> FinishedSuccess;
		public event EventHandler<EventArgs> FinishedFailure;

		private readonly ManualResetEvent waitHandle;
		private Exception lastException;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the execution status.
		/// </summary>
		/// <value>The execution status.</value>
		public ExecutionStatus ExecutionStatus { get; set; }

		/// <summary>
		/// Gets the max living instances allowed in the queue.
		/// </summary>
		/// <value>The max living instances.</value>
		public virtual int MaxQueuedInstances
		{
			get { return Int32.MaxValue; }
		}

		/// <summary>
		/// Gets the singleton key.
		/// </summary>
		/// <value>The singleton key.</value>
		public virtual string SingletonKey
		{
			get { return String.Empty; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is long lived.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is long lived; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsLongLived
		{
			get { return false; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance needs an active network connection to run.
		/// </summary>
		public virtual bool RequiresNetworkConnection
		{
			get { return false; }
		}

		/// <summary>
		/// Gets or sets the minimum datetime to execute this task.
		/// </summary>
		/// <value>The execute after.</value>
		public DateTime ExecuteAfter { get; set; }

		/// <summary>
		/// Gets or sets the progress group.
		/// </summary>
		/// <value>The progress group.</value>
		public ProgressGroup ProgressGroup { get; set; }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get
			{
				string[] parts = GetType().ToString().Split('.');

				return parts.Last();
			}
		}

		/// <summary>
		/// If this is set to true then we won't execute the CanExecute method.
		/// </summary>
		public bool OverrideCanExecute { get; set; }

		public Exception LastException
		{
			get { return lastException; }
		}

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BackgroundTask"/> class.
		/// </summary>
		protected BackgroundTask()
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

			ExecutionStatus = ExecutionStatus.Pending;
			ExecuteAfter = DateTime.Now.AddSeconds(-1);
			waitHandle = new ManualResetEvent(false);
		}

		#endregion

		#region Methods

		protected virtual bool CanExecute()
		{
			return true;
		}

		/// <summary>
		/// Executes this task synchronously.
		/// </summary>
		public void Execute()
		{
			Logger.Debug("Executing tasks [{0}]", LogSource.BackgroundTask, GetType());

			try 
			{
				// Assign the client-context to the current thread
				ChannelContext.Current.ClientContext = ClientState.Current.Context;

				bool canExecute = CanExecute();

				if (RequiresNetworkConnection)
					canExecute = NetworkInterface.GetIsNetworkAvailable();

				if (canExecute || OverrideCanExecute)
				{
					ExecuteCore();
				}
			}
			catch (Exception ex)
			{
				Logger.Debug("An error has occured while executing task. Task = {0}, Exception = {1}", LogSource.BackgroundTask, GetType(), ex);

				lastException = ex;

				throw;
			}

			Logger.Debug("Finished executing tasks [{0}]", LogSource.BackgroundTask, GetType());
		}

		public WaitHandle ExecuteAsync()
		{
			ClientState.Current.TaskQueue.Enqueue(this);

			return waitHandle;
		}

		public void Execute(object async)
		{
			Execute();
		}

		public void OnStarted()
		{
			ExecutionStatus = ExecutionStatus.Running;

			OnPropertyChanged("ExecutionStatus");

			if (Started != null)
			{
				Started(this, EventArgs.Empty);
			}
		}

		public virtual void OnSuccess()
		{
			ExecutionStatus = ExecutionStatus.Success;

			OnPropertyChanged("ExecutionStatus");

			if (FinishedSuccess != null)
			{
				FinishedSuccess(this, EventArgs.Empty);
			}

			waitHandle.Set();
		}

		public virtual void OnFailure()
		{
			ExecutionStatus = ExecutionStatus.Error;

			OnPropertyChanged("ExecutionStatus");

			if (FinishedFailure != null)
			{
				FinishedFailure(this, EventArgs.Empty);
			}

			waitHandle.Set();
		}
	
		public virtual void OnCompleted()
		{
			OnFinished();
		}

		public void OnFinished()
		{
			if (Finished != null)
			{
				Finished(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Implemented by childs to provide the core execution code.
		/// </summary>
		protected abstract void ExecuteCore();

		/// <summary>
		/// Executes the given action on the application UI thread.
		/// </summary>
		/// <param name="action">The action.</param>
		protected void ExecuteOnUIThread(Action action)
		{
			Thread.CurrentThread.ExecuteOnUIThread(action);
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

		public virtual void Dispose()
		{

		}

		#endregion
	}
}