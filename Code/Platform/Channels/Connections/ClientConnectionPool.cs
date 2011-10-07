using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Inbox2.Platform.Channels.Interfaces;

namespace Inbox2.Platform.Channels.Connections
{
	public class ClientConnectionPool<T> : IConnectionPool<T> where T : IChannelConnection
	{
		private readonly object _syncRoot = new object();
		private readonly Dictionary<T, bool> locks;

		public ClientConnectionPool()
		{
			locks = new Dictionary<T, bool>();
		}

		public bool AcquireLock(out T instance, string hostname, int port, bool isSecured, string username, string password)
		{
			lock (_syncRoot)
			{
				var free = locks.Count(c => c.Value == false && c.Key.Hostname == hostname && c.Key.Port == port && c.Key.Username == username && c.Key.Password == password) > 0;

				if (free)
				{
					var connection = locks.First(c => c.Value == false && c.Key.Hostname == hostname && c.Key.Port == port && c.Key.Username == username && c.Key.Password == password).Key;

					Trace.WriteLine("*** Locking " + connection.UniqueId);

					locks[connection] = true;

					connection.IsLocked = true;

					instance = connection;

					return true;
				}
			}

			Trace.WriteLine("*** Lock miss");

			instance = default(T);

			return false;
		}

		public void ReleaseLock(T connection)
		{
			lock (_syncRoot)
			{
				Trace.WriteLine("*** Unlocking " + connection.UniqueId);

				connection.IsLocked = false;

				ConnectionPoolScavenger.ExtendLease(connection);

				locks[connection] = false;
			}
		}

		public void RegisterInstance(T connection)
		{
			lock (_syncRoot)
			{
				locks.Add(connection, false);
			}
		}

		public void UnregisterAllInstances(string hostname, int port, string username, string password)
		{
			lock (_syncRoot)
			{
				var connectionsToRemove = new List<T>();

				foreach (var c in locks.Keys)
				{
					if (c.Hostname == hostname && c.Port == port && c.Username == username && c.Password == password)
						connectionsToRemove.Add(c);
				}

				foreach (var c in connectionsToRemove)
				{
					c.Close();
					locks.Remove(c);
				}
			}
		}

		public void UnregisterInstance(T connection)
		{
			lock (_syncRoot)
			{
				connection.Close();
				locks.Remove(connection);
			}
		}

		public int HasConnections(string hostname, int port, string username, string password)
		{
			lock (_syncRoot)
			{
				return locks.Count(c => c.Key.Hostname == hostname
					&& c.Key.Port == port && c.Key.Username == username && c.Key.Password == password);
			}
		}
	}
}
