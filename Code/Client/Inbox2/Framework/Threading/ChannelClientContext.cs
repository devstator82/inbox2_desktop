using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Interfaces;

namespace Inbox2.Framework.Threading
{
	public class ChannelClientContext : IClientContext
	{
		private readonly IClientContext inner;
		private readonly ChannelConfiguration config;

		public ChannelClientContext(IClientContext inner, ChannelConfiguration config)
		{
			this.inner = inner;
			this.config = config;
		}

		public string DisplayName
		{
			get { return inner.DisplayName; }
		}

		public bool HasSetting(string key)
		{
			return inner.HasSetting(AppendKey(key));
		}

		public object GetSetting(string key)
		{
			return inner.GetSetting(AppendKey(key));
		}

		public object GetSettingFrom(string filename)
		{
			return inner.GetSettingFrom(filename);
		}

		public IEnumerable<object> GetSettingsFor(string keyPrefix)
		{
			return inner.GetSettingsFor(AppendKey(keyPrefix));
		}

		public void SaveSetting(string key, object value)
		{
			inner.SaveSetting(AppendKey(key), value);
		}

		public void SaveSettingTo(string filename, object value)
		{
			inner.SaveSettingTo(filename, value);
		}

		public void DeleteSetting(string key)
		{
			inner.DeleteSetting(AppendKey(key));
		}

		string AppendKey(string key)
		{
			return String.Format("/Settings/Channels/{0}/{1}", config.ChannelId, key);
		}
	}
}
