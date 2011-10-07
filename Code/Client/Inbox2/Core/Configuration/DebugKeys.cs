using System;
using System.IO;
using System.Linq;
using Inbox2.Framework;
using Inbox2.Platform.Framework.Extensions;

namespace Inbox2.Core.Configuration
{
	public static class DebugKeys
	{
		private static string _dataDir;

		public static string DefaultDataDirectory
		{
			get
			{
				if (String.IsNullOrEmpty(_dataDir))
				{					
					// If the data switch is present, use that
					if (!String.IsNullOrEmpty(CommandLine.Current.DataDir))
					{
						if (CommandLine.Current.DataDir.IndexOfAny(Path.GetInvalidPathChars()) == -1)
							_dataDir = CommandLine.Current.DataDir;
					}
					else
					{						
						var appPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

						// Check for data folder in the current application directory
						if (Directory.Exists(Path.Combine(appPath, "data")))
							_dataDir = Path.Combine(appPath, "data");
						else
							// Default behavior: use application data
							_dataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Inbox2");	
					}
				}

				return _dataDir;
			}
		}

		public static int DefaultNrOfProcessors
		{
			get
			{
				return "/Inbox2/Client/Debug/DefaultNrOfProcessors".AsKey(10);
			}
		}

		public static int DefaultPageSize
		{
			get
			{
				return "/Inbox2/Client/Debug/DefaultPageSize".AsKey(50);
			}
		}

		public static string[] DisabledPlugins
		{
			get
			{
				var setting = "/Settings/Plugins/Disabled".AsKey(String.Empty);

				return setting.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)
					.Select(s => s.Trim())
					.ToArray();
			}
		}

		public static int HttpListenerPort
		{
			get
			{
				return "/Settings/Application/HttpListenerPort".AsKey(62101);
			}
		}
	}
}
