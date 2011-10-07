using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi.Logging
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
    public class ClientInfo
    {        
        [DataMember(EmitDefaultValue=false, Order = 1)]
        public string ClientId { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 2)]
        public string ClientVersion { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 3)]
        public DateTime DateInstalled { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 4)]
        public Size ClientSize { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 5)]
        public int ClientWndowState { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 6)]
        public int TotalChannels { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 7)]
        public int SocialChannels { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 8)]
        public int SearchChannels { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 9)]
        public bool IsSingleLineView { get; set; }        

        [DataMember(EmitDefaultValue=false, Order = 10)]
        public double FoldersWidth { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 11)]
        public double RealtimeColumnWidth { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 12)]
        public double PreviewPaneWidth { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 13)]
        public double PreviewPaneHeight { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 14)]
        public short PreviewPaneLocation { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 15)]
        public int Labels { get; set; }
        
        [DataMember(EmitDefaultValue=false, Order = 16)]
        public int Messages { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 17)]
        public int Documents { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 18)]
        public int Persons { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 19)]
        public int Profiles { get; set; }

        [DataMember(EmitDefaultValue=false, Order = 20)]
        public System System { get; set; }

        public ClientInfo()
        {
            System = new System();
        }
    }
}
