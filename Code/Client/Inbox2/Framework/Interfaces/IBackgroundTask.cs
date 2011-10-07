using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Framework.Interfaces
{
	public interface IBackgroundTask
	{
		/// <summary>
		/// Gets or sets the execution status.
		/// </summary>
		/// <value>The execution status.</value>
		ExecutionStatus ExecutionStatus { get; set; }

		/// <summary>
		/// Gets the max living instances allowed in the queue.
		/// </summary>
		/// <value>The max living instances.</value>
		int MaxQueuedInstances { get; }

		/// <summary>
		/// Gets the singleton key.
		/// </summary>
		/// <value>The singleton key.</value>
		string SingletonKey { get; }

		/// <summary>
		/// Gets or sets the minimum datetime to execute this task.
		/// </summary>
		/// <value>The execute after.</value>
		DateTime ExecuteAfter { get; set; }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		string Name { get; }
	}
}
