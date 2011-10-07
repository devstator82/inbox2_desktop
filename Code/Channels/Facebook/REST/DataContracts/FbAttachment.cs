using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Channels.Facebook.REST.DataContracts
{
	public class FbAttachment
	{
		public string PreviewImageUrl { get; set; }

		public string PreviewAltText { get; set; }

		public string TargetUrl { get; set; }

		public FbMediaType MediaType { get; set; }
	}
}
