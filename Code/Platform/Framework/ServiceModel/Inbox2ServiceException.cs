using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Framework.ServiceModel
{
	public class Inbox2ServiceException : ApplicationException
	{
		public Inbox2ServiceException(string message)
			: base(message)
		{
		}

		public Inbox2ServiceException(string message, Exception innerException) : base(message, innerException)
		{
		}		
	}
}
