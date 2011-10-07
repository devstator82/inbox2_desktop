using System;
using Inbox2.Framework;

namespace Inbox2.Core.Configuration
{
	public static class CloudApi
	{
		public static string ApiBaseUrl
		{
			get
			{
				return String.Format("http://api{0}.inbox2.com/",
				   String.IsNullOrEmpty(CommandLine.Current.Environment) ? String.Empty : "." + CommandLine.Current.Environment);
			}
		}

		public static string ApplicationKey
		{
			get
			{
				return "ZABhADQAMgA4AGQAYQAyAA==";
			}
		}

		public static string AccessToken
		{
			get
			{
				return SettingsManager.ClientSettings.AppConfiguration.AuthToken;
			}
		}
	}
}
