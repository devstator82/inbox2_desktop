using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Inbox2.Core.Configuration;
using Inbox2.Core.Threading.Tasks;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Threading;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Framework.CloudApi.Enumerations;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Framework.Web;
using Inbox2.Platform.Logging;

namespace Inbox2.Core.Threading.Commands
{
	class SyncEntityCommand : ConnectedCommand, IContextTask
	{
		private readonly string key;
		private readonly Entities entities;
		private readonly QueuedCommand inner;

		public Dictionary<string, object> Values { get; private set; }

		public SyncEntityCommand(Entities entities, string key, QueuedCommand inner)
		{
			Values = new Dictionary<string, object> { { "wrap_access_token", CloudApi.AccessToken } };

			this.key = key;
			this.entities = entities;
			this.inner = inner;
		}

		protected override void ExecuteCore()
		{
			try
			{
				Values.Add("keys", key);
				Values.Add("value", inner.Value);
				Values.Add("modifyaction", inner.ModifyAction);

				var data = String.Join("&", Values.Select(kv => String.Format("{0}={1}", kv.Key, kv.Value)).ToArray());

				HttpServiceRequest.Post(CloudApi.ApiBaseUrl + String.Format("modify/{0}", entities), data, true);
			}
			catch (WebException ex)
			{
				var response = (HttpWebResponse)ex.Response;

				using (var stream = response.GetResponseStream())
					Logger.Error("An error has occured while executing cloud command. Error = {0}", LogSource.Command, stream.ReadString());

				throw;
			}			
		}
	}
}