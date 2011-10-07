// HtmlAgilityPack V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>
using System;

namespace HtmlAgilityPack
{
    internal class NameValuePair
    {
        internal readonly string Name;
        internal string Value;

        internal NameValuePair()
        {
        }

        internal NameValuePair(string name)
            :
            this()
        {
            Name = name;
        }

        internal NameValuePair(string name, string value)
            :
            this(name)
        {
            Value = value;
        }
    }

}
