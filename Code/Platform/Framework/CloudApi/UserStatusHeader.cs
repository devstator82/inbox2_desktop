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
	[DataContract(Namespace = Settings.DefaultNamespace, Name = "StatusUpdate")]
	public class UserStatusHeader
	{
		[DataMember(EmitDefaultValue = false, Order = 1)]
		public long Changed { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 2)]
		public ModifyAction Action { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 3)]
		public string EntityKey { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 4)]
		public string SourceChannelKey { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 5)]
		public string TargetChannelKey { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 6)]
		public string SearchKeyword { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 7)]
		public string ChannelStatusKey { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 8)]
		public Sender Sender { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 9)]
		public string To { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 10)]
		public string Status { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 11)]
		public string InReplyTo { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 12)]
		public short StatusType { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 13)]
		public long SortDate { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 14)]
		public List<UserStatusHeader> Children { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 15)]
		public List<UserStatusAttachmentHeader> Attachments { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 16)]
		public bool IsRead { get; set; }

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