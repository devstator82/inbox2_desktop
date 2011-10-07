using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Inbox2.Platform.Framework.ServiceModel.DataContracts
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public class Result
	{
		[DataMember]
		public int ResultCode;

		[DataMember]
		public string ResultText;

		public Result()
		{
			ResultText = "Success";
		}
	}
}