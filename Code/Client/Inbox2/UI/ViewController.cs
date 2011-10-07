using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Inbox2.Core.Configuration;
using Inbox2.Core.Threading;
using Inbox2.Core.Threading.Repeat;
using Inbox2.Core.Threading.Tasks;
using Inbox2.Framework;
using Inbox2.Framework.Deployment;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Framework.Interop.Windows;
using Inbox2.Framework.Localization;
using Inbox2.Framework.Threading;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Platform.Framework;
using Inbox2.Platform.Logging;
using Inbox2.UI.Controls;
using Inbox2.UI.Controls.Setup;
using Inbox2.UI.Extensions;
using Inbox2.UI.TaskbarNotification;
using Inbox2.UI.Windows;
using MessageBox = Inbox2.UI.Windows.MessageBox;

namespace Inbox2.UI
{
	[Export(typeof(IViewController))]
	public class ViewController : IViewController
	{
		private Grid root;
		private Stack<PopupWindow> popupChildren;
		private MultiViewFrameControl container;

		/// <summary>
		/// Gets or sets the task bar notify manager.
		/// </summary>
		/// <value>The task bar notify manager.</value>
		public TaskbarNotificationManager TaskBarNotifyManager { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public ViewController()
		{
			popupChildren = new Stack<PopupWindow>();
		}

		/// <summary>
		/// Initializes the controller with the specified canvas.
		/// </summary>
		/// <param name="canvas">The canvas.</param>
		public void Initialize(Grid root)
		{
			this.root = root;
			
			// Create taskbar notifier
			TaskBarNotifyManager = new TaskbarNotificationManager();
			Inbox2MessageBox.ShowMessageBox += MessageBox_ShowMessageBox;

			this.root.Children.Add(TaskBarNotifyManager.TaskbarIcon);
		}

		/// <summary>
		/// Initializes the main application view, called during application startup.
		/// </summary>
		public void Startup()
		{
			if (!SettingsManager.ClientSettings.AppConfiguration.IsChannelSetupFinished)
			{
				var control = new ChannelSetupPageControl { Opacity = 0 };
				root.Children.Add(control);

				control.FadeIn();

				return;	
			}

			if (container == null)
			{
				ThreadPool.QueueUserWorkItem(LoadMailBox);

				using (new CodeTimer("ViewController/CompleteStartup"))
				{
					container = new MultiViewFrameControl();
					root.Children.Add(container);					

					// Remove startup controls
					root.Children.RemoveElementOfType<ChannelSetupPageControl>();					
				}

				ShowPopup<ApplicationLoadingControl>();
			}
		}

		public bool CanShutdown()
		{
			if (container == null)
				return true;

			return container.CanShutdown();
		}

		void LoadMailBox(object state)
		{
			using (new CodeTimer("Startup/State"))
			{
				VirtualMailBox.Current.InboxLoadComplete += MailBox_InboxLoadComplete;
				VirtualMailBox.Current.Load();

				PluginsManager.Current.Plugins.ForEach(p => p.Initialize());

				var queue = (TaskQueue)ClientState.Current.TaskQueue;

				new Repeat("ProcessQueue").Every(3).Seconds().Call(queue.ProcessingPool.Process);
				new Repeat("Cleanup").Every(5).Minutes().Call(Tasks.Cleanup);
				new Run("PurgeData").After(30).Seconds().Call(Tasks.PurgeData);

				if (!CommandLine.Current.DisableStartupSendReceive)
				{
					Tasks.Receive();
					Tasks.Send();
					Tasks.Sync();
				}

				new Run("UpdateCheck").After(5).Seconds().Call(Tasks.CheckForUpdate);
				new Run("ShipLogs").After(15).Seconds().Call(Tasks.ShipLogs);
			}

			// Forces update of status updates counts
			EventBroker.Publish(AppEvents.SyncStatusUpdatesFinished);

			ClientState.StartupSuccess = true;
		}

		void MailBox_InboxLoadComplete(object sender, EventArgs e)
		{
			HidePopup();

			container.LoadMultiViewState();

			CheckCommandLine();
			CheckDefaultMailClientState();

			foreach (var upgrade in UpgradeActionBase.Upgrades)
				new BackgroundActionTask(upgrade.AfterLoadUpgradeAsync).ExecuteAsync();
		}    	

		/// <summary>
		/// Shutdowns this instance.
		/// </summary>
		public void Shutdown()
		{
			if (container != null)
			{
				container.PersistChildren();
			}

			// Clean up taskbar notifier (would otherwise stay open until application finishes)
			TaskBarNotifyManager.Dispose();
		}

		public void ShowPopup<T>() where T : Control, new()
		{
			ShowPopup(new T());
		}

		public void ShowPopup(Control control)
		{
			var window = new PopupWindow(control);
			popupChildren.Push(window);

			window.Owner = Application.Current.MainWindow;
			window.ShowDialog();
		}

		public void HidePopup()
		{
			if (popupChildren.Count > 0)
			{
				var window = popupChildren.Pop();
				window.ForceClose();

				EventBroker.Publish(AppEvents.RequestFocus);
			}
		}

		/// <summary>
		/// Moves to the given overview instance.
		/// </summary>
		/// <param name="view"></param>
		public void MoveTo(WellKnownView view)
		{
			container.ShowTabFor(view);
		}

		/// <summary>
		/// Moves to the given details instance.
		/// </summary>
		/// <param name="plugin">The plugin.</param>
		/// <param name="dataInstance">The data instance.</param>
		public void MoveTo(IDetailsViewPlugin plugin, object dataInstance)
		{
			var tabItem = container.ContainsTabFor(dataInstance);

			if (plugin.ForceSingle && tabItem != null)
			{
				// Show tab
				container.TabHost.SelectedItem = tabItem;

				return;
			}

			container.CreateTabFor(plugin.Header, plugin.Icon, true, plugin, plugin.CreateView(), dataInstance, WellKnownView.DetailsView, true);
		}

		/// <summary>
		/// Moves to.
		/// </summary>
		/// <param name="plugin">The plugin.</param>
		/// <param name="dataInstance">The data instance.</param>
		public void MoveTo(INewItemViewPlugin plugin, object dataInstance)
		{
			var tabItem = container.ContainsTabFor(dataInstance);

			if (plugin.ForceSingle && tabItem != null)
			{
				// Show tab
				container.TabHost.SelectedItem = tabItem;

				return;
			}

			container.CreateTabFor(plugin.Header, plugin.Icon, true, plugin, plugin.CreateView(), dataInstance, WellKnownView.NewItemsView, true);
		}

		void CheckCommandLine()
		{
			if (!String.IsNullOrEmpty(CommandLine.Current.Mailto))
				EventBroker.Publish(AppEvents.New, CommandLine.Current.Mailto);
		}

		void CheckDefaultMailClientState()
		{
			var settings = SettingsManager.ClientSettings.AppConfiguration;

			if (!settings.IsJustRegistered && settings.IsDefaultMailClientCheckEnabled)
			{
				// Check if Inbox2 is the default mail client
				var handler = new DefaultMailClientHandler();

				if (!handler.IsDefaultMailClient())
				{
					var result = Inbox2MessageBox.Show(Strings.NotYourDefaultMailClient, Inbox2MessageBoxButton.YesNo,
													   Strings.AlwaysPerformThisCheckDuringStartup, true);

					settings.IsDefaultMailClientCheckEnabled = result.DoNotAskAgainResult;
					SettingsManager.Save();

					if (result.Result == Inbox2MessageBoxResult.Yes)
					{
						var executable = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Inbox2DefaultMailClient.exe");

						try
						{
							// Start set default mail client process as administrator if needed
							new Process { StartInfo = new ProcessStartInfo(executable) { UseShellExecute = true, Verb = "runas" } }.Start();
						}
						catch (Exception ex)
						{
							Logger.Error("An error has occured while trying to start process. Exception = {0}", LogSource.UI, ex);
						}
					}
				}
			}
		}

		void MessageBox_ShowMessageBox(object sender, MessageBoxEventArgs e)
		{
			var window = new MessageBox(e) { Owner = Application.Current.MainWindow };

			window.ShowDialog();
		}
	}
}
