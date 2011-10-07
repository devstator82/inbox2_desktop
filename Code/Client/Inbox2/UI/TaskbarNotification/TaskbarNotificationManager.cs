using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using Inbox2.Core.Configuration;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interop;
using Inbox2.Framework.Security;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.UI.Controls;

namespace Inbox2.UI.TaskbarNotification
{
    public class TaskbarNotificationManager
    {
		static readonly Icon icon = Icon.ExtractAssociatedIcon(AppDomain.CurrentDomain.BaseDirectory + "Inbox2.exe");

    	private readonly VirtualMailBox mailbox = VirtualMailBox.Current;

        /// <summary>
        /// Gets or sets the taskbar icon.
        /// </summary>
        /// <value>The taskbar icon.</value>
        public TaskbarIcon TaskbarIcon { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskbarNotificationManager"/> class.
        /// </summary>
        public TaskbarNotificationManager()
        {        	
            // Configure Taskbar icon
            TaskbarIcon = new TaskbarIcon
              	{
					Icon = icon,
              		ToolTipText = "Inbox2 desktop"
              	};
			TaskbarIcon.TrayLeftMouseUp += TaskbarIcon_TrayLeftMouseUp;

        	// Subscribe for events
            EventBroker.Subscribe(AppEvents.ReceiveFinished, () => Thread.CurrentThread.ExecuteOnUIThread(ShowNotifyBalloon));
            EventBroker.Subscribe(AppEvents.SyncFinished, () => Thread.CurrentThread.ExecuteOnUIThread(ShowNotifyBalloon));
        }

		void TaskbarIcon_TrayLeftMouseUp(object sender, RoutedEventArgs e)
		{
			var window = Application.Current.MainWindow;

			window.ShowInTaskbar = true;
			window.Show();

			if (window.WindowState == WindowState.Minimized)
				window.WindowState = WindowState.Normal;

			window.Activate();
			window.Focus();
		}

        /// <summary>
        /// Shows the notify balloon.
        /// </summary>
        private void ShowNotifyBalloon()
        {
        	var config = SettingsManager.ClientSettings.AppConfiguration;

			if (config.ShowNotificationsPopup == false || config.IsJustRegistered || config.IsLoadingAllMessages)
			{
				mailbox.Messages.ForEach(m => m.IsNew = false);
				mailbox.StatusUpdates.ForEach(s => s.IsNew = false);

				return;
			}

        	List<Message> messages;
        	List<UserStatus> statusupdates;
			List<UserStatus> searchupdates;

			using (mailbox.Messages.ReaderLock)
				messages = mailbox.Messages.Where(message => message.IsNew && message.IsRead == false && message.MessageFolder == Folders.Inbox).ToList();

			using (mailbox.StatusUpdates.ReaderLock)
				statusupdates = mailbox.StatusUpdates.Where(status => status.IsNew && status.StatusType != StatusTypes.SearchUpdate && status.IsRead == false).ToList();

			using (mailbox.StatusUpdates.ReaderLock)
				searchupdates = mailbox.StatusUpdates.Where(status => status.IsNew && status.StatusType == StatusTypes.SearchUpdate && status.IsRead == false).ToList();

            if (messages.Count() > 0 || statusupdates.Count() > 0 || searchupdates.Count() > 0)
            {
                var notifyBalloon = new NotifyBalloon();

                // Check if there is already a custom balloon active.
                if (TaskbarIcon.CustomBalloon != null)
                {
					notifyBalloon = TaskbarIcon.CustomBalloon.Child as NotifyBalloon;

					TaskbarIcon.ResetBalloonCloseTimer(5000);					
                }
                else
                {
					TaskbarIcon.ShowCustomBalloon(notifyBalloon, PopupAnimation.Fade, config.ShowNotificationsPopupFor * 1000);

					TaskbarIcon.AddBalloonClosingHandler(notifyBalloon,
						delegate
						{
							mailbox.Messages.ForEach(m => m.IsNew = false);
							mailbox.StatusUpdates.ForEach(s => s.IsNew = false);
						});										
                }

				// Play sound if enabled
				if (messages.Count > 0 && config.PlayNotificationsSound)
				{
					// XP does not support the SND_SYSTEM glaf
					var flags = VistaTools.IsVista() ?
						WinApi.SoundFlags.SND_SYSTEM | WinApi.SoundFlags.SND_NODEFAULT | WinApi.SoundFlags.SND_ALIAS | WinApi.SoundFlags.SND_ASYNC :
						WinApi.SoundFlags.SND_NODEFAULT | WinApi.SoundFlags.SND_ALIAS | WinApi.SoundFlags.SND_ASYNC;

					// First clear any previously enqueued sound
					// See: http://blogs.msdn.com/larryosterman/archive/2008/09/15/why-call-playsound-null-null-snd-nodefault.aspx
					WinApi.PlaySound(null, UIntPtr.Zero, (uint)WinApi.SoundFlags.SND_NODEFAULT);
					WinApi.PlaySound("MailBeep", UIntPtr.Zero, (uint)flags);
				}

				notifyBalloon.UpdateCounts(
					messages.OrderByDescending(m => m.SortDate),
					statusupdates.OrderByDescending(s => s.SortDate),
					searchupdates.OrderByDescending(s => s.SortDate));
			}
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public void Dispose()
        {
            TaskbarIcon.Dispose();
        }
    }
}
