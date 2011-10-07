using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inbox2.Platform.Framework.Exceptions;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;
using Inbox2.Platform.Logging;
using Logger=Inbox2.Platform.Logging.Logger;

namespace Inbox2.Platform.Framework.ServiceModel
{
	public abstract class HandlerBase<Req, Resp> where Resp : ResponseBase, new() where Req: RequestBase
	{
		public Resp Process(Req request)
		{
			try
			{
				WebServiceTracer.TracerId = request.Tracer;
				WebServiceTracer.UserId = request.UserId;

				request.Validate();

				return ProcessCore(request);
			}
			catch (UserFriendlyException ex)
			{
				Logger.Warn("Service handler threw an exception. Exception = {0}", LogSource.Sync, ex);

				Resp response = new Resp();

				response.Result = new Result();
				response.Result.ResultCode = -1;
				response.Result.ResultText = ex.Message;

				return response;
			}
			catch (Exception ex)
			{
				Logger.Warn("Service handler threw an exception. Exception = {0}", LogSource.Sync, ex);

				Resp response = new Resp();

				response.Result = new Result();
				response.Result.ResultCode = -1;
				response.Result.ResultText = "A general processing error has occured.";

				return response;
			}
		}

		public abstract Resp ProcessCore(Req request);
	}
}