// HtmlAgilityPack V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>
using System;

namespace HtmlAgilityPack
{
    /// <summary>
    /// Represents a fragment of code in a mixed code document.
    /// </summary>
    public class MixedCodeDocumentCodeFragment : MixedCodeDocumentFragment
    {
        internal string _code;

        internal MixedCodeDocumentCodeFragment(MixedCodeDocument doc)
            :
            base(doc, MixedCodeDocumentFragmentType.Code)
        {
        }

        /// <summary>
        /// Gets the fragment code text.
        /// </summary>
        public string Code
        {
            get
            {
                if (_code == null)
                {
                    _code = FragmentText.Substring(_doc.TokenCodeStart.Length,
                        FragmentText.Length - _doc.TokenCodeEnd.Length - _doc.TokenCodeStart.Length - 1).Trim();
                    if (_code.StartsWith("="))
                    {
                        _code = _doc.TokenResponseWrite + _code.Substring(1, _code.Length - 1);
                    }
                }
                return _code;
            }
            set
            {
                _code = value;
            }
        }
    }
}
