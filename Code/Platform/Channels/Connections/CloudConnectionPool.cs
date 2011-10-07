using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Interfaces;

namespace Inbox2.Platform.Channels.Connections
{
	public class CloudConnectionPool<T> : IConnectionPool<T> where T : IChannelConnection
	{
		public bool AcquireLock(out T instance, string hostname, int port, bool isSecured, string username, string password)
		{
			instance = (T)Activator.CreateInstance(typeof(T), hostname, port, isSecured, username, password);

			return true;
		}

		public void ReleaseLock(T connection)
		{			
		}

		public void RegisterInstance(T connection)
		{			
		}

		public void UnregisterAllInstances(string hostname, int port, string username, string password)
		{			
		}

		public void UnregisterInstance(T connection)
		{			
		}

		public int HasConnections(string hostname, int port, string username, string password)
		{
			// Pretend like we always have enough instances
			return Int32.MaxValue;
		}
	}
}
