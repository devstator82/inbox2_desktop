using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Inbox2.Platform.Channels.Interfaces;

namespace Inbox2.Platform.Channels.Connections
{
	public static class ConnectionPoolScavenger
	{
		private static Dictionary<IChannelConnection, DateTime> _Leases;
		private static object _SyncLock = new object();
		private static int _ExtendLeaseInSeconds = 15;
		private static Timer _ScavangerTimer;

		/// <summary>
		/// Initializes the <see cref="ConnectionPoolScavenger"/> class.
		/// </summary>
		static ConnectionPoolScavenger()
		{
			_Leases = new Dictionary<IChannelConnection, DateTime>();
			
			_ScavangerTimer = new Timer(1000);
			_ScavangerTimer.AutoReset = true;
			_ScavangerTimer.Elapsed += ScavangerTimerTick;
			_ScavangerTimer.Start();
		}

		/// <summary>
		/// Extends the lease for the given connection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance">The instance.</param>
		public static void ExtendLease<T>(T instance) where T: IChannelConnection
		{
			lock (_SyncLock)
			{
				if (_Leases.ContainsKey(instance))
				{
					_Leases[instance].AddSeconds(_ExtendLeaseInSeconds);
				}
				else
				{
					_Leases.Add(instance, DateTime.Now.AddSeconds(15));
				}
			}
		}

		/// <summary>
		/// Shutdowns this instance and clears all the open standing leases.
		/// </summary>
		public static void Shutdown()
		{
			lock (_SyncLock)
			{
				foreach (var lease in _Leases.Keys)
				{					
					lease.Close();
					lease.Dispose();					
				}

				_Leases.Clear();
			}
		}

		/// <summary>
		/// Cleans up any unused connections.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
		static void ScavangerTimerTick(object sender, ElapsedEventArgs e)
		{
			lock (_SyncLock)
			{
				var outdated = 
					_Leases.Where(kv => DateTime.Now > kv.Value 
					                    && kv.Key.IsConnected == false && kv.Key.IsLocked == false)
						.Select(kv => kv.Key)
						.ToList();

				foreach (var connection in outdated)
					connection.Close();

				foreach (var connection in outdated)
					_Leases.Remove(connection);
			}
		}
	}
}