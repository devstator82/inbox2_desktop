using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Channels.Facebook.REST.DataContracts
{
	public class FbMessage
	{
		public string MessageId { get; set; }					
		public string ThreadId { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }

		public bool Read { get; set; }

		public DateTime DateCreated { get; set; }
		
		public SourceAddress From { get; set; }
		public SourceAddressCollection To { get; set; }
	}
}