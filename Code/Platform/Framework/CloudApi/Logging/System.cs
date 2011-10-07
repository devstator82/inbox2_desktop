using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi.Logging
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
    public class System
    {
        [DataMember(EmitDefaultValue=false, Order = 1)]
        public Size Resolution { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 2)]
        public int Monitors { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 3)]
        public string OperatingSystem { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 4)]
        public string Cpu { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 5)]
        public int CpuCount { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 6)]
        public string CpuArchitecture { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 7)]
        public ulong Memory { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 8)]
        public string BrowserVersion { get; set; }
    }
}
