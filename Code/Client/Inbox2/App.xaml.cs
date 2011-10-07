using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Inbox2.Core;
using Inbox2.Core.Configuration;
using Inbox2.Core.Threading.Handlers;
using Inbox2.Framework;
using Inbox2.Framework.Localization;
using Inbox2.Framework.Security;
using Inbox2.Framework.Stats;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Logging;
using Microsoft.Win32;
using MessageBox = System.Windows.MessageBox;

namespace Inbox2
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		readonly string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
		readonly Stopwatch stopwatch = new Stopwatch();

		bool isShuttingDown;
		bool startupSuccess;

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			
			// Fix for external startup environements (for instance when clicking on mailto links from IE)
			Directory.SetCurrentDirectory(path);

			AppDomain.CurrentDomain.UnhandledException += GlobalExceptionHandler;
			Current.DispatcherUnhandledException += GlobalWPFExceptionHandler;

			// Increase maximum concurrent HttpWebRequest connections
			ServicePointManager.DefaultConnectionLimit = 100;

			if (CheckForUpgrade())
			{
				// first check if we need elevated permissions
				new Process { StartInfo = new ProcessStartInfo(Path.Combine(path, "Inbox2Upgrade.exe")) 
					{ Arguments = "upgrade", UseShellExecute = true, Verb = "runas" } }.Start();

				// Shutdown current application
				Current.Shutdown();
			}
			else 
			{
				LoadTheme("/Settings/Application/Theme".AsKey("DarkSide"));

				// Run startup code
				ThreadPool.QueueUserWorkItem(delegate { Core.Startup.PyBinding(); });

				Core.Startup.Logging("log4net.config");
				Core.Startup.TypeConverters();
				Core.Startup.DataSources();
				Core.Startup.Search();
				Core.Startup.CorePlugins();
				Core.Startup.AppPlugins();
				Core.Startup.Plumbing();
				Core.Startup.Channels();				
				Core.Startup.Commands();
				Core.Startup.KeyboardHooks();
				Core.Startup.LoadStats();

				MessagesHandler.Init();
				DocumentsHandler.Init();
				ContactsHandler.Init();
				UserStatusHandler.Init();

				// Can also help in ssl debugging with Fiddler2
				if (SettingsManager.ClientSettings.AppConfiguration.IgnoreSslCertificateIssues)
					ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

				OverrideDependencyProperties();

				var window = new MainWindow();
				window.Show();

				stopwatch.Start();				

				startupSuccess = true;
			}
		}

		internal void OverrideDependencyProperties()
		{
			Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata { DefaultValue = 20 });
		}

		internal void Activate()
		{
			if (!MainWindow.IsVisible)
				MainWindow.Show();

			if (MainWindow.WindowState == WindowState.Minimized)
				MainWindow.WindowState = WindowState.Normal;

			MainWindow.Activate();
			MainWindow.Focus();
		}

		/// <summary>
		/// Checks if there is a file called upgrade.rar and extracts the contents, skipping the 
		/// Inbox2.exe and Inbox2.Core.Unmanaged.dll assemblies.
		/// 
		/// After the upgrade process finishes, we delete the upgrade.rar file.
		/// </summary>
		bool CheckForUpgrade()
		{
			RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Inbox2\\Upgrade", true);

			try
			{
				if (key != null)
				{
					var filename = key.GetValue("filename") as string;

					if (!String.IsNullOrEmpty(filename))
					{
						// Check if upgrade version matches current version
						var version = key.GetValue("version") as string;

						if (!String.IsNullOrEmpty(version))
						{
							if (new Version(version) != GetType().Assembly.GetName().Version)
							{
								// Looks like we have a orphaned update archive, remove from registry
								key.SetValue("version", GetType().Assembly.GetName().Version.ToString());
								key.DeleteValue("filename");

								return false;
							}
						}

						StringBuilder sb = new StringBuilder();
						sb.AppendLine(Strings.Inbox2DetectedAnUpgrade);
						
						if (!Security.IsAdmin())
						{
							sb.AppendLine();
							sb.AppendLine(Strings.NonAdminAccount);
						}

						return MessageBox.Show(sb.ToString(), "Inbox2 upgrade available", 
							MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.Yes) 
								== MessageBoxResult.Yes;
					}
				}

				return false;
			}
			finally
			{
				if (key != null)
					key.Close();
			}
		}

		void LoadTheme(string themename)
		{
			Resources.MergedDictionaries.Add(LoadComponent(new Uri("/Inbox2.UI.Resources;component/ResourceConverters.xaml", UriKind.Relative)) as ResourceDictionary);
            Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/SharedDictionary.xaml", themename), UriKind.Relative)) as ResourceDictionary);
            Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/Button.xaml", themename), UriKind.Relative)) as ResourceDictionary);
            Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/Border.xaml", themename), UriKind.Relative)) as ResourceDictionary);
            Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/CheckBox.xaml", themename), UriKind.Relative)) as ResourceDictionary);
            Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/ComboBox.xaml", themename), UriKind.Relative)) as ResourceDictionary);
            Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/Grid.xaml", themename), UriKind.Relative)) as ResourceDictionary);
            Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/Label.xaml", themename), UriKind.Relative)) as ResourceDictionary);
            Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/ListBox.xaml", themename), UriKind.Relative)) as ResourceDictionary);
            Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/ProgressBar.xaml", themename), UriKind.Relative)) as ResourceDictionary);
			Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/RadioButton.xaml", themename), UriKind.Relative)) as ResourceDictionary);
			Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/ScrollBar.xaml", themename), UriKind.Relative)) as ResourceDictionary);
			Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/ScrollViewer.xaml", themename), UriKind.Relative)) as ResourceDictionary);
			Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/Slider.xaml", themename), UriKind.Relative)) as ResourceDictionary);
			Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/TextBlock.xaml", themename), UriKind.Relative)) as ResourceDictionary);
			Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/TextBox.xaml", themename), UriKind.Relative)) as ResourceDictionary);
			Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/ToggleButton.xaml", themename), UriKind.Relative)) as ResourceDictionary);
			Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/TabControl.xaml", themename), UriKind.Relative)) as ResourceDictionary);
			Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/ToolTip.xaml", themename), UriKind.Relative)) as ResourceDictionary);
			Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/ListView.xaml", themename), UriKind.Relative)) as ResourceDictionary);
			Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/Menu.xaml", themename), UriKind.Relative)) as ResourceDictionary);
			Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/Dialog.xaml", themename), UriKind.Relative)) as ResourceDictionary);
            Resources.MergedDictionaries.Add(LoadComponent(new Uri(String.Format("/Inbox2.UI.Themes.{0};component/themes/Expander.xaml", themename), UriKind.Relative)) as ResourceDictionary);
		}
		
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Application.Exit"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.Windows.ExitEventArgs"/> that contains the event data.</param>
		protected override void OnExit(ExitEventArgs e)
		{
			if (startupSuccess)
			{
				isShuttingDown = true;

				stopwatch.Stop();

				ClientStats.LogEventWithTime("/Application/LastRunTime", new TimeSpan(stopwatch.ElapsedTicks).TotalSeconds);

				new Shutdown().Run();
			}

			base.OnExit(e);
		}

		#region Event handlers

		protected void GlobalExceptionHandler(object sender, UnhandledExceptionEventArgs e)
		{
			Logger.Fatal("A unhandled exception has occured. Exception = {0}", LogSource.UI, (Exception)e.ExceptionObject);

			// Log error synchronously since we are probably crashing
			UnhandledExceptionWindow.LogError((Exception)e.ExceptionObject);

			ShowErrorDialog((Exception)e.ExceptionObject);
		}

		protected void GlobalWPFExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			var ex = e.Exception;

			if (ex is ReflectionTypeLoadException)
			{
				var rtl = ((ReflectionTypeLoadException) ex);

				if (rtl.LoaderExceptions.Length > 0)
					ex = rtl.LoaderExceptions[0];
			}

			Logger.Fatal("A unhandled Dispatcher exception has occured. Exception = {0}", LogSource.UI, ex);

			e.Handled = true;

			ThreadPool.QueueUserWorkItem(state => UnhandledExceptionWindow.LogError(e.Exception));

			ShowErrorDialog(e.Exception);	
		}

		/// <summary>
		/// Shows the error dialog.
		/// </summary>
		/// <param name="e">The e.</param>
		void ShowErrorDialog(Exception e)
		{
			if (isShuttingDown || ClientState.StartupSuccess == false)
				return;

			if (Dispatcher.CheckAccess())
			{
				new UnhandledExceptionWindow(e) { Owner = Current.MainWindow }.ShowDialog();
			}
			else
			{
				Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => ShowErrorDialog(e)));
			}			
		}

		#endregion
	}
}
