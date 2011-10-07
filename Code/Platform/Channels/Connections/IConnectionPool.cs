using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Interfaces;

namespace Inbox2.Platform.Channels.Connections
{
	internal interface IConnectionPool<T> where T : IChannelConnection
	{
		bool AcquireLock(out T instance, string hostname, int port, bool isSecured, string username, string password);

		void ReleaseLock(T connection);

		void RegisterInstance(T connection);

		void UnregisterAllInstances(string hostname, int port, string username, string password);

		void UnregisterInstance(T connection);

		int HasConnections(string hostname, int port, string username, string password);
	}
}
