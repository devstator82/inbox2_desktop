using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using Inbox2.Core.Configuration;
using Inbox2.Core.Threading.Repeat;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Threading;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Logging;
using Microsoft.Win32;

namespace Inbox2.Core.Threading.Tasks.Application
{
	public class CheckForUpdateTask : BackgroundTask
	{
		public override bool RequiresNetworkConnection
		{
			get { return true; }
		}

		string AssetBaseUrl
		{
			get
			{
				return String.Format("http://download{0}.inbox2.com/",
				   String.IsNullOrEmpty(CommandLine.Current.Environment) ? String.Empty : "." + CommandLine.Current.Environment);				
			}
		}		

		protected override void ExecuteCore()
		{
			string versionString;

			if (CheckNeedsUpgrade(out versionString))
				DownloadUpdate(versionString);

			//DownloadRssUpdates("Twitter", "http://twitter.com/statuses/user_timeline/16018665.rss");
			//DownloadRssUpdates("Blog", "http://feeds.feedburner.com/inbox2");
			//DownloadRssUpdates("Facebook", "http://www.facebook.com/feeds/page.php?format=rss20&id=82632103211");

			EventBroker.Publish(AppEvents.UpdateCheckFinished);
		}		

		public bool CheckNeedsUpgrade(out string versionString)
		{
			var wc = new WebClient();
			var currentVersion = GetType().Assembly.GetName().Version;
			var clientId = SettingsManager.ClientSettings.AppConfiguration.ClientId;

			versionString = wc.DownloadString(AssetBaseUrl + String.Format("version/latest?clientId={0}&version={1}", clientId, currentVersion));

			return (new Version(versionString) > currentVersion);
		}

		void DownloadUpdate(string versionString)
		{
			Logger.Debug("Starting download of upgrade archive", LogSource.BackgroundTask);

			var wc = new WebClient();
			var filename = Path.GetTempFileName();
			string url;

			if (String.IsNullOrEmpty(CommandLine.Current.Environment))
				url = String.Format("http://cdn.inbox2.com/client/x86/upgrade {0}.zip", versionString);
			else
				url = String.Format("http://download.{0}.inbox2.com/client/x86/upgrade {1}.zip", CommandLine.Current.Environment, versionString);

			wc.DownloadFile(url, filename);

			RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Inbox2\\Upgrade", true);

			if (key == null)
				key = Registry.CurrentUser.CreateSubKey("Software\\Inbox2\\Upgrade");

			key.SetValue("filename", filename);
			key.SetValue("version", GetType().Assembly.GetName().Version.ToString());
			key.Close();

			Logger.Debug("Finished download of upgrade archive", LogSource.BackgroundTask);
			Logger.Debug("Pending upgrade filename = {0}", LogSource.BackgroundTask, filename);
		}

		public override void OnCompleted()
		{
			// Schedule next execution
			new Run("UpdateCheck").After(60).Minutes().Call(Tasks.CheckForUpdate);

			base.OnCompleted();
		}
	}
}
