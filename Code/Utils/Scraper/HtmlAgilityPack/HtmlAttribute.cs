// HtmlAgilityPack V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>
using System;
using System.Collections;

namespace HtmlAgilityPack
{
	/// <summary>
	/// Represents an HTML attribute.
	/// </summary>
	public class HtmlAttribute: IComparable
	{
		internal int _line = 0;
		internal int _lineposition = 0;
		internal int _streamposition = 0;
		internal int _namestartindex = 0;
		internal int _namelength = 0;
		internal int _valuestartindex = 0;
		internal int _valuelength = 0;
		internal HtmlDocument _ownerdocument; // attribute can exists without a node
		internal HtmlNode _ownernode;
		internal string _name;
		internal string _value;

		internal HtmlAttribute(HtmlDocument ownerdocument)
		{
			_ownerdocument = ownerdocument;
		}

		/// <summary>
		/// Creates a duplicate of this attribute.
		/// </summary>
		/// <returns>The cloned attribute.</returns>
		public HtmlAttribute Clone()
		{
			HtmlAttribute att = new HtmlAttribute(_ownerdocument);
			att.Name = Name;
			att.Value = Value;
			return att;
		}

		/// <summary>
		/// Compares the current instance with another attribute. Comparison is based on attributes' name.
		/// </summary>
		/// <param name="obj">An attribute to compare with this instance.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the names comparison.</returns>
		public int CompareTo(object obj)
		{
			HtmlAttribute att = obj as HtmlAttribute;
			if (att == null)
			{
				throw new ArgumentException("obj");
			}
			return Name.CompareTo(att.Name);
		}

		public string XmlName
		{
			get
			{
				return HtmlDocument.GetXmlName(Name);
			}
		}

		internal string XmlValue
		{
			get
			{
				return Value;
			}
		}

		/// <summary>
		/// Gets the qualified name of the attribute.
		/// </summary>
		public string Name
		{
			get
			{
				if (_name == null)
				{
					_name = _ownerdocument._text.Substring(_namestartindex, _namelength).ToLower();
				}
				return _name;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				_name = value.ToLower();
				if (_ownernode != null)
				{
					_ownernode._innerchanged = true;
					_ownernode._outerchanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the attribute.
		/// </summary>
		public string Value
		{
			get
			{
				if (_value == null)
				{
					_value = _ownerdocument._text.Substring(_valuestartindex, _valuelength);
				}
				return _value;
			}
			set
			{
				_value = value;
				if (_ownernode != null)
				{
					_ownernode._innerchanged = true;
					_ownernode._outerchanged = true;
				}
			}
		}

		/// <summary>
		/// Gets the line number of this attribute in the document.
		/// </summary>
		public int Line
		{
			get
			{
				return _line;
			}
		}

		/// <summary>
		/// Gets the column number of this attribute in the document.
		/// </summary>
		public int LinePosition
		{
			get
			{
				return _lineposition;
			}
		}

		/// <summary>
		/// Gets the stream position of this attribute in the document, relative to the start of the document.
		/// </summary>
		public int StreamPosition
		{
			get
			{
				return _streamposition;
			}
		}

		/// <summary>
		/// Gets the HTML node to which this attribute belongs.
		/// </summary>
		public HtmlNode OwnerNode
		{
			get
			{
				return _ownernode;
			}
		}

		/// <summary>
		/// Gets the HTML document to which this attribute belongs.
		/// </summary>
		public HtmlDocument OwnerDocument
		{
			get
			{
				return _ownerdocument;
			}
		}

	}


}
