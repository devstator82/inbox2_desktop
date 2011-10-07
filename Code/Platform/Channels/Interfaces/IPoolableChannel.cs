using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Channels.Interfaces
{
	public interface IPoolableChannel
	{
		void OverrideMaxConcurrentConnections(int maxConcurrentConnections);

		void EnsureConnectionPool();

		void FreeAllConnections();

		void FreeConnection();
	}
}
