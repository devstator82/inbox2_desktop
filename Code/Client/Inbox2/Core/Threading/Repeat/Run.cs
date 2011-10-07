using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Core.Threading.Repeat
{
	public class Run : ScheduledItem
	{
		public int repeatDelay;

		public Run(string key) : base(key)
		{
		}

		public Run After(int repeatDelay)
		{
			this.repeatDelay = repeatDelay;

			return this;
		}

		public Run Seconds()
		{
			DateScheduled = DateTime.Now.AddSeconds(repeatDelay);

			return this;
		}

		public Run Minutes()
		{
			DateScheduled = DateTime.Now.AddMinutes(repeatDelay);

			return this;
		}
	}
}
