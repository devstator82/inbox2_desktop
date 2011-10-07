using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Interfaces.Enumerations
{
	[Serializable]
	public enum ExecutionStatus
	{
		Pending,
		Submitted,
		Running,
		Success,
		Error
	}
}