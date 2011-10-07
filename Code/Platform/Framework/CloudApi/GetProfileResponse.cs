using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public class GetProfileResponse
	{
		[DataMember(EmitDefaultValue = false, Order = 1)]
		public string Name { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 2)]
		public string AvatarUrl { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 3)]
		public string Location { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 4)]
		public List<Education> Education { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 5)]
		public LastStatus LastStatus { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 6)]
		public List<Occupation> ProfessionalHistory { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 7)]
		public List<Service> Services { get; set; }

		[DataMember(EmitDefaultValue = false, Order = 8)]
		public List<Sender> EmailAddresses { get; set; }
	}
}