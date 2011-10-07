using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public class GetAccountResponse
	{
		[DataMember(EmitDefaultValue = false, Order = 1)]
		public string Username { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 2)]
		public string DisplayName { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 3)]
		public string Avatar { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 4)]
		public string Signature { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 5)]
		public string ConfigHash { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 6)]
		public List<Setting> UserSettings { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 7)]
		public List<ChannelHeader> Channels { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 8)]
		public List<ChannelSearch> Search { get; set; }		

		public GetAccountResponse()
		{
			UserSettings = new List<Setting>();
			Channels = new List<ChannelHeader>();
			Search = new List<ChannelSearch>();
		}
	}
}
