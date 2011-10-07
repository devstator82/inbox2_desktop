using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Channels.Facebook.REST.DataContracts
{
	public class FbStatus
	{
		public long Uid { get; set; }
		public string StatusId { get; set; }
		public string Message { get; set; }
		public SourceAddress From { get; set; }
		public SourceAddress To { get; set; }
		public DateTime DateCreated { get; set; }

		public List<FbStatus> Comments { get; private set; }
		public List<FbAttachment> Attachments { get; private set; }

		public FbStatus()
		{
			Comments = new List<FbStatus>();
			Attachments = new List<FbAttachment>();
		}
	}

	public class FbStatusComparer : IEqualityComparer<FbStatus>
	{
		public bool Equals(FbStatus x, FbStatus y)
		{
			return x.StatusId.Equals(y.StatusId);
		}

		public int GetHashCode(FbStatus obj)
		{
			return obj.StatusId.GetHashCode();
		}
	}
}
