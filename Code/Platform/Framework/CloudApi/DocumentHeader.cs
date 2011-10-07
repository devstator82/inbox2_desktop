using System;
using System.Runtime.Serialization;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Framework.CloudApi.Enumerations;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi
{
	[DataContract(Namespace = Settings.DefaultNamespace, Name = "Document")]
	public class DocumentHeader
	{
		[DataMember(EmitDefaultValue = false, Order = 1)]
		public long Changed { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 2)]
		public ModifyAction Action { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 3)]
		public string EntityKey { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 4)]
		public string Filename { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 5)]
		public int ContentType { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 6)]
		public string ContentId { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 7)]
		public long Size { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 8)]
		public Content Content { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 9)]
		public Content Preview { get; set; }

		public DateTime ChangedDate
		{
			get
			{
				return Changed.ToUnixTime();
			}
		}
	}
}
