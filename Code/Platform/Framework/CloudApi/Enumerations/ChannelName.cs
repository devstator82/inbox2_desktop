using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi.Enumerations
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public enum ChannelName
	{
		[EnumMember(Value = "Unknown")]
		Unknown,

		[EnumMember(Value = "GMail")]
		GMail,

		[EnumMember(Value = "Hotmail")]
		Hotmail,

		[EnumMember(Value = "Yahoo")]
		Yahoo,

		[EnumMember(Value = "AOL")]
		AOL,

		[EnumMember(Value = "Exchange")]
		Exchange,

		[EnumMember(Value = "Facebook")]
		Facebook,

		[EnumMember(Value = "Twitter")]
		Twitter,

		[EnumMember(Value = "LinkedIn")]
		LinkedIn,

		[EnumMember(Value = "Yammer")]
		Yammer,

		[EnumMember(Value = "Hyves")]
		Hyves,

		[EnumMember(Value = "Pop3")]
		Pop3,

		[EnumMember(Value = "Imap")]
		Imap,

		// Secondary sites maily from thirdparty data providers
		[EnumMember(Value = "Bebo")]
		Bebo,

		[EnumMember(Value = "Flickr")]
		Flickr,

		[EnumMember(Value = "Friendster")]
		Friendster,

		[EnumMember(Value = "Hi5")]
		Hi5,

		[EnumMember(Value = "LiveJournal")]
		LiveJournal,

		[EnumMember(Value = "Multiply")]
		Multiply,

		[EnumMember(Value = "MySpace")]
		MySpace,

		[EnumMember(Value = "MyYearBook")]
		MyYearBook,

		[EnumMember(Value = "Plaxo")]
		Plaxo,

		[EnumMember(Value = "Amazon")]
		Amazon,

		[EnumMember(Value = "Ecademy")]
		Ecademy,

		[EnumMember(Value = "Flixster")]
		Flixster,

		[EnumMember(Value = "Ringo")]
		Ringo,

		[EnumMember(Value = "Tickle")]
		Tickle,

		[EnumMember(Value = "Tribe")]
		Tribe,

		[EnumMember(Value = "Yelp")]
		Yelp,

		[EnumMember(Value = "Tagged")]
		Tagged,

		[EnumMember(Value = "Metroflog")]
		Metroflog,

		[EnumMember(Value = "Pandora")]
		Pandora,

		[EnumMember(Value = "PhotoBucket")]
		PhotoBucket,

		[EnumMember(Value = "FriendFeed")]
		FriendFeed,
	}
}
