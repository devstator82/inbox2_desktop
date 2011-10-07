using System;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Localization;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Interfaces;

namespace Inbox2.UI.Controls.Setup
{
	/// <summary>
	/// Interaction logic for ChannelSetupRedirectWindow.xaml
	/// </summary>
	public partial class ChannelSetupRedirectWindow : Window
	{
		private readonly ChannelConfiguration configuration;
		private readonly IWebRedirectBuilder builder;
		private readonly bool isCloudChannel;		

		public bool? Success { get; private set; }
		public Uri LastUri { get; private set; }
		public string Token { get; private set; }
		public string TokenSecret { get; private set; }

		public ChannelSetupRedirectWindow(ChannelConfiguration configuration, bool isCloudChannel)
		{
			this.configuration = configuration;
			this.isCloudChannel = isCloudChannel;
			this.builder = configuration.RedirectBuilder;			

			InitializeComponent();

			DataContext = this;
		}

		void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			browser.Navigate(builder.BuildRedirectUri());
		}
				
		void browser_Navigating(object sender, NavigatingCancelEventArgs e)
		{
			LastUri = e.Uri;
			var uri = e.Uri.ToString();

			// If the configuration has an SuccessUri configured, 
			// we check if we should close this window or not incase 
			// the browser is arrived on the success page.
			if (!string.IsNullOrEmpty(builder.SuccessUri))
			{
				if (configuration.DisplayName == "Yammer" || configuration.DisplayName == "Facebook")
				{
					if (uri.Contains(builder.SuccessUri))
					{
						Success = true;
						e.Cancel = true;
						Close();
					}
				}
				else
				{
					if (uri.StartsWith(builder.SuccessUri))
					{
						Success = true;
						e.Cancel = true;
						Close();
					}
				}
			}

			addressTextBox.Text = uri;
			loading.Visibility = Visibility.Visible;
			((Storyboard)FindResource("RunLoaderStoryboard")).Begin(); 
		}

		void browser_LoadCompleted(object sender, NavigationEventArgs e)
		{
			loading.Visibility = Visibility.Collapsed;
		}
	}
}
