using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Interfaces;

namespace Inbox2.Platform.Channels.Connections
{
	public class PushConnectionPool<T> : IConnectionPool<T> where T : class, IChannelConnection
	{
		private readonly object _syncRoot = new object();
		private readonly Dictionary<long, List<T>> connections;

		public PushConnectionPool()
		{
			connections = new Dictionary<long, List<T>>();
		}

		public bool AcquireLock(out T instance, string hostname, int port, bool isSecured, string username, string password)
		{
			List<T> list;

			lock (_syncRoot)
			{
				if (connections.ContainsKey(ChannelConnectionBase.UserId))
				{
					list = connections[ChannelConnectionBase.UserId];

					var connection = list.FirstOrDefault(c => c.Hostname == hostname && c.Port == port && c.Username == username && c.Password == password);

					instance = connection;

					if (instance != null)
						return true;
				}
			}

			instance = (T)Activator.CreateInstance(typeof(T), hostname, port, isSecured, username, password);

			RegisterInstance(instance);

			return true;
		}

		public void ReleaseLock(T connection)
		{
			ConnectionPoolScavenger.ExtendLease(connection);
		}

		public void RegisterInstance(T connection)
		{
			lock (_syncRoot)
			{
				if (!connections.ContainsKey(ChannelConnectionBase.UserId))
					connections.Add(ChannelConnectionBase.UserId, new List<T>());

				connections[ChannelConnectionBase.UserId].Add(connection);
			}
		}

		public void UnregisterAllInstances(string hostname, int port, string username, string password)
		{
			lock (_syncRoot)
			{
				if (connections.ContainsKey(ChannelConnectionBase.UserId))
				{
					var connectionsToRemove = connections[ChannelConnectionBase.UserId]
						.Where(c => c.Hostname == hostname 
							&& c.Port == port 
							&& c.Username == username 
							&& c.Password == password)
						.ToList();

					foreach (var c in connectionsToRemove)
					{
						c.Close();
						connections[ChannelConnectionBase.UserId].Remove(c);
					}
				}
			}
		}

		public void UnregisterInstance(T connection)
		{
			connection.Close();

			lock (_syncRoot)
			{
				if (connections.ContainsKey(ChannelConnectionBase.UserId))
					connections[ChannelConnectionBase.UserId].Remove(connection);
			}
		}

		public int HasConnections(string hostname, int port, string username, string password)
		{
			// Pretend like we always have enough instances
			return Int32.MaxValue;
		}
	}
}
