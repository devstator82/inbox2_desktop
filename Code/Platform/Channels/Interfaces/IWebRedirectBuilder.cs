using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Platform.Channels.Interfaces
{
	public interface IWebRedirectBuilder
	{
		string Token { get; set; }

		string TokenSecret { get; set; }

		string SuccessUri { get; }

		Uri BuildRedirectUri();
		
		string ParseVerifier(string returnValue);		

		bool ValidateReturnValue(string returnValue);

		string GetUsername();
	}
}
