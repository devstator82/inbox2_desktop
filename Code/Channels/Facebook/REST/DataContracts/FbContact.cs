using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Channels.Facebook.REST.DataContracts
{
	public class FbContact
	{
		public string UserId { get; set; }
		public string Name { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public string AvatarSquareUrl { get; set; }
	}
}
