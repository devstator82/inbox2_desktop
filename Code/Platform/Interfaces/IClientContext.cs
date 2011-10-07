using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Interfaces
{
	public interface IClientContext
	{
		string DisplayName { get; }

		bool HasSetting(string key);

		object GetSetting(string key);

		object GetSettingFrom(string filename);

		IEnumerable<object> GetSettingsFor(string keyPrefix);

		void SaveSetting(string key, object value);

		void SaveSettingTo(string filename, object value);
		
		void DeleteSetting(string key);
	}
}