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
	[DataContract(Namespace = Settings.DefaultNamespace, Name = "Profile")]
	public class ProfileHeader
	{
		[DataMember(EmitDefaultValue = false, Order = 1)]
		public long Changed { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 2)]
		public ModifyAction Action { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 3)]
		public string EntityKey { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 4)]
		public string PersonKey { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 5)]
		public string ChannelKey { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 6)]
		public string ScreenName { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 7)]
		public string Address { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 8)]
		public string Location { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 9)]
		public string GeoLocation { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 10)]
		public string CompanyName { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 11)]
		public string Title { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 12)]
		public string Street { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 13)]
		public string HouseNumber { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 14)]
		public string ZipCode { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 15)]
		public string City { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 16)]
		public string State { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 17)]
		public string Country { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 18)]
		public string CountryCode { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 19)]
		public string PhoneNr { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 20)]
		public string MobileNr { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 21)]
		public string FaxNr { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 22)]
		public string AvatarSmall { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 23)]
		public string AvatarNormal { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 24)]
		public string AvatarLarge { get; set; }

		public DateTime ChangedDate
		{
			get
			{
				return Changed.ToUnixTime();
			}
		}
	}
}
