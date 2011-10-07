using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Inbox2.Platform.Framework
{
	public class SafeSession
	{
		private static readonly SafeSession _Current = new SafeSession();

		public static SafeSession Current
		{
			get { return _Current; }
		}

		static bool HasHttpContext
		{
			get { return HttpContext.Current != null; }
		}

		private readonly Dictionary<string, object> nonHttpSession = new Dictionary<string, object>();

		public object this[string key]
		{
			get
			{
				if (HasHttpContext)
					return HttpContext.Current.Session[key];
				else
				{
					if (nonHttpSession.ContainsKey(key))
						return nonHttpSession[key];

					return null;
				}				
			}
			set
			{
				if (HasHttpContext)
					HttpContext.Current.Session[key] = value;
				else
					nonHttpSession[key] = value;
			}
		}

		public void Remove(string key)
		{
			if (HasHttpContext)
			{
				HttpContext.Current.Session.Remove(key);
			}
			else
			{
				nonHttpSession.Remove(key);
			}
		}

		private SafeSession()
		{			
		}
	}
}