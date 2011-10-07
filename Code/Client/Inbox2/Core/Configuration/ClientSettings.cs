using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Serialization;
using Inbox2.Platform.Logging;

namespace Inbox2.Core.Configuration
{
	public class ClientSettings
	{
		protected SerializableDictionary settings;

		public object this[string s]
		{
			get
			{
				if (settings.ContainsKey(s))
					return settings[s];

				return null;
			}
			set { settings[s] = value; }
		}

		public ClientSettings()
		{
			// todo better way of generating/storing key
			// Default encryption key
			SerializableDictionary.EncryptionKey = "letmein";

			settings = new SerializableDictionary();

			Load();
		}

		internal void Load()
		{
			if (!File.Exists(DebugKeys.DefaultDataDirectory + "\\Settings.xml"))
			{
				settings = new SerializableDictionary();
				return;
			}

			try
			{
				using (FileStream fs = new FileStream(DebugKeys.DefaultDataDirectory + "\\Settings.xml", FileMode.OpenOrCreate))
				{
					XmlSerializer ser = new XmlSerializer(typeof(SerializableDictionary));
					settings = (SerializableDictionary)ser.Deserialize(fs);
				}
			}
			catch (Exception ex)
			{
				Logger.Error("An error has occured while deserializing the settings, resetting settings. Exception = {0}", LogSource.Configuration, ex);

				settings = new SerializableDictionary();
			}
		}

		internal void Save()
		{
			try
			{
				using (FileStream fs = new FileStream(DebugKeys.DefaultDataDirectory + "\\Settings.xml", FileMode.Create))
				{
					XmlSerializer ser = new XmlSerializer(typeof(SerializableDictionary));
					ser.Serialize(fs, settings);
				}
			}
			catch (Exception ex)
			{
				Logger.Error("An error has occured while deserializing the settings, resetting settings. Exception = {0}", LogSource.Configuration, ex);
			}
		}

		public void Reset()
		{
			if (File.Exists(DebugKeys.DefaultDataDirectory + "\\Settings.xml"))
			{
				File.Delete(DebugKeys.DefaultDataDirectory + "\\Settings.xml");
			}
		}

		public Version Version
		{
			get
			{
				if (this["Version"] != null)
				{
					return new Version(this["Version"].ToString());
				}
				return new Version(1, 0);
			}
			set
			{
				this["Version"] = value.ToString();
			}
		}

		public Rect Location
		{
			get
			{
				if (this["Location"] != null)
				{
					return ((Rect)this["Location"]);
				}
				return Rect.Empty;
			}
			set
			{
				this["Location"] = value;
			}
		}

		public WindowState WindowState
		{
			get
			{
				if (this["WindowState"] != null)
				{
					return (WindowState)this["WindowState"];
				}
				return WindowState.Normal;
			}
			set
			{
				this["WindowState"] = value;
			}
		}

		public AppConfiguration AppConfiguration
		{
			get
			{
				if (this["AppConfiguration"] != null)
				{
					return (AppConfiguration)this["AppConfiguration"];
				}

				var auth = new AppConfiguration();

				this["AppConfiguration"] = auth;

				return auth;
			}
			set
			{
				this["AppConfiguration"] = value;
			}
		}		

		public SerializableDictionary ContextSettings
		{
			get
			{
				if (this["ContextSettings"] != null)
				{
					return (SerializableDictionary)this["ContextSettings"];
				}

				var settings = new SerializableDictionary();

				this["ContextSettings"] = settings;

				return settings;
			}
			set
			{
				this["ContextSettings"] = value;
			}
		}
	}
}
