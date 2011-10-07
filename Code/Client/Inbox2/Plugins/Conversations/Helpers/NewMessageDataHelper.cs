using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Inbox2.Framework.Plugins.SharedControls;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Text;

namespace Inbox2.Plugins.Conversations.Helpers
{
	[DataContract]
	public class NewMessageDataHelper
	{
		[DataMember]
		public long? SourceMessageId { get; set; }

		[DataMember]
		public SourceAddressCollection To { get; set; }

		[DataMember]
		public SourceAddressCollection Cc { get; set; }

		[DataMember]
		public SourceAddressCollection Bcc { get; set; }

		[DataMember]
		public string Context { get; set; }

		[DataMember]
		public string Body { get; set; }

		[DataMember]
		public long SelectedChannelId { get; set; }

		[DataMember]
		public List<AttachmentDataHelper> AttachedFiles { get; set; }

		[DataMember]
		public bool SuppressSignature { get; set; }

		public NewMessageDataHelper()
		{
			AttachedFiles = new List<AttachmentDataHelper>();
		}

		public static NewMessageDataHelper Parse(string mailtoString)
		{
			var uri = new Uri(mailtoString);

			var to = new SourceAddress(String.Format("{0}@{1}", uri.UserInfo, uri.DnsSafeHost));
			var cc = new SourceAddressCollection();
			var bcc = new SourceAddressCollection();
			var subject = String.Empty;
			var body = String.Empty;

			// See if there is a subject embedded in the url
			if (uri.Query.Length > 0)
			{
				var parts = NameValueParser.GetCollection(uri.Query, "?", "&");

				if (parts["subject"] != null)
					subject = parts["subject"];

				if (parts["body"] != null)
					body = parts["body"];

				if (parts["cc"] != null)
					cc = new SourceAddressCollection(parts["cc"]);

				if (parts["bcc"] != null)
					bcc = new SourceAddressCollection(parts["bcc"]);
			}

			return new NewMessageDataHelper { Context = subject, To = to.ToList(), Cc = cc, Bcc = bcc, Body = body };
		}

		public override bool Equals(object obj)
		{
			var other = obj as NewMessageDataHelper;

			if (other == null)
				return false;

			if (!SourceMessageId.HasValue)
				return false;

			if (!other.SourceMessageId.HasValue)
				return false;

			return SourceMessageId.Value.Equals(other.SourceMessageId.Value);
		}

		public override int GetHashCode()
		{
			return SourceMessageId.GetHashCode();
		}
	}	
}