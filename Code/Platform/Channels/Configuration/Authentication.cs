using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Inbox2.Platform.Channels.Configuration
{
	[Serializable]
	public class Authentication
	{
		[XmlIgnore]
		public Type Type
		{
			get { return Type.GetType(TypeSurrogate); }
			set
			{
				if (value != null)
					TypeSurrogate = value.AssemblyQualifiedName;
			}
		}

		/// <summary>
		/// Helper property to fix serialization issues with the Type property.
		/// </summary>
		public string TypeSurrogate { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }

		public Authentication()
		{
			// NetDataContract serializer chokes over null strings, so we assign an empty string instead
			TypeSurrogate = String.Empty;
		}
	}
}