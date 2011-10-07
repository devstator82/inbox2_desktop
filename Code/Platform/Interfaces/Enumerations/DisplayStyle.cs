using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Interfaces.Enumerations
{
	[Serializable]
	public enum DisplayStyle
	{
		None = 0,
		Simple = 1,
		Advanced = 2,
		Redirect = 3,
		RedirectWithPin = 4,
		Other = 5,
		FbConnect = 6
	}
}