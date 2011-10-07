using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using HttpServer;
using HttpServer.Headers;
using HttpServer.Modules;

namespace Inbox2ClientWorker.HttpListener
{
	internal class RestModule : IModule
	{
		private readonly List<IRestEndpoint> endpoints;

		public List<IRestEndpoint> Endpoints
		{
			get { return endpoints; }
		}

		public RestModule()
		{
			endpoints = new List<IRestEndpoint>();
		}		

		public ProcessingResult Process(RequestContext context)
		{
			var ep = endpoints.Where(e => context.Request.Uri.AbsolutePath.StartsWith(e.Url)).ToList();

			if (ep.Count == 0)
				throw new NotFoundException("Handler not found");

			foreach (var endpoint in ep)
				endpoint.Process(context);

			context.Response.Status = HttpStatusCode.OK;
			context.Response.Connection.Type = ConnectionType.Close;

			return ProcessingResult.Abort;
		}
	}
}
