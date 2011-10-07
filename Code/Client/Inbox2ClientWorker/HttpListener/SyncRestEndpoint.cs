using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpServer;
using Inbox2ClientWorker.Core.Threading.Tasks;

namespace Inbox2ClientWorker.HttpListener
{
	public class SyncRestEndpoint : IRestEndpoint
	{
		public string Url
		{
			get { return "/sync"; }
		}

		/// <summary>
		/// Executes either a syncprio or sync(channel).
		/// </summary>
		/// <param name="context"></param>
		public void Process(RequestContext context)
		{
			Tasks.SyncPrio();
		}
	}
}
