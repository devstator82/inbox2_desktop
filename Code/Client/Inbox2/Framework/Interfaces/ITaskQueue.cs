using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.Interfaces
{
	public interface ITaskQueue
	{
		int Count { get; }

		/// <summary>
		/// Enqueues the specified work item.
		/// </summary>
		/// <param name="backgroundTask">The work item.</param>
		void Enqueue(IBackgroundTask backgroundTask);

		/// <summary>
		/// Dequeues this instance.
		/// </summary>
		/// <returns></returns>
		IBackgroundTask Dequeue();

		/// <summary>
		/// Returns the first occurance of the given task type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T Find<T>() where T : IBackgroundTask;
	}
}