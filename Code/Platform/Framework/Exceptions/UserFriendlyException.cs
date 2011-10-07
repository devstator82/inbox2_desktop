using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Framework.Exceptions
{
	public class UserFriendlyException : ApplicationException
	{
		public UserFriendlyException(string message) : base(message)
		{
		}
	}
}