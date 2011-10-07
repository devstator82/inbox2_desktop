using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Inbox2.Platform.Framework.ServiceModel.DataContracts
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public abstract class RequestBase
	{
		[DataMember]
		public Guid Tracer { get; set; }

		[DataMember]
		public long UserId { get; set; }

		public void Validate()
		{
			var result = ValidateCore();

			if (result == false)
			{
				throw new Inbox2ServiceException("A request validation error occured.");
			}
		}

		public virtual bool ValidateCore()
		{
			return true;
		}
	}
}
