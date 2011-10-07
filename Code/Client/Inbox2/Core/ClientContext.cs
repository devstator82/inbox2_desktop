using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Inbox2.Core.Configuration;
using Inbox2.Platform.Interfaces;

namespace Inbox2.Core
{
	[Export(typeof(IClientContext))]
	public class ClientContext : IClientContext
	{
		public string DisplayName
		{
			get { return SettingsManager.ClientSettings.AppConfiguration.Fullname; }
		}

		public bool HasSetting(string key)
		{
			return SettingsManager.ClientSettings.ContextSettings.ContainsKey(key);
		}

		public object GetSetting(string key)
		{
			if (!SettingsManager.ClientSettings.ContextSettings.ContainsKey(key))
				return String.Empty;

			return SettingsManager.ClientSettings.ContextSettings[key];			
		}

		public object GetSettingFrom(string filename)
		{
			return SettingsManager.Read(filename);
		}

		public IEnumerable<object> GetSettingsFor(string keyPrefix)
		{
			foreach (var pair in SettingsManager.ClientSettings.ContextSettings)
			{
				if (pair.Key.StartsWith(keyPrefix))
					yield return pair.Value;
			}
		}

		public void SaveSetting(string key, object value)
		{
			if (SettingsManager.ClientSettings.ContextSettings.ContainsKey(key))
				SettingsManager.ClientSettings.ContextSettings.Remove(key);

			SettingsManager.ClientSettings.ContextSettings.Add(key, value);
			SettingsManager.Save();
		}

		public void SaveSettingTo(string filename, object value)
		{
			SettingsManager.Save(filename, value);
		}

		public void DeleteSetting(string key)
		{
			SettingsManager.ClientSettings.ContextSettings.Remove(key);
			SettingsManager.Save();
		}
	}
}
