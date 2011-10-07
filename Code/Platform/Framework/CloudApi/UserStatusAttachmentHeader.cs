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
	[DataContract(Namespace = Settings.DefaultNamespace, Name = "Attachment")]
	public class UserStatusAttachmentHeader
	{
		[DataMember(EmitDefaultValue = false, Order = 1)]
		public long Changed { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 2)]
		public ModifyAction Action { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 3)]
		public string EntityKey { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 4)]
		public string PreviewImageUrl { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 5)]
		public string PreviewAltText { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 6)]
		public string TargetUrl { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 7)]
		public short MediaType { get; set; }

		public DateTime ChangedDate
		{
			get
			{
				return Changed.ToUnixTime();
			}
		}
	}
}
