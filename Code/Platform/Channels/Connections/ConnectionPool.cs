using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Interfaces;

namespace Inbox2.Platform.Channels.Connections
{
	public static class ConnectionPool<T> where T : class, IChannelConnection
	{
		private static readonly IConnectionPool<T> inner;

		static ConnectionPool()
		{
			var setting = ConfigurationManager.AppSettings["/Settings/Channels/Codebase"];
			bool cloud = false;
			bool push = false;

			if (setting != null && setting.ToLower() == "cloud")
				cloud = true;

			if (setting != null && setting.ToLower() == "push")
				push = true;

			if (cloud)
				inner = new CloudConnectionPool<T>();
			else if (push)
				inner = new PushConnectionPool<T>();
			else
				inner = new ClientConnectionPool<T>();
		}

		public static bool AcquireLock(out T instance, string hostname, int port, bool isSecured, string username, string password)
		{
			return inner.AcquireLock(out instance, hostname, port, isSecured, username, password);
		}

		public static void ReleaseLock(T connection)
		{
			inner.ReleaseLock(connection);
		}

		public static void RegisterInstance(T connection)
		{
			inner.RegisterInstance(connection);
		}

		public static void UnregisterAllInstances(string hostname, int port, string username, string password)
		{
			inner.UnregisterAllInstances(hostname, port, username, password);
		}

		public static void UnregisterInstance(T connection)
		{
			inner.UnregisterInstance(connection);
		}

		public static int HasConnections(string hostname, int port, string username, string password)
		{
			return inner.HasConnections(hostname, port, username, password);
		}		
	}
}