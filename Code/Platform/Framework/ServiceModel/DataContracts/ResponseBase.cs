using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Inbox2.Platform.Framework.ServiceModel.DataContracts
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public abstract class ResponseBase
	{
		[DataMember]
		public Result Result { get; set; }

		protected ResponseBase()
		{
			Result = new Result();
		}
	}
}