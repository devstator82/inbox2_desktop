using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Inbox2.Core.Configuration;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Localization;
using Inbox2.Framework.Stats;
using Inbox2.Platform.Framework;
using Inbox2.Plugins.Conversations.Controls;

namespace Inbox2.UI.Controls.Views
{
	/// <summary>
	/// Interaction logic for ActivityStreamControl.xaml
	/// </summary>
	public partial class ActivityStreamControl : UserControl
	{	
	    #region Construction

	    public ActivityStreamControl()
	    {
	        using (new CodeTimer("ActivityStreamControl/Constructor"))
	            InitializeComponent();
	    }

	    #endregion

	    #region Methods

	    protected override void OnInitialized(EventArgs e)
	    {
	        base.OnInitialized(e);
			
	        LoadMainOverview();
	        ShowMessagesLoader();
	        ShowBanner();
	        ShowSurvey();
	    }

	    void LoadMainOverview()
	    {
			using (new CodeTimer("ActivityStreamControl/LoadMainOverview"))
				StreamViewsContainerRootGrid.Children.Add(new OverviewControl());
	    }

	    void ShowMessagesLoader()
	    {
	        if (SettingsManager.ClientSettings.AppConfiguration.IsJustRegistered)
	        {
	            EventBroker.Subscribe(AppEvents.ReceiveFinished, () => Thread.CurrentThread.ExecuteOnUIThread(ReceiveFinished));
	        }

	        // Restart loading all messages
	        if (SettingsManager.ClientSettings.AppConfiguration.IsLoadingAllMessages)
	        {
				EventBroker.Publish(AppEvents.RequestReceive, Int32.MaxValue);	            
	            EventBroker.Subscribe(AppEvents.ReceiveFinished, () => Thread.CurrentThread.ExecuteOnUIThread(ReceiveFinished));

	            LoadingAllMesagesBorder.Visibility = Visibility.Visible;
	        }
	    }

	    void ShowBanner()
	    {
	        if (SettingsManager.ClientSettings.AppConfiguration.ShowBanner)
	            StreamViewBanner.Visibility = Visibility.Visible;
	    }		

	    void ShowSurvey()
	    {
	        if (!SettingsManager.ClientSettings.AppConfiguration.DateSurveyDone.HasValue)
	        {
	            var dateInstalled = new DirectoryInfo(DebugKeys.DefaultDataDirectory).CreationTime;

	            if ((DateTime.Now - dateInstalled).TotalDays > 7)
	            {                    
	                if (SettingsManager.ClientSettings.AppConfiguration.DateSurveySnoozed.HasValue)
	                {
	                    // User snoozed survey, if that was less then 7 days ago do not show survey
	                    if ((DateTime.Now - SettingsManager.ClientSettings.AppConfiguration.DateSurveySnoozed.Value).TotalDays < 7)
	                        return;
	                }

	                TakeTheSurveyBorder.Visibility = Visibility.Visible;
	            }
	        }
	    }

	    void ReceiveFinished()
	    {
	        if (SettingsManager.ClientSettings.AppConfiguration.IsJustRegistered 
				&& ChannelsManager.Channels.Any(c => !c.Configuration.IsConnected))
	        {
	            LoadAllMesagesBorder.Visibility = Visibility.Visible;
	        }

	        if (SettingsManager.ClientSettings.AppConfiguration.IsLoadingAllMessages)
	        {
	            SettingsManager.ClientSettings.AppConfiguration.IsLoadingAllMessages = false;
	            SettingsManager.Save();

	            LoadingAllMesagesBorder.Visibility = Visibility.Collapsed;	
	        }			
	    }

        void RunReceive()
        {
            EventBroker.Publish(AppEvents.RequestReceive, Int32.MaxValue);

            EventBroker.Subscribe(AppEvents.ReceiveFinished, () => Thread.CurrentThread.ExecuteOnUIThread(delegate
            {
                if (SettingsManager.ClientSettings.AppConfiguration.IsLoadingAllMessages)
                {
                    SettingsManager.ClientSettings.AppConfiguration.IsLoadingAllMessages = false;
                    SettingsManager.Save();
                }
            }));
        }

	    #endregion

	    #region Event handlers

	    /// <summary>
	    /// User clicks on the load all messages button.
	    /// </summary>
	    /// <param name="sender"></param>
	    /// <param name="e"></param>
	    void LoadAllMessages_Click(object sender, RoutedEventArgs e)
	    {
			ClientStats.LogEvent("Load all messages in overview");

	        if (Inbox2MessageBox.Show(Strings.ThisCouldTakeALongTime, Inbox2MessageBoxButton.YesNo).Result == Inbox2MessageBoxResult.Yes)
	        {
				// todo mwa
				//if (((TaskQueue)ClientState.Current.TaskQueue).ProcessingPool.HasRunning<ReceiveTask>())
				//{
				//    // Let current send/receive finish
				//    IEventReg subscription = null;

				//    subscription = EventBroker.Subscribe(AppEvents.ReceiveFinished, delegate
				//        {
				//            EventBroker.Unregister(subscription);

				//            RunReceive();
				//        });
				//}	           

				RunReceive();

				EventBroker.Publish(AppEvents.RequestReceive, Int32.MaxValue);
	            EventBroker.Subscribe(AppEvents.ReceiveFinished, () => Thread.CurrentThread.ExecuteOnUIThread(ReceiveFinished));

	            SettingsManager.ClientSettings.AppConfiguration.IsJustRegistered = false;
	            SettingsManager.ClientSettings.AppConfiguration.IsLoadingAllMessages = true;
	            SettingsManager.Save();

	            LoadAllMesagesBorder.Visibility = Visibility.Collapsed;
	            LoadingAllMesagesBorder.Visibility = Visibility.Visible;				
	        }			
	    }

	    /// <summary>
	    /// User clicks on the close button and does not want to load all messages.
	    /// </summary>
	    /// <param name="sender"></param>
	    /// <param name="e"></param>
	    void CloseLoadAllMessages_Click(object sender, RoutedEventArgs e)
	    {
			ClientStats.LogEvent("Close load all messages in overview");

	        SettingsManager.ClientSettings.AppConfiguration.IsJustRegistered = false;
	        SettingsManager.Save();

	        LoadAllMesagesBorder.Visibility = Visibility.Collapsed;
	    }

	    void StopLoadingAllMessages_Click(object sender, RoutedEventArgs e)
	    {
			ClientStats.LogEvent("Stop loading all messages in overview");

	        // Cancel any running receive tasks
			// todo mwa
			//((TaskQueue)ClientState.Current.TaskQueue).ProcessingPool.Kill<ReceiveMessagesTask>();
			//((TaskQueue)ClientState.Current.TaskQueue).ProcessingPool.Kill<ReceiveFoldersTask>();
			//((TaskQueue)ClientState.Current.TaskQueue).ProcessingPool.Kill<ReceiveTask>();			

	        SettingsManager.ClientSettings.AppConfiguration.IsLoadingAllMessages = false;
	        SettingsManager.Save();

	        LoadingAllMesagesBorder.Visibility = Visibility.Collapsed;
	    }		

	    void LearnMore_Click(object sender, RoutedEventArgs e)
	    {
			ClientStats.LogEvent("Click on lifestyle sync banner in overview");

	        new Process { StartInfo = new ProcessStartInfo("http://www.inbox2.com/lifestylesync") }.Start();
	    }

	    void CloseBanner_Click(object sender, RoutedEventArgs e)
	    {
			ClientStats.LogEvent("Close on lifestyle sync banner in overview");

	        StreamViewBanner.Visibility = Visibility.Collapsed;
			
	        SettingsManager.ClientSettings.AppConfiguration.ShowBanner = false;
	        SettingsManager.Save();
	    }

	    void TakeSurvey_Click(object sender, RoutedEventArgs e)
	    {
			ClientStats.LogEvent("Click on take survey in overview");

	        SettingsManager.ClientSettings.AppConfiguration.SurveysDone++;
            SettingsManager.ClientSettings.AppConfiguration.DateSurveyDone = DateTime.Now;
	        SettingsManager.ClientSettings.AppConfiguration.DateSurveySnoozed = null;
            SettingsManager.Save();

	        new Process { StartInfo = new ProcessStartInfo("http://www.inbox2.com/survey/1") }.Start();

            TakeTheSurveyBorder.Visibility = Visibility.Collapsed;
	    }

	    void MaybeLater_Click(object sender, RoutedEventArgs e)
	    {
	        SettingsManager.ClientSettings.AppConfiguration.DateSurveySnoozed = DateTime.Now;
	        SettingsManager.Save();

            TakeTheSurveyBorder.Visibility = Visibility.Collapsed;
	    }

	    #endregion
	}
}