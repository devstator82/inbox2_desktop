using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Inbox2.Platform.Framework.ServiceModel.Extensions
{
	public class CloudEndpointBehavior : Attribute, IEndpointBehavior
	{
		public void Validate(ServiceEndpoint endpoint)
		{

		}

		public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{

		}

		public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
		{
			//endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new CloudMessageInspector());
		}

		public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
			throw new NotImplementedException();
		}
	}
}
