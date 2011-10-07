using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Interfaces.ValueTypes
{
	public class ReceiveRange
	{
		public long PageSize { get; set; }

		public bool OnlyNew { get; set; }

		public static ReceiveRange Default
		{
			get { return new ReceiveRange {OnlyNew = true, PageSize = 50}; }
		}
	}
}