using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Core.Threading.Repeat
{
	public abstract class ScheduledItem
	{
		protected Action action;

		public DateTime DateScheduled { get; protected set; }

		public string Key { get; private set; }

		protected ScheduledItem(string key)
		{
			Key = key;
		}

		public void Call(Action theAction)
		{
			action = theAction;

			ScheduledTaskRunner.Enqueue(this);
		}

		public virtual void OnBeforeExecute()
		{			
		}

		public void Execute()
		{
			OnBeforeExecute();

			try
			{
				action();
			}
			catch
			{
				// Swallow exceptions
			}

			ScheduledTaskRunner.Dequeue(this);

			OnAfterExecute();
		}

		public virtual void OnAfterExecute()
		{
		}
	}
}
