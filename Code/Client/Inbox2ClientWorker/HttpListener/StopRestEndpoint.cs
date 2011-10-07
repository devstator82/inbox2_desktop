using System;
using System.Windows.Forms;
using HttpServer;

namespace Inbox2ClientWorker.HttpListener
{
	public class StopRestEndpoint : IRestEndpoint
	{
		public string Url
		{
			get { return "/stop"; }
		}

		public void Process(RequestContext context)
		{
			Application.Exit();
		}
	}
}