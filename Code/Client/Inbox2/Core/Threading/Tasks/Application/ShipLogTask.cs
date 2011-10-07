using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Inbox2.Core.Configuration;
using Inbox2.Core.Threading.Repeat;
using Inbox2.Framework;
using Inbox2.Framework.Stats;
using Inbox2.Framework.Threading;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Framework.Web;
using Inbox2.Platform.Logging;
using Newtonsoft.Json;

namespace Inbox2.Core.Threading.Tasks.Application
{
	public class ShipLogTask : BackgroundTask
	{
		private static string _SessionId = Guid.NewGuid().GetHash(12);

		public override bool RequiresNetworkConnection
		{
			get { return true; }
		}

		private string AssetBaseUrl
		{
			get
			{
				return String.Format("http://download{0}.inbox2.com/",
				   String.IsNullOrEmpty(CommandLine.Current.Environment) ? String.Empty : "." + CommandLine.Current.Environment);
			}
		}

		protected override bool CanExecute()
		{
			return !SettingsManager.ClientSettings.AppConfiguration.IsStatsDisabled;
		}

		protected override void ExecuteCore()
		{
			
		}

		public override void OnCompleted()
		{
			// Schedule next execution
			new Run("ShipLogs").After(5).Minutes().Call(Tasks.ShipLogs);

			base.OnCompleted();
		}

		static string ToJson(object o)
		{
			var sb = new StringBuilder();

			using (var sw = new StringWriter(sb))
			{
				// Create json representation of data
				var ser = new JsonSerializer();
				ser.Serialize(sw, o);
			}

			return sb.ToString();
		}
	}
}
