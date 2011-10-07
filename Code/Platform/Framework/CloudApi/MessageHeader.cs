using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Framework.CloudApi.Enumerations;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Platform.Framework.CloudApi
{
	[DataContract(Namespace = Settings.DefaultNamespace, Name = "Message")]
	public class MessageHeader
	{
		[DataMember(EmitDefaultValue = false, Order = 1)]
		public long Changed { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 2)]
		public ModifyAction Action { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 3)]
		public string EntityKey { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 4)]
		public string Subject { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 5)]
		public int MessageFolder { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 6)]
		public Sender Sender { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 7)]
		public string To { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 8)]
        public string ReturnTo { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 9)]
		public string CC { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 10)]
		public string BCC { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 11)]
		public string SourceChannelKey { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 12)]
		public string TargetChannelKey { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 13)]
        public string InReplyTo { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 14)]
		public bool IsRead { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 15)]
		public bool IsStarred { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 16)]
		public bool IsArchived { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 17)]
		public string Labels { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 18)]
		public short ShareState { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 19)]
		public Content ContentHtml { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 20)]
		public Content ContentText { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 21)]
		public long SortDate { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 22)]
		public List<DocumentHeader> Attachments { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 23)]
		public ConversationHeader Conversation { get; set; }

		public DateTime ChangedDate
		{
			get
			{
				return Changed.ToUnixTime();
			}
		}

		public DateTime SortDateDate
		{
			get
			{
				return SortDate.ToUnixTime();
			}
		}
	}
}