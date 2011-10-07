using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Interfaces;

namespace Inbox2.Platform.Framework.ServiceModel
{
	public class WebServiceTracer : ITracer
	{
		[ThreadStatic]
		public static Guid TracerId;

		[ThreadStatic]
		public static long UserId;

		public override Guid GetTacerId()
		{
			return TracerId;
		}

		public override long GetUserId()
		{
			return UserId;
		}
	}

	public class WebServiceTracerContext : ITracerContext
	{
		public ITracer Tracer
		{
			get { return new WebServiceTracer(); }
		}
	}
}
