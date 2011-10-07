using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Inbox2.Framework.Extensions;
using Inbox2.Platform.Framework.Collections;

namespace Inbox2.Framework.Threading.Progress
{
	public class ProgressManager : INotifyPropertyChanged
	{
		#region Singleton pattern implementation

		private static ProgressManager _Current;

		public static ProgressManager Current
		{
			get
			{
				if (_Current == null)
					_Current = new ProgressManager();

				return _Current;
			}
		}

		private ProgressManager()
		{
			ProgressGroups = new AdvancedObservableCollection<ProgressGroup>();
		}

		#endregion

		private readonly object _SyncLock = new object();

		public event PropertyChangedEventHandler PropertyChanged;
		
		public AdvancedObservableCollection<ProgressGroup> ProgressGroups { get; private set; }

		public bool HasRunning
		{
			get
			{
				lock (_SyncLock)
					return ProgressGroups.Count(g => !g.IsCompleted) > 0;
			}
		}

		public void Register(ProgressGroup group)
		{
			bool contains;

			lock (_SyncLock)
				contains = ProgressGroups.Contains(group);

			if (!contains)
			{
				group.IsRegistered = true;
				group.Completed += ProgressGroupFinished;

				lock (_SyncLock)
					ProgressGroups.Add(group);

				OnPropertyChanged("HasRunning");
			}
		}

		void ProgressGroupFinished(object sender, EventArgs e)
		{
			var group = (ProgressGroup)sender;

			lock (_SyncLock)
				ProgressGroups.Remove(group);

			OnPropertyChanged("HasRunning");
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				Thread.CurrentThread.RaiseUIPropertyChanged(this, propertyName, PropertyChanged);
		}
	}
}