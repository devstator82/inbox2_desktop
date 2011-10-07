using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpServer;

namespace Inbox2ClientWorker.HttpListener
{
	public interface IRestEndpoint
	{
		string Url { get; }

		void Process(RequestContext context);
	}
}
