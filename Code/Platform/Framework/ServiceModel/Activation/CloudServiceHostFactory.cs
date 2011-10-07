using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;

namespace Inbox2.Platform.Framework.ServiceModel.Activation
{
	public class CloudServiceHostFactory : WebServiceHostFactory
	{
		protected override System.ServiceModel.ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
		{
			return new CloudServiceHost(serviceType, baseAddresses[0]);
		}
	}
}