using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Inbox2.Platform.Framework.Configuration
{
	/// <summary>
	/// Configuration section handler that deserializes configuration settings to an object.
	/// </summary>
	/// <remarks>The root node must have a type attribute defining the type to deserialize to.</remarks>
	public class XmlSerializerSectionHandler : IConfigurationSectionHandler
	{
		public object Create(object parent, object configContext, System.Xml.XmlNode section)
		{
			// get the name of the type from the type= attribute on the root node
			XPathNavigator xpn = section.CreateNavigator();
			string TypeName = (string)xpn.Evaluate("string(@type)");
			if (TypeName == "")
			{
				throw new ConfigurationErrorsException(
					"The type attribute is not present on the root node of the <" +
					section.Name + "> configuration section ", section);
			}

			// make sure this string evaluates to a valid type
			Type t = Type.GetType(TypeName);
			if (t == null)
			{
				throw new ConfigurationErrorsException(
					"The type attribute \'" + TypeName + "\' specified in the root node of the " +
					"the <" + section.Name + "> configuration section is not a valid type.",
					section);
			}
			XmlSerializer xs = new XmlSerializer(t);

			// attempt to deserialize an object of this type from the provided XML section
			XmlNodeReader xnr = new XmlNodeReader(section);
			try
			{
				return xs.Deserialize(xnr);
			}
			catch (Exception ex)
			{
				string s = ex.Message;
				Exception iex = ex.InnerException;
				while (iex != null)
				{
					s += "; " + iex.Message;
					iex = iex.InnerException;
				}
				throw new ConfigurationErrorsException(
					"Unable to deserialize an object of type \'" + TypeName +
					"\' from  the <" + section.Name + "> configuration section: " +
					s, ex, section);
			}
		}
	}
}
