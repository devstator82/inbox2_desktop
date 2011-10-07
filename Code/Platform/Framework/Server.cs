using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public class Server
	{
		[DataMember(EmitDefaultValue = false, Order = 1)]
		public string Hostname { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 2)]
		public int? Port { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 3)]
		public string Username { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 4)]
		public string Password { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 5)]
		public bool IsSecured { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 6)]
		public string ConnectionType { get; set; }
	}
}
