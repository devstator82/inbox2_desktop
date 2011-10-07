// HtmlAgilityPack V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>
using System;

namespace HtmlAgilityPack
{
    /// <summary>
    /// Represents a base class for fragments in a mixed code document.
    /// </summary>
    public abstract class MixedCodeDocumentFragment
    {
        internal MixedCodeDocumentFragmentType _type;
        internal MixedCodeDocument _doc;
        internal int _index;
        internal int _length;
        internal int _line;
        internal int _lineposition;
        internal string _fragmenttext;

        internal MixedCodeDocumentFragment(MixedCodeDocument doc, MixedCodeDocumentFragmentType type)
        {
            _doc = doc;
            _type = type;
            switch (type)
            {
                case MixedCodeDocumentFragmentType.Text:
                    _doc._textfragments.Append(this);
                    break;

                case MixedCodeDocumentFragmentType.Code:
                    _doc._codefragments.Append(this);
                    break;
            }
            _doc._fragments.Append(this);
        }

        /// <summary>
        /// Gets the type of fragment.
        /// </summary>
        public MixedCodeDocumentFragmentType FragmentType
        {
            get
            {
                return _type;
            }
        }

        /// <summary>
        /// Gets the fragment position in the document's stream.
        /// </summary>
        public int StreamPosition
        {
            get
            {
                return _index;
            }
        }

        /// <summary>
        /// Gets the line number of the fragment.
        /// </summary>
        public int Line
        {
            get
            {
                return _line;
            }
        }

        /// <summary>
        /// Gets the line position (column) of the fragment.
        /// </summary>
        public int LinePosition
        {
            get
            {
                return _lineposition;
            }
        }

        /// <summary>
        /// Gets the fragement text.
        /// </summary>
        public string FragmentText
        {
            get
            {
                if (_fragmenttext == null)
                {
                    _fragmenttext = _doc._text.Substring(_index, _length);
                }
                return _fragmenttext;
            }
        }
    }
}
