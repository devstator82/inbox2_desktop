using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Inbox2.Platform.Channels.Attributes;
using Inbox2.Platform.Channels.Text;

namespace Inbox2.Platform.Channels.Entities
{
	[DataContract]
	public class ChannelPerson
	{
		public string Name
		{
			get
			{
				return String.Format("{0} {1}", Firstname, Lastname).Trim();
			}
			set
			{
				var res = PersonName.Parse(value);

				Firstname = res.Firstname;
				Lastname = res.Lastname;
			}
		}

		[DataMember(Order = 1)]
		public long SourceChannelId { get; set; }

		[DataMember(Order = 2)]
		public string Firstname { get; set; }

		[DataMember(Order = 3)]
		public string Lastname { get; set; }

		[DataMember(Order = 4)]
		public DateTime? DateOfBirth { get; set; }

		[DataMember(Order = 5)]
		public string Locale { get; set; }

		[DataMember(Order = 6)]
		public string Gender { get; set; }

		[DataMember(Order = 7)]
		public string Timezone { get; set; }
	}
}