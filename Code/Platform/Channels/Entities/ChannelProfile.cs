using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Inbox2.Platform.Channels.Attributes;

namespace Inbox2.Platform.Channels.Entities
{
	[DataContract]
	public class ChannelProfile
	{
		[DataMember(Order = 1)]
		public string ChannelProfileKey { get; set; }

		[DataMember(Order = 2)]
		public long SourceChannelId { get; set; }

		[DataMember(Order = 3)]
		public ProfileType ProfileType { get; set; }

		[DataMember(Order = 4)]
		public string ScreenName { get; set; }

		[DataMember(Order = 5)]
		public string Url { get; set; }

		[DataMember(Order = 6)]
		public SourceAddress SourceAddress { get; set; }

		[DataMember(Order = 7)]
		public string Location { get; set; }

		[DataMember(Order = 8)]
		public string GeoLocation { get; set; }

		[DataMember(Order = 9)]
		public string CompanyName { get; set; }

		[DataMember(Order = 10)]
		public string Title { get; set; }

		[DataMember(Order = 11)]
		public string Street { get; set; }

		[DataMember(Order = 12)]
		public string HouseNumber { get; set; }

		[DataMember(Order = 13)]
		public string ZipCode { get; set; }

		[DataMember(Order = 14)]
		public string City { get; set; }

		[DataMember(Order = 15)]
		public string State { get; set; }

		[DataMember(Order = 16)]
		public string Country { get; set; }

		[DataMember(Order = 17)]
		public string CountryCode { get; set; }

		[DataMember(Order = 18)]
		public string PhoneNr { get; set; }

		[DataMember(Order = 19)]
		public string MobileNr { get; set; }

		[DataMember(Order = 20)]
		public string FaxNr { get; set; }

		/// <summary>
		/// Only used inside the channels infrastructure. Don't depend on this in the UI or caching.
		/// </summary>
		[DataMember(Order = 21)]
		public ChannelAvatar ChannelAvatar { get; set; }
	}
}