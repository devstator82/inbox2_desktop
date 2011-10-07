using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using Inbox2.Platform.Framework.ServiceModel.Extensions;

namespace Inbox2.Platform.Framework.ServiceModel.Activation
{
	public class CloudServiceHost : WebServiceHost
	{
		public CloudServiceHost()
		{
		}

		public CloudServiceHost(Type serviceType, params Uri[] baseAddresses)
			:
				base(serviceType, baseAddresses)
		{
		}

		public CloudServiceHost(object singeltonInstance, params Uri[] baseAddresses)
			:
				base(singeltonInstance, baseAddresses)
		{
		}

		protected override void ApplyConfiguration()
		{
			base.ApplyConfiguration();

			InjectExtensions();
		}

		private void InjectExtensions()
		{
			foreach (ServiceEndpoint endpoint in Description.Endpoints)
			{
				endpoint.Behaviors.Add(new CloudEndpointBehavior());
			}
		}
	}
}