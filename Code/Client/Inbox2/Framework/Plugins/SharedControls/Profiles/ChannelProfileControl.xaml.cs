using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Stats;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Extensions;

namespace Inbox2.Framework.Plugins.SharedControls.Profiles
{
	/// <summary>
	/// Interaction logic for ChannelProfileControl.xaml
	/// </summary>
	public partial class ChannelProfileControl : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public Person Person { get; private set; }

		public Profile Profile { get; private set; }

		public ChannelConfiguration Channel { get; private set; }

	    public Color BackgroundColor
	    {
            get { return (Color)ColorConverter.ConvertFromString(ChannelsManager.GetChannelColor(Channel)); }
	    }

		public UserStatus LastStatus { get; private set; }		

		public ChannelProfileControl(Person person, Profile profile, ChannelConfiguration channel)
		{
			InitializeComponent();

			DataContext = this;

			Person = person;
			Profile = profile;
			Channel = channel;

			if (Channel.Charasteristics.SupportsStatusUpdates)
			{
				// Execute task to get latest status update for this user
				var task = new GetLastUserStatusTask(Channel, 
					ChannelsManager.GetChannelObject(Channel.ChannelId).StatusUpdatesChannel, Profile.SourceAddress);

				task.FinishedSuccess += GetLastUserStatusTask_Finished;
				task.FinishedFailure += GetLastUserStatusTask_FinishedFailure;
				task.ExecuteAsync();

				// Start loader animation
				((Storyboard)FindResource("RunLoaderStoryboard")).Begin();
			}
		}
	
		void GetLastUserStatusTask_Finished(object sender, EventArgs e)
		{
			var status = ((GetLastUserStatusTask)sender).Status;

			Thread.CurrentThread.ExecuteOnUIThread(delegate
			{
				loader.Visibility = Visibility.Collapsed;							

				if (status == null)
					empty.Visibility = Visibility.Visible;
				else
				{
					LastStatus = status.DuckCopy<UserStatus>();

					OnPropertyChanged("LastStatus");
				}				
			});			
		}

		void GetLastUserStatusTask_FinishedFailure(object sender, EventArgs e)
		{
			Thread.CurrentThread.ExecuteOnUIThread(delegate
               	{
					loader.Visibility = Visibility.Collapsed;
					empty.Visibility = Visibility.Visible;
               	});
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		void View_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		void View_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEventWithSegment("View profile on channel from user profile", Channel.DisplayName);

			var url = Channel.ProfileInfoBuilder.BuildServiceProfileUrl(Profile.DuckCopy<ChannelProfile>());

			new Process { StartInfo = new ProcessStartInfo(url) }.Start();
		}

		void Reply_Click(object sender, RoutedEventArgs e)
		{
			ClientStats.LogEvent("Compose status update from user profile");

			EventBroker.Publish(AppEvents.RequestStatusUpdate, String.Format("@{0} ", Profile.ScreenName), Channel.ChannelId, LastStatus);
		}

		void ViewAllButton_Click(object sender, RoutedEventArgs e)
		{
			ClientStats.LogEvent("View everything from user profile");

			EventBroker.Publish(AppEvents.View, Profile.Person);
		}		
	}
}


