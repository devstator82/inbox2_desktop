using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;

namespace Inbox2.Plugins.StatusUpdates
{
	public class StatusUpdatesState : PluginStateBase
	{
		private static StatusUpdatesState _Current;

		public static StatusUpdatesState Current
		{
			get
			{
				if (_Current == null)
					_Current = new StatusUpdatesState();

				return _Current;
			}
		}

		public ThreadSafeCollection<UserStatus> StatusUpdates
		{
			get { return VirtualMailBox.Current.StatusUpdates; }
		}
	}
}
