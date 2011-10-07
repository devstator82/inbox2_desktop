// HtmlAgilityPack V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>
using System;

namespace HtmlAgilityPack
{
    /// <summary>
    /// Represents a fragment of text in a mixed code document.
    /// </summary>
    public class MixedCodeDocumentTextFragment : MixedCodeDocumentFragment
    {
        internal MixedCodeDocumentTextFragment(MixedCodeDocument doc)
            :
            base(doc, MixedCodeDocumentFragmentType.Text)
        {
        }

        /// <summary>
        /// Gets the fragment text.
        /// </summary>
        public string Text
        {
            get
            {
                return FragmentText;
            }
            set
            {
                base._fragmenttext = value;
            }
        }
    }
}
