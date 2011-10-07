using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Inbox2.Platform.Framework.ServiceModel.Extensions
{
	/// <summary>
	/// Based on code from: http://blogs.msdn.com/kaevans/archive/2004/08/02/206432.aspx
	/// Our implementation simply removes the nil="true" attribute on nulled types.
	/// </summary>
	public class XmlManglingWriter : XmlTextWriter
	{
		protected bool skipAttribute = false;
		
		public XmlManglingWriter(System.IO.TextWriter writer)
			: base(writer)
		{
		}

		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			base.WriteStartElement(null, localName, null);
		}

		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			//If the prefix or localname are "xmlns", don't write it.
			if (prefix.CompareTo("xmlns") == 0 || localName.CompareTo("xmlns") == 0
				|| prefix.CompareTo("nil") == 0 || localName.CompareTo("nil") == 0)			
			{
				skipAttribute = true;
			}
			else
			{
				base.WriteStartAttribute(null, localName, null);
			}
		}

		public override void WriteString(string text)
		{
			//If we are writing an attribute, the text for the xmlns
			//or xmlns:prefix declaration would occur here.  Skip
			//it if this is the case.
			if (!skipAttribute)
			{
				base.WriteString(text);
			}
		}

		public override void WriteEndAttribute()
		{
			//If we skipped the WriteStartAttribute call, we have to
			//skip the WriteEndAttribute call as well or else the XmlWriter
			//will have an invalid state.
			if (!skipAttribute)
			{
				base.WriteEndAttribute();
			}
			//reset the boolean for the next attribute.
			skipAttribute = false;
		}


		public override void WriteQualifiedName(string localName, string ns)
		{
			//Always write the qualified name using only the
			//localname.
			base.WriteQualifiedName(localName, null);
		}
	}
}
