using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi.Logging
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
    public class TraceInfo
    {
        [DataMember(EmitDefaultValue=false, Order = 1)]
        public string ViewName { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 2)]
        public int Elapsed { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 3)]
        public List<LogInfo> Logs { get; set; }

        public TraceInfo()
        {
            Logs = new List<LogInfo>();
        }
    }
}