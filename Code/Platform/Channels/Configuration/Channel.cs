using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Inbox2.Platform.Channels.Configuration
{
	[Serializable]
	public class Channel
	{
		[XmlIgnore]
		public Type Type
		{
			get { return Type.GetType(TypeSurrogate); }
			set { TypeSurrogate = value.AssemblyQualifiedName; }
		}

		/// <summary>
		/// Helper property to fix serialization issues with the Type property.
		/// </summary>
		public string TypeSurrogate { get; set; }

		public string Hostname { get; set; }

		public int Port { get; set; }

		public bool IsSecured { get; set; }

		public int MaxConcurrentConnections { get; set; }

		public Authentication Authentication { get; set; }

		public Channel()
		{
			Authentication = new Authentication();
		}
	}
}