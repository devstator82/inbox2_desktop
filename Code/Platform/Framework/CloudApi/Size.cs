using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Inbox2.Platform.Framework.ServiceModel.DataContracts;

namespace Inbox2.Platform.Framework.CloudApi
{
	[DataContract(Namespace = Settings.DefaultNamespace)]
    public struct Size
    {
        [DataMember(EmitDefaultValue=false, Order = 1)]
        public double Width;

        [DataMember(EmitDefaultValue=false, Order = 2)]
        public double Height;

        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }
    }
}
