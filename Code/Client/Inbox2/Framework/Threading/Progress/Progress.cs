using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using Inbox2.Framework.Collections;
using Inbox2.Framework.Extensions;
using Inbox2.Platform.Framework.Collections;

namespace Inbox2.Framework.Threading.Progress
{
	public class Progress : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private object _SyncLock = new object();

		/// <summary>
		/// Gets the list of progress groups currently active.
		/// </summary>
		public AdvancedObservableCollection<ProgressGroup> ProgressGroups { get; private set; }

		public bool HasRunning
		{
			get
			{
				lock (_SyncLock)
					return ProgressGroups.Count(g => !g.IsCompleted) > 0;
			}
		}

		public Progress()
		{
			ProgressGroups = new AdvancedObservableCollection<ProgressGroup>();
		}

		public void Add(ProgressGroup group)
		{
			group.IsRegistered = true;
			group.Completed += ProgressGroupFinished;

			lock (_SyncLock)
				ProgressGroups.Add(group);

			OnPropertyChanged("HasRunning");
		}

		void ProgressGroupFinished(object sender, EventArgs e)
		{
			ProgressGroup group = (ProgressGroup) sender;

			lock (_SyncLock)
				ProgressGroups.Remove(group);

			OnPropertyChanged("HasRunning");
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				Thread.CurrentThread.RaiseUIPropertyChanged(this, propertyName, PropertyChanged);
			}
		}
	}
}