using System;
using System.IO;
using System.Text;
using Inbox2.Core.Configuration;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Stats;
using Inbox2.Platform.Channels.Connections;
using Inbox2.Platform.Framework.Web;
using Inbox2.UI.Launcher;
using Newtonsoft.Json;

namespace Inbox2.Core
{
	public class Shutdown
	{
		public void Run()
		{
			EventBroker.Publish(AppEvents.ShuttingDown);

			Stats();
			KeyboardHooks();
			SaveSettings();
			Connections();
			Databases();
			Storage();			
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is shutting down.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is shutting down; otherwise, <c>false</c>.
		/// </value>
		public static bool IsShuttingDown { get; set; }

		public static void SaveSettings()
		{
			foreach (var plugin in PluginsManager.Current.Plugins)
			{
				if (plugin.State != null)
					plugin.State.Shutdown();
			}

			SettingsManager.Save();
		}

		public static void Connections()
		{
			ConnectionPoolScavenger.Shutdown();
		}

		public static void Databases()
		{
		}

		public static void Storage()
		{
			ClientState.Current.Storage.Dispose();
		}

		public static void KeyboardHooks()
		{
			Keyboard.Shutdown();
		}

		public static void Stats()
		{
			var stats = ClientStats.Flush();

			if (stats.Count > 0)
			{
				var sb = new StringBuilder();

				using (var sw = new StringWriter(sb))
				{
					// Create json representation of data
					var ser = new JsonSerializer();
					ser.Serialize(sw, stats);
				}

				SettingsManager.ClientSettings.ContextSettings["/Application/StoredStats"] = sb.ToString();
			}
		}

		/// <summary>
		/// Resets the data store.
		/// </summary>
		public static void ResetDataStore()
		{
			if (Directory.Exists(DebugKeys.DefaultDataDirectory) == false)
				Directory.Delete(DebugKeys.DefaultDataDirectory, true);
		}
	}
}
