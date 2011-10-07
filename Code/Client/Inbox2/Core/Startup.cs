using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using Inbox2.Core.Configuration;
using Inbox2.Core.DataAccess;
using Inbox2.Core.Search;
using Inbox2.Core.Storage;
using Inbox2.Core.Threading;
using Inbox2.Core.Threading.Tasks;
using Inbox2.Framework;
using Inbox2.Framework.Deployment;
using Inbox2.Framework.Extensions.ComponentModel;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Stats;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework;
using Inbox2.Platform.Framework.CloudApi.Logging;
using Inbox2.Platform.Framework.ComponentModel;
using Inbox2.Platform.Framework.Web;
using Inbox2.Platform.Logging;
using Inbox2.UI;
using Newtonsoft.Json;
using PyBinding;
using DebugKeys=Inbox2.Core.Configuration.DebugKeys;
using Document = Inbox2.Framework.VirtualMailBox.Entities.Document;
using Message = Inbox2.Framework.VirtualMailBox.Entities.Message;
using Person = Inbox2.Framework.VirtualMailBox.Entities.Person;
using Profile = Inbox2.Framework.VirtualMailBox.Entities.Profile;

namespace Inbox2.Core
{
	public class Startup
	{
		private static bool _isFirstRun;

		public static void PyBinding()
		{			
			// All assemblies used by PyBinding should be added here
			Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Inbox2.Platform.Channels.dll"));

			// Loads the IronPython runtime
			PythonEvaluator evaluator = new PythonEvaluator();
		}

		/// <summary>
		/// Initializes logging.
		/// </summary>
		public static void Logging(string filename)
		{
			if (Directory.Exists(DebugKeys.DefaultDataDirectory) == false)
			{
				Directory.CreateDirectory(DebugKeys.DefaultDataDirectory);
				_isFirstRun = true;
			}

			string logpath = Path.Combine(DebugKeys.DefaultDataDirectory, "logs");

			if (!Directory.Exists(logpath)) Directory.CreateDirectory(logpath);

			// Set the log path property which is used by our client logger
			log4net.GlobalContext.Properties["LogPath"] = logpath;

			using (Stream log4netConfigStream = typeof(Logger).Assembly.GetManifestResourceStream("Inbox2.Platform.Logging." + filename))
				Logger.Initialize(new Log4NetLogger(log4netConfigStream));

			Logger.Debug("__________________________", LogSource.Startup);
			Logger.Debug("Inbox2 application startup", LogSource.Startup);
			Logger.Debug("Version: " + Assembly.GetExecutingAssembly().GetName().Version, LogSource.Startup);
			Logger.Debug("Data directory is {0}", LogSource.Startup, DebugKeys.DefaultDataDirectory);
		}
		
		/// <summary>
		/// Initializes the data sources.
		/// </summary>
		public static void DataSources()
		{
			DatabaseUtil.InitializeDataStore();

			// Right now hardcoded to speed up startup
			ClientState.Current.DataService = new SQLiteDataService();
		}

		/// <summary>
		/// Initializes the search index.
		/// </summary>
		public static void Search()
		{
			SearchUtil.InitializeSearchStore();
		}

		/// <summary>
		/// Initializes the application plugins.
		/// </summary>
		public static void CorePlugins()
		{
			using (new CodeTimer("Startup/CorePlugins"))
			{
				ClientState.Current.Storage = new OpenFileStorage();
				ClientState.Current.Search = new Search.Search();
				ClientState.Current.TaskQueue = new TaskQueue();
				ClientState.Current.Context = new ClientContext();
				ClientState.Current.UndoManager = new UndoManager();
				ClientState.Current.ViewController = new ViewController();
			}			
		}

		public static void AppPlugins()
		{
			using (new CodeTimer("Startup/AppPlugins"))
			{
				// Build MEF catalog of components
				var catalog = new AggregateCatalog();

				catalog.Catalogs.Add(new DirectoryCatalog(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "*commands*.dll"));
				catalog.Catalogs.Add(new DirectoryCatalog(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "*.plugins.*.dll"));

				var container = new CompositionContainer(catalog);

				container.ComposeParts(PluginsManager.Current);
			}
		}

		/// <summary>
		/// Initializes the commands.
		/// </summary>
		public static void Commands()
		{
			EventBroker.Subscribe(AppEvents.RequestSend, Tasks.Send);

			EventBroker.Subscribe(AppEvents.RequestReceive, Tasks.ReceivePrio);
			EventBroker.Subscribe(AppEvents.RequestReceive, (int pageSize) => Tasks.ReceivePage(pageSize));
			EventBroker.Subscribe(AppEvents.RequestReceive, (ChannelInstance channel) => Tasks.Receive(channel));

			EventBroker.Subscribe(AppEvents.RequestSync, Tasks.SyncPrio);
			EventBroker.Subscribe(AppEvents.RequestSync, (ChannelInstance channel) => Tasks.Sync(channel));

			EventBroker.Subscribe(AppEvents.RequestCommands, Tasks.Commands);

			NetworkChange.NetworkAvailabilityChanged += delegate 
			{
				if (NetworkInterface.GetIsNetworkAvailable())
				{
					Tasks.Send();
					Tasks.Commands();					
					Tasks.Receive();
				}
			};
		}

		/// <summary>
		/// Initializes the database and stuff.
		/// </summary>
		public static void Plumbing()
		{
			var clientId = SettingsManager.ClientSettings.AppConfiguration.ClientId;
			var baseUrl = String.Format("http://download{0}.inbox2.com/",
				   String.IsNullOrEmpty(CommandLine.Current.Environment) ? String.Empty : "." + CommandLine.Current.Environment);

			Migrate.Up(typeof(ChannelConfig));
			Migrate.Up(typeof(Conversation));
			Migrate.Up(typeof(Message));
			Migrate.Up(typeof(Document));
			Migrate.Up(typeof(DocumentVersion));
			Migrate.Up(typeof(Person));
			Migrate.Up(typeof(Profile));
			Migrate.Up(typeof(UserStatus));
			Migrate.Up(typeof(UserStatusAttachment));
			Migrate.Up(typeof(QueuedCommand));
			Migrate.Up(typeof(FeedItem));

			var appVersion = Assembly.GetExecutingAssembly().GetName().Version;
			var savedVersion = SettingsManager.ClientSettings.Version;

			Logger.Warn("Saved version {0}", LogSource.Startup, savedVersion);

			if (_isFirstRun || Environment.CommandLine.Contains("/firstrun"))
			{
				HttpServiceRequest.Post(baseUrl + "version/install", 
					String.Format("clientId={0}&version={1}", clientId, appVersion));
			}

			if (_isFirstRun == false && appVersion > savedVersion)
			{
				Logger.Debug("Upgrade detected, performing nescessary upgrade actions", LogSource.Startup);

				// Get all upgrades
				UpgradeActionBase.Upgrades.AddRange(typeof(Startup)
					.Assembly.GetTypes()
					.Where(t => t.IsSubclassOf(typeof (UpgradeActionBase)))
					.Select(t => (UpgradeActionBase) Activator.CreateInstance(t))
					.Where(i => i.TargetVersion > savedVersion)
					.OrderBy(i => i.TargetVersion));

				UpgradeActionBase.Upgrades.ForEach(i => i.Upgrade());

				HttpServiceRequest.Post(baseUrl + "version/upgrade", 
					String.Format("clientId={0}&from={1}&to={2}", clientId, savedVersion, appVersion));
			}

			// Save current version
			SettingsManager.ClientSettings.Version = appVersion;
		}	

		/// <summary>
		/// Loads and initializes the channels.
		/// </summary>
		public static void Channels()
		{
			var channels = ClientState.Current.DataService.SelectAll<ChannelConfig>();			

			foreach (var channel in channels)
				ChannelsManager.Add(ChannelFactory.Create(channel));
		}

		/// <summary>
		/// Initializes the type converters.
		/// </summary>
		public static void TypeConverters()
		{
			TypeDescriptor.AddAttributes(typeof(SourceAddress),
				new TypeConverterAttribute(typeof(SourceAddressConverter)));

			TypeDescriptor.AddAttributes(typeof(SourceAddressCollection),
				new TypeConverterAttribute(typeof(SourceAddressCollectionConverter)));

			TypeDescriptor.AddAttributes(typeof(Stream),
				new TypeConverterAttribute(typeof(StreamConverter)));
		}

		/// <summary>
		/// Gets the app version.
		/// </summary>
		/// <returns></returns>
		public static Version GetAppVersion()
		{
			//Get the version of the currently executing Assembly
			Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

			//Check to see if we are ClickOnce Deployed
			if (ApplicationDeployment.IsNetworkDeployed)
				currentVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion;

			return currentVersion;
		}

		/// <summary>
		/// Initializes the keyboard hooks.
		/// </summary>
		public static void KeyboardHooks()
		{
			//Keyboard.Initialize();
		}

		public static void LoadStats()
		{
			// Check if we have a saved logs
			if (SettingsManager.ClientSettings.ContextSettings.ContainsKey("/Application/StoredStats"))
			{
				var storedlogs = SettingsManager.ClientSettings.ContextSettings["/Application/StoredStats"];

				if (storedlogs != null)
				{
					// Deserialize logs
					using (var sr = new StringReader((string) storedlogs))
					{
						var ser = new JsonSerializer();
                        var logs = (List<TraceInfo>)ser.Deserialize(sr, typeof(List<TraceInfo>));

						ClientStats.ReEnqueue(logs);

						SettingsManager.ClientSettings.ContextSettings.Remove("/Application/StoredStats");
						SettingsManager.Save();
					}
				}
			}
		}
	}
}