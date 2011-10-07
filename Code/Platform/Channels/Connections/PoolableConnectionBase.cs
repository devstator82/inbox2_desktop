using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Interfaces;

namespace Inbox2.Platform.Channels.Connections
{
	public abstract class PoolableConnectionBase<T> : IClientChannel, IPoolableChannel where T: ChannelConnectionBase
	{
		protected T connection;

		public virtual string Hostname { get; set; }

		public virtual int Port { get; set; }

		public virtual bool IsSecured { get; set; }
		public virtual bool IsEnabled { get; set; }

		public virtual int MaxConcurrentConnections { get; set; }

		public virtual IChannelCredentialsProvider CredentialsProvider { get; set; }

		public virtual string Protocol
		{
			get; private set;
		}

		public void OverrideMaxConcurrentConnections(int maxConcurrentConnections)
		{
			MaxConcurrentConnections = maxConcurrentConnections;
		}

		public void EnsureConnectionPool()
		{
			var credentials = CredentialsProvider.GetCredentials();

			// check if we need to create connections in the connection pool
			int connections = ConnectionPool<T>.HasConnections(Hostname, Port, credentials.Claim, credentials.Evidence);

			if (connections < MaxConcurrentConnections)
			{
				for (int i = 0; i < MaxConcurrentConnections - connections; i++)
				{
					// Can not use generic new here because new constraint lacks parameter validation
					// todo: perhaps replace with lightweight code-gen?					
					var instance = (T)Activator.CreateInstance(typeof (T), Hostname, Port, IsSecured,
					                         credentials.Claim, credentials.Evidence);

					ConnectionPool<T>.RegisterInstance(instance);
				}
			}
		}

		public void FreeAllConnections()
		{
			var credentials = CredentialsProvider.GetCredentials();

			ConnectionPool<T>.UnregisterAllInstances(Hostname, Port, credentials.Claim, credentials.Evidence);
		}

		public void FreeConnection()
		{
			if (connection == null)
				return;

			ConnectionPool<T>.UnregisterInstance(connection);
		}

		public virtual void Dispose()
		{
			connection.Dispose();
		}
	}
}
