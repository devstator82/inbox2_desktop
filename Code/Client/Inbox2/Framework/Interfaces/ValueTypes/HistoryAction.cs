using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.Interfaces.ValueTypes
{
	public class HistoryAction
	{
		public Action Do { get; private set; }

		public Action Undo { get; private set; }

		public HistoryAction(Action @do, Action undo)
		{
			Do = @do;
			Undo = undo;
		}
	}
}
