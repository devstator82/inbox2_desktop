using System;
using System.Text;
using HtmlAgilityPack;

namespace HtmlAgilityPack.AddOns.FormProcessor
{
    /// <summary>
    /// Absolutzes references in common attributes. The defaults are
    /// background, href, src, lowsrc, and action. These can be
    /// configured using the AttributsToAbsolutize array.
    /// Alternatively, the AttributesSelectStatement can be specified
    /// directly to set the statement. Several static methods also
    /// exist for easy access. 
    /// </summary>
    public class AttributeReferenceAbsolutizer
    {
        /// <summary>
        /// Convenience method to create a default instance of the class and absolutize the references
        /// within the startNode parameter.
        /// </summary>
        /// <param name="startNode">The start node.</param>
        /// <param name="baseUrl">The base URL.</param>   
        public static void ExecuteDefaultAbsolutization(HtmlNode startNode, string baseUrl)
        {
            AttributeReferenceAbsolutizer absolutizer =
                new AttributeReferenceAbsolutizer(baseUrl);
            absolutizer.ExecuteAbsolutization(startNode);
        }

        /// <summary>
        /// Gets the absolute URI.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static Uri GetAbsoluteUri(Uri baseUri, string path)
        {
            Uri uriTarget;
            if (Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out uriTarget))
            {
                // See what type of URI it is...
                if (uriTarget.IsAbsoluteUri)
                {
                    return uriTarget;
                }
                else
                {
                    // Since it's not absolute, see whether it is relative to
                    // the current directory, or points to the web server root
                    if (uriTarget.OriginalString[0] == '/')
                    {
                        return new Uri(new Uri("http://" + baseUri.Authority),
                            uriTarget.OriginalString.Substring(1));
                    }
                    else
                    {
                        return new Uri(GetBaseUri(baseUri), uriTarget);
                    }
                }
            }
            else
            {
                return baseUri;
            }
        }

        /// <summary>
        /// Gets the base URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static Uri GetBaseUri(Uri uri)
        {
            string pathToLastSubdir = uri.GetLeftPart(UriPartial.Path);
            pathToLastSubdir = pathToLastSubdir.Substring(0,
                pathToLastSubdir.LastIndexOf('/') + 1);
            return new Uri(pathToLastSubdir);
        }

        private string _baseUrl;

        /// <summary>
        /// The starting point URL. All relative references will be appended to 
        /// this URL when they are absolutized.
        /// </summary>
        public string BaseURL
        {
            get { return _baseUrl; }
            set { _baseUrl = value; }
        }

        private string[] _attributesToAbsolutize = new string[] 
        {
            "background", "href", "src", "lowsrc", "action"
        };

        /// <summary>
        /// The attribute names that should be absolutized.
        /// </summary>
        public string[] AttributesToAbsolutize
        {
            get { return _attributesToAbsolutize; }
            set { _attributesToAbsolutize = value; }
        }

        private string _attributesSelectStatement = null;
        /// <summary>
        /// The XPath statement used to select attributes. By default, this
        /// is constructed the first time the property is accessed by 
        /// assembling all the values of the AttributesToAbsolutize array.
        /// But, you can manually specify it as well. It should look like:
        /// //@attributeName | //@secondAttributeName
        /// </summary>
        public string AttributesSelectStatement
        {
            get
            {
                if (_attributesSelectStatement == null)
                {
                    StringBuilder attrSelectStatement = new StringBuilder();
                    foreach (string attr in this.AttributesToAbsolutize)
                    {
                        attrSelectStatement.Append(" | //@" + attr);
                    }
                    _attributesSelectStatement =
                        attrSelectStatement.ToString().Substring(3);
                }
                return _attributesSelectStatement;
            }
            set { _attributesSelectStatement = value; }
        }

        /// <summary>
        /// Constructor that configures the instance with the specified baseURL.
        /// </summary>
        /// <param name="baseURL"></param>
        public AttributeReferenceAbsolutizer(string baseURL)
        {
            this.BaseURL = baseURL;
        }

        /// <summary>
        /// Performs the absolutizing on the specified startNode object. It uses
        /// the AttributesSelectStatement property to extract the attributes from 
        /// startNode.
        /// </summary>
        /// <param name="startNode"></param>
        public void ExecuteAbsolutization(HtmlNode startNode)
        {
            HtmlNodeCollection attrNodes =
                startNode.SelectNodes(this.AttributesSelectStatement);
            if (attrNodes == null)
                return;

            foreach (HtmlNode node in attrNodes)
            {
                foreach (string attr in this.AttributesToAbsolutize)
                {
                    // Make sure the attribute exists in this node
                    HtmlAttribute att = node.Attributes[attr];
                    if (att != null && att.Value != null)
                    {
                        att.Value = GetAbsoluteUri(new Uri(this._baseUrl), att.Value).ToString();
                    }
                }
            }
        }
    }
}
