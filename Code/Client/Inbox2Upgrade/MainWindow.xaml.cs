using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;

namespace Inbox2Upgrade
{
    /// <summary>
    /// Interaction logic for Updater.xaml
    /// </summary>
	public partial class MainWindow : Window
    {
    	readonly string path;
		private string filename;
    	private DispatcherTimer timer;
    	private bool isUpgrade;

    	static readonly string[] skipUpgrade = new[] { "Inbox2Upgrade.exe", "ICSharpCode.SharpZipLib.dll" };
    	
    	public MainWindow()
        {
        	path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        	InitializeComponent();
        }

    	void Updater_OnLoaded(object sender, RoutedEventArgs e)
    	{
    		try
    		{
				// Check if we need to upgrade
    			RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Inbox2\\Upgrade");

    			if (key != null)
    			{
    				filename = key.GetValue("filename") as string;

    				if (!String.IsNullOrEmpty(filename))
    				{
    					isUpgrade = true;

    					if (!EnsureSetupNotRunning())
    					{
    						Application.Current.Shutdown();
    						return;
    					}

    					if (!EnsureClientNotRunning())
    					{
							Application.Current.Shutdown();
							return;
    					}

    					Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => ThreadPool.QueueUserWorkItem(Extract)));

    					return;
    				}
    			}

    			ProgressTextBlock.Text = "Installation completed...";

    			timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, delegate { Finished(); }, Dispatcher);
    			timer.Start();
    		}
    		catch (Exception ex)
    		{
    			MessageBox.Show("An exception has occured while trying to upgrade Inbox2. Exception = " + ex, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

				Application.Current.Shutdown();
    		}
    	}

		protected bool EnsureClientNotRunning()
		{
			var processes = Process.GetProcessesByName("inbox2");

			if (processes.Length > 0)
			{
				// Client is running
				if (MessageBox.Show("The Inbox2 desktop client is currently running, upgrade can not continue. Click ok to close the running instances or cancel to abort the upgrade.", "Question", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
				{
					// Kill all running instances
					foreach (var process in Process.GetProcessesByName("inbox2"))
					{
						process.Kill();

						Thread.Sleep(3000);
					}

					return EnsureClientNotRunning();
				}

				return false;
			}

			return true;
		}

		protected bool EnsureSetupNotRunning()
		{
			var processes = Process.GetProcessesByName("inbox2upgrade");
			var current = Process.GetCurrentProcess();

			if (processes.Length > 1)
			{
				// Client is running
				if (MessageBox.Show("ANother instance of the Inbox2 upgrade executable is currently running, upgrade can not continue. Click ok to close the running instances or cancel to abort the upgrade.", "Question", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
				{
					// Kill all running instances
					foreach (var process in Process.GetProcessesByName("inbox2upgrade"))
					{
						if (process.Id != current.Id)
						{
							process.Kill();

							Thread.Sleep(500);
						}
					}

					return EnsureSetupNotRunning();
				}

				return false;
			}

			return true;
		}

		void Finished()
		{
			try
    		{
				ProgressTextBlock.Text = "Starting Inbox2...";

				// Exit application and start inbox2
				new Process { StartInfo = new ProcessStartInfo(Path.Combine(path, "inbox2.exe"))
                  	{
                  		WorkingDirectory = path, 
						Arguments = isUpgrade ? String.Empty : "/firstrun"
                  	}
				}.Start();

				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show("An exception has occured while trying to upgrade Inbox2. Exception = " + ex, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

				Application.Current.Shutdown();
			}
		}

		void Extract(object state)
		{
			try
    		{
				var zip = new FastZip();

				zip.ExtractZip(filename, path, FastZip.Overwrite.Prompt, name => !skipUpgrade.Contains(Path.GetFileName(name)), String.Empty, String.Empty, false);

				var key = Registry.CurrentUser.OpenSubKey("Software\\Inbox2\\Upgrade", true);

				if (key != null)
				{
					key.SetValue("version", Assembly.LoadFrom(Path.Combine(path, "Inbox2.exe")).GetName().Version.ToString());
					key.DeleteValue("filename");
					key.Close();
				}

				Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)Finished);
			}
			catch (Exception ex)
			{
				MessageBox.Show("An exception has occured while trying to upgrade Inbox2. Exception = " + ex, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

				Application.Current.Shutdown();
			}
		}	
	}
}
