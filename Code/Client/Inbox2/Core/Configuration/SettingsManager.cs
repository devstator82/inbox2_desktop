using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Framework;

namespace Inbox2.Core.Configuration
{
	public static class SettingsManager
	{
		private static ClientSettings _ClientSettings;

		public static ClientSettings ClientSettings
		{
			get
			{
				if (_ClientSettings == null)
				{
					using (new JoeCulture())
					{
						_ClientSettings = new ClientSettings();
						_ClientSettings.Load();
					}
				}

				return _ClientSettings;
			}
		}

		public static void Save()
		{
			using (new JoeCulture())
				_ClientSettings.Save();
		}

		public static T SettingOrDefault<T>(string key, T defaultValue)
		{
			var obj = ClientState.Current.Context.GetSetting(key);

			if (obj == null)
				return defaultValue;

			if (!(obj is T))
				return defaultValue;

			return (T)obj;
		}

		public static object Read(string filename)
		{
			using (new JoeCulture())
			{
				string path = Path.Combine(DebugKeys.DefaultDataDirectory, filename);

				if (!File.Exists(path))
					return null;

				using (var stream = File.OpenRead(path))
				{
					NetDataContractSerializer ser = new NetDataContractSerializer();
					return ser.Deserialize(stream);
				}
			}
		}

		public static void Save(string filename, object value)
		{
			using (new JoeCulture())
			{
				string path = Path.Combine(DebugKeys.DefaultDataDirectory, filename);

				using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
				{
					NetDataContractSerializer ser = new NetDataContractSerializer();
					ser.Serialize(fs, value);
				}
			}
		}
	}
}
