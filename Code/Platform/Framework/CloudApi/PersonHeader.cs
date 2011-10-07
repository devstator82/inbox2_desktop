using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Framework.CloudApi.Enumerations;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi
{
	[DataContract(Namespace = Settings.DefaultNamespace, Name = "Person")]
	public class PersonHeader
	{
		[DataMember(EmitDefaultValue = false, Order = 1)]
		public long Changed { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 2)]
		public ModifyAction Action { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 3)]
		public string EntityKey { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 4)]
		public string Name { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 5)]
		public string Firstname { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 6)]
		public string Lastname { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 7)]
		public string AvatarUrl { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 8)]
		public int SocialScore { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 9)]
		public List<ProfileHeader> Profiles { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 10)]
		public List<Sender> EmailAddresses { get; set; }

		public DateTime ChangedDate
		{
			get
			{
				return Changed.ToUnixTime();
			}
		}
	}
}
