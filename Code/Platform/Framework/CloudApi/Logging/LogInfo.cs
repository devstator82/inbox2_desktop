using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi.Logging
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
	public class LogInfo
	{
		[DataMember(EmitDefaultValue=false, Order = 1)]
		public string EventName { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 2)]
		public double Time { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 3)]
		public string Segment { get; set; }

		[DataMember(EmitDefaultValue=false, Order = 4)]
		public long DateCreated { get; set; }

		public LogInfo(string eventName, double time)
		{
			EventName = eventName;
			Time = time;
			DateCreated = DateTime.Now.ToUnixTime();
		}

		public LogInfo(string eventName, string segment)
		{
			EventName = eventName;
			Segment = segment;
			DateCreated = DateTime.Now.ToUnixTime();
		}

		public LogInfo()
		{
		}
	}
}
