using System;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using Inbox2.Framework.Localization;
using Inbox2.Framework.Interfaces.Enumerations;

namespace Inbox2.Framework
{
	public class SyncState : INotifyPropertyChanged
	{		
		public event PropertyChangedEventHandler PropertyChanged;

		private SyncStateState currentState;
		private long progress;

		public SyncStateState CurrentSyncState
		{
			get { return currentState; }
		}

		public string CurrentState
		{
			get
			{
				switch (currentState)
				{
					case SyncStateState.NoChannels:
						return String.Empty;

					case SyncStateState.NoConnection:
						return Strings.SyncNotConnected;
					
					case SyncStateState.Connecting:
						return Strings.SyncConnecting;

					case SyncStateState.Synching:
						return String.Format(Strings.SyncSyncing, progress);
					
					case SyncStateState.Completed:
						return Strings.SyncCompleted;

					case SyncStateState.AuthError:
						return Strings.SyncAuthError;

					case SyncStateState.NetError:
						return Strings.SyncNetError;
				}

				return String.Empty;
			}
		}

		public SyncState()
		{
			currentState = SyncStateState.NoChannels;

			NetworkChange.NetworkAvailabilityChanged += delegate { UpdateIdleState(); };
		}

		public void UpdateIdleState()
		{
			if (!NetworkInterface.GetIsNetworkAvailable())
			{
				currentState = SyncStateState.NoConnection;
				OnPropertyChanged("CurrentState");

				return;
			}

			if (!ChannelsManager.Channels.Any(c => c.Configuration.IsConnected))
			{
				currentState = SyncStateState.NoChannels;
				OnPropertyChanged("CurrentState");

				return;
			}

			progress = 0;

			currentState = SyncStateState.Completed;
			OnPropertyChanged("CurrentState");
		}

		public void SetSyncState(SyncStateState currentState)
		{
			this.currentState = currentState;

			OnPropertyChanged("CurrentState");
		}

		public void SetProgress(long progress)
		{
			this.progress = progress > 100 ? 100 : progress;

			OnPropertyChanged("CurrentState");
		}

		/// <summary>
		/// Called when [propertyproperty changed].
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				Thread.CurrentThread.RaiseUIPropertyChanged(this, propertyName, PropertyChanged);
			}
		}
	}
}
