using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.VirtualMailBox.Entities;

namespace Inbox2.UI.Helpers
{
	internal class CoolSearchResult
	{
		public object Result { get; private set; }

		public CoolSearchResult(object result)
		{
			Result = result;
		}

		public void NavigateTo()
		{
			if (Result is Conversation)
			{
				EventBroker.Publish(AppEvents.View, ((Conversation)Result).Last);
			}
			else if (Result is Document)
			{
				EventBroker.Publish(AppEvents.View, (Document)Result);
			}
			else if (Result is Person)
			{
				EventBroker.Publish(AppEvents.View, (Person)Result);
			}
			else if (Result is Label)
			{
				EventBroker.Publish(AppEvents.View, (Label)Result);
			}
		}
	}
}
