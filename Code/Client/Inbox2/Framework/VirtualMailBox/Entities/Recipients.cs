using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Framework.VirtualMailBox.Entities
{
	public class Recipients
	{
		public SourceAddressCollection To { get; set; }
		public SourceAddressCollection CC { get; set; }
		public SourceAddressCollection BCC { get; set; }

		public bool IsEmpty
		{
			get
			{
				return !((To.Count > 0) || (CC.Count > 0) || (BCC.Count > 0));
			}
		}

		public Recipients()
		{
			To = new SourceAddressCollection();
			CC = new SourceAddressCollection();
			BCC = new SourceAddressCollection();
		}
	}
}
