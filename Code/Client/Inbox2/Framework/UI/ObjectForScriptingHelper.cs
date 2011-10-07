using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Inbox2.Framework.Interfaces.Enumerations;

namespace Inbox2.Framework.UI
{
	[ComVisible(true)]
	public class ObjectForScriptingHelper
	{
		public void JsNavigate(string uri)
		{
			if (uri.ToLower().StartsWith("mailto:"))
			{
				EventBroker.Publish(AppEvents.New, uri);				
			}
			else
			{
				new Process { StartInfo = new ProcessStartInfo(uri) }.Start();
			}
		}
	}
}
