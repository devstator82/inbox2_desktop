using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Inbox2.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Framework.Localization;

namespace Inbox2.Framework.VirtualMailBox.Entities
{
	[Serializable]
	public class LabelsContainer : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public string Labelname { get; private set; }

		private long? _count;

		public long Count
		{
			get
			{
				if (!_count.HasValue)
				{
					Refresh();

					return 0;
				}

				return _count.Value;
			}
		}

		public LabelsContainer(string labelname)
		{
			Labelname = labelname;
		}

		public void Refresh()
		{
			ThreadPool.QueueUserWorkItem(GetCountAsync);
		}

		void GetCountAsync(object state)
		{
			var mailbox = VirtualMailBox.Current;

			if (Labelname == Strings.Todo)
				using (mailbox.Messages.ReaderLock)
					_count = mailbox.Messages.Count(m => m.IsTodo);
			else if (Labelname == Strings.Someday)
				using (mailbox.Messages.ReaderLock)
					_count = mailbox.Messages.Count(m => m.IsSomeday);
			else if (Labelname == Strings.WaitingFor)
				using (mailbox.Messages.ReaderLock)
					_count = mailbox.Messages.Count(m => m.IsWaitingFor);
			else
				using (mailbox.Labels.ReaderLock)
					_count = mailbox.Labels[Labelname].Sum(l => l.Messages.Count(m => m.MessageFolder != Folders.Trash));

			Thread.CurrentThread.RaiseUIPropertyChanged(this, "Count", PropertyChanged);
		}

		public override string ToString()
		{
			return Labelname;
		}
	}
}


