using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Interfaces;

namespace Inbox2.Framework.Threading
{
	public class SetupChannelClientContext : IClientContext
	{
		private readonly Dictionary<string, object> values;

		public Dictionary<string, object> Values
		{
			get { return values; }
		}

		public SetupChannelClientContext()
		{
			 values = new Dictionary<string, object>();
		}

		public string DisplayName
		{
			get { return String.Empty; }
		}

		public bool HasSetting(string key)
		{
			return values.ContainsKey(key);
		}

		public object GetSetting(string key)
		{
			return values[key];
		}

		public object GetSettingFrom(string filename)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<object> GetSettingsFor(string keyPrefix)
		{
			throw new NotImplementedException();
		}

		public void SaveSetting(string key, object value)
		{
			values.Add(key, value);
		}

		public void SaveSettingTo(string filename, object value)
		{
			throw new NotImplementedException();
		}

		public void DeleteSetting(string key)
		{
			values.Remove(key);
		}
	}
}
