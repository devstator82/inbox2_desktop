using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.Interfaces.Plugins
{
	public interface IStatePlugin
	{
		event EventHandler<EventArgs> SelectionChanged;

		bool CanView { get; }
		bool CanNew { get; }
		bool CanReply { get; }
		bool CanReplyAll { get; }
		bool CanForward { get; }
		bool CanDelete { get; }
		bool CanMarkRead { get; }
		bool CanMarkUnread { get; }

		void View();
		void New();
		void Reply();
		void ReplyAll();
		void Forward();
		void Delete();
		void MarkRead();
		void MarkUnread();
		void Shutdown();
	}
}
