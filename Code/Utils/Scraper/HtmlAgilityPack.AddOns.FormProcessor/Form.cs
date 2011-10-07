using System;
using System.Text;
using HtmlAgilityPack;

namespace HtmlAgilityPack.AddOns.FormProcessor
{
    /// <summary>
    /// Enumeration values to specify how the form elements are related to the
    /// form tag.
    /// </summary>
    public enum FormQueryModeEnum : short
    {
        Nested,
        Adjacent
    }
    /// <summary>
    /// Encapsulates an HTML FORM tag. Stores a reference to the FORM's HtmlNode
    /// instance and a related collection of FORM elements in an instnace of
    /// HtmlNodeCollection.
    /// </summary>
    public class Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Form"/> class.
        /// </summary>
        /// <param name="formNode">The form node.</param>
        /// <param name="elementNodes">The element nodes.</param>
        public Form(HtmlNode formNode,
            HtmlNodeCollection elementNodes)
        {
            _form = formNode;
            _elements = elementNodes;
        }

        /// <summary>
        /// Gets the URL for action attribute of the form.
        /// </summary>
        /// <value>The action.</value>
        public string action
        {
            get
            {
                return this._form.GetAttributeValue("action", "");
            }
        }

        private HtmlNodeCollection _elements;


        /// <summary>
        /// Gets the elements.
        /// </summary>
        /// <value>The elements.</value>
        public HtmlNodeCollection elements
        {
            get { return _elements; }
        }

        private HtmlNode _form;

        /// <summary>
        /// Gets the form.
        /// </summary>
        /// <value>The form.</value>
        public HtmlNode form
        {
            get { return _form; }
        }

        /// <summary>
        /// Gets the <see cref="T:HtmlNode"/> with the specified name.
        /// </summary>
        /// <value>The HtmlNode with the specified name.</value>
        public HtmlNode this[string name]
        {
            get
            {
                foreach (HtmlNode node in
                      _elements)
                {
                    if (node.Attributes["name"] != null &&
                          node.Attributes["name"].Value == name)
                        return node;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the <see cref="T:HtmlNode"/> at the specified index.
        /// </summary>
        /// <value>The HtmlNode at the specified index.</value>
        public HtmlNode this[int index]
        {
            get
            {
                return _elements[index];
            }
        }
    }
}
