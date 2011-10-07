using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;

namespace Inbox2.Framework.Threading
{
	[DataContract]
	public abstract class ConnectedCommand : CommandBase
	{
		public override bool CanExecute
		{
			get { return NetworkInterface.GetIsNetworkAvailable();  }
		}
	}
}
