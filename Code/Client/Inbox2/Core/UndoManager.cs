using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Interfaces.ValueTypes;
using Inbox2.Framework.Stats;

namespace Inbox2.Core
{
	[Export(typeof(IUndoManager))]
	public class UndoManager : IUndoManager
	{
		protected Stack<HistoryAction> historyActions;
		protected Stack<HistoryAction> futureActions;

		public UndoManager()
		{
			historyActions = new Stack<HistoryAction>();
			futureActions = new Stack<HistoryAction>();
		}

		public void Execute(HistoryAction action)
		{		
			action.Do();

			futureActions.Clear();
			historyActions.Push(action);
		}

		public void Undo()
		{
			if (historyActions.Count == 0)
				return;

			var action = historyActions.Pop();

			action.Undo();

			futureActions.Push(action);

			ClientStats.LogEvent("UndoManager/Undo");
		}

		public void Redo()
		{
			if (futureActions.Count == 0)
				return;

			var action = futureActions.Pop();

			action.Do();

			historyActions.Push(action);

			ClientStats.LogEvent("UndoManager/Redo");
		}
	}
}
