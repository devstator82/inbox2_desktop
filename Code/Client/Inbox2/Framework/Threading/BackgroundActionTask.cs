using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.Threading
{
	public class BackgroundActionTask : BackgroundTask
	{
		protected Action action;

		public BackgroundActionTask(Action action)
		{
			this.action = action;
		}

		protected override void ExecuteCore()
		{
			action.Invoke();
		}
	}
}