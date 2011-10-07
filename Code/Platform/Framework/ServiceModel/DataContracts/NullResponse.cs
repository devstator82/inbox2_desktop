using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Platform.Framework.ServiceModel.DataContracts
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public class NullResponse : ResponseBase
	{
		public static NullResponse Default = new NullResponse();
	}
}
