using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Inbox2.Framework.Security
{
	public static class Security
	{
		public static bool IsAdmin()
		{
			if (VistaTools.IsReallyVista())
			{
				return VistaTools.IsElevated();
			}
			else
			{
				// Not vista, check if we are running under administrative priveleges
				var identity = WindowsIdentity.GetCurrent();
				var user = new WindowsPrincipal(identity);
				var sid = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);

				return user.IsInRole(sid);
			}
		}
	}
}
