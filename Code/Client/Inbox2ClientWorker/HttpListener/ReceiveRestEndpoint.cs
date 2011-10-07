using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpServer;
using Inbox2ClientWorker.Core.Threading.Tasks;

namespace Inbox2ClientWorker.HttpListener
{
	public class ReceiveRestEndpoint : IRestEndpoint
	{
		public string Url
		{
			get { return "/receive"; }
		}

		/// <summary>
		/// Executes either a ReceivePrio Receive(channel) or ReceivePage(pageSize).
		/// </summary>
		/// <param name="context"></param>
		public void Process(RequestContext context)
		{
			Tasks.ReceivePrio();
		}
	}
}
