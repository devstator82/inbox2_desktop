using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Extensions;
using Inbox2.Core.Configuration;
using Inbox2.Framework;
using Inbox2.Framework.Localization;
using Inbox2.UI;
using Window = System.Windows.Window;

namespace Inbox2
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public MainWindow()
		{
			InitializeComponent();
		}

		public bool IsCompositionEnabled
		{
			get { return WindowChrome._IsCompositionEnabled; }
		}

		void Window_Initialized(object sender, EventArgs e)
		{
			ApplicationHostControl.Initialize(this);

			WindowChrome.DwmCompositionChanged += delegate { OnPropertyChanged("IsCompositionEnabled"); };			
		}

		void Window_Closing(object sender, CancelEventArgs e)
		{
			var controller = (ViewController) ClientState.Current.ViewController;

			if (!controller.CanShutdown())
			{
				if (Inbox2MessageBox.Show(Strings.AreYouSureYouWantToExit, Inbox2MessageBoxButton.YesNo).Result == Inbox2MessageBoxResult.No)
				{
					e.Cancel = true;

					return;
				}
			}

			ApplicationHostControl.Shutdown();

			Application.Current.Shutdown();
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		void MainWindow_OnStateChanged(object sender, EventArgs e)
		{
			if (SettingsManager.ClientSettings.AppConfiguration.MinimizeToTray && WindowState == WindowState.Minimized)
			{
				ShowInTaskbar = false;
				Hide();
			}
		}
	}
}