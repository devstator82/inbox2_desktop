using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.Utils.Viral
{
	public static class Messages
	{
		static string[] _ = new[]
	     	{
				@"Streaming my email and twitter with #inbox2 #desktop, don't miss out! http://bit.ly/9pHFfd",
				@"I just downloaded the coolest email client ever! Get it while it's hot http://bit.ly/dwGjHv #inbox2",
				@"Enjoying aggregated email, twitter and facebook in #inbox2, you have got to try this out http://bit.ly/9AQ54N"
	     	};

		public static string Next()
		{
			return _[new Random().Next(0, 3)];
		}
	}
}
