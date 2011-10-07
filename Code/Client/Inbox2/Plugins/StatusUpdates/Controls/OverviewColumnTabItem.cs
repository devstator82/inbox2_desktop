using System;
using System.Linq;
using System.Threading;
using System.Windows;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.UI.Controls;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Platform.Channels;

namespace Inbox2.Plugins.StatusUpdates.Controls
{
	public class OverviewColumnTabItem : TabItem
	{
		#region Fields

		public static readonly DependencyProperty CountProperty =
			DependencyProperty.Register("Count", typeof(int), typeof(OverviewColumnTabItem), new UIPropertyMetadata(0));		

		private readonly VirtualMailBox mailbox;

		#endregion

		#region Properties

		public ChannelInstance Channel { get; private set; }
		public string Keyword { get; private set; }

		public StatusUpdatesState State
		{
			get { return PluginsManager.Current.GetState<StatusUpdatesState>(); }
		}

		public string ChannelKeyword
		{
			get { return String.Format("{0}|{1}", Channel.Configuration.DisplayName, Keyword); }
		}

		public bool IsInSearchMode
		{
			get { return !String.IsNullOrEmpty(Keyword); }
		}

		public int Count
		{
			get { return (int)GetValue(CountProperty); }
			set { SetValue(CountProperty, value); }
		}

		#endregion

		#region Construction

		public OverviewColumnTabItem()
		{
			mailbox = VirtualMailBox.Current;

			DataContext = this;

			EventBroker.Subscribe(AppEvents.SyncStatusUpdatesFinished, 
				() => ThreadPool.QueueUserWorkItem(UpdateCountAsync));
		}

		public OverviewColumnTabItem(ChannelInstance channel, string keyword) : this()
		{
			Channel = channel;
			Keyword = keyword;
		}

		#endregion

		void UpdateCountAsync(object state)
		{
			int count;

			if (Channel == null)
			{
				using (mailbox.StatusUpdates.ReaderLock)
					count = State.StatusUpdates.Count(u => u.IsRead == false);
			}
			else
			{
				using (mailbox.StatusUpdates.ReaderLock)
					count = IsInSearchMode ?
						State.StatusUpdates.Count(u => u.SearchKeyword == ChannelKeyword && u.IsRead == false) :
						State.StatusUpdates.Count(u => u.SourceChannelId == Channel.Configuration.ChannelId && u.IsRead == false);
			}

			Thread.CurrentThread.ExecuteOnUIThread(() => Count = count);
		}
    }
}
