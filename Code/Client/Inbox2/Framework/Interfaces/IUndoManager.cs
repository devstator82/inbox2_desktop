using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework.Interfaces.ValueTypes;

namespace Inbox2.Framework.Interfaces
{
	public interface IUndoManager
	{
		void Execute(HistoryAction action);

		void Undo();

		void Redo();
	}
}
