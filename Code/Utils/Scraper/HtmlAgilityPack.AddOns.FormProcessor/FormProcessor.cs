// FormProcessor V0.1 - Josh Gough http://www.ultravioletconsulting.com
// This code references Simon Mourier's HtmlAgilityPack http://smourier.blogspot.com
using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;
using HtmlAgilityPack;

namespace HtmlAgilityPack.AddOns.FormProcessor 
{
    /// <summary>
    /// <para>
    /// Builds on top of HtmlAgilityPack to allow for modifying HTML FORMS
    /// returned from HTML pages. Allows for modification of and submission
    /// of the form values using standard application/x-www-urlencoded POST
    /// operations.
    /// </para>
    /// <para>
    /// Allows you to modify the form using familiar JavaScript-like indexer
    /// syntax. For example: 
    /// </para>
    /// <code>
    /// form["elementName"].SettAttributeValue("value", someValue);
    /// </code>
    /// <para>Or:</para>
    /// <code>
    /// form.elements["elementName"].SetAttributeValue("value", someValue);
    /// </code>
    /// </summary>
    /// <example>
    /// Suppose you have several accounts with balances that you
    /// can view online. Unfortunately, your various vendors or
    /// service providers do not offer you a programmatic way
    /// to view this data on-demand, at least not without paying.
    /// 
    /// If you use VerizonWireless, you can use this code to find out
    /// your current balance as of January 2006:
    /// 
    /// <code> 
    /// using HtmlAgilityPack;
    /// using HtmlAgilityPack.AddOns.FormProcessor;
    /// 
    /// ... other stuff here ...
    /// 
    /// try 
    /// {
    ///         string uname = "Replace with user name";
    ///         string pwd = "Replace with password";
    ///         Form form = p.GetForm(
    ///             "https://myaccount.verizonwireless.com/vzs/loginform",
    ///             "//form[@name='loginForm']", FormQueryModeEnum.Nested);
    ///
    ///         form["j_username"].SetAttributeValue("value", userName);
    ///         form["j_password"].SetAttributeValue("value", password);
    ///
    ///         HtmlDocument doc = p.SubmitForm(form);
    ///
    ///         string strBal = doc.DocumentNode.SelectSingleNode
    ///             ("//span[@class='redText']").InnerText;
    ///
    ///         strBal = System.Web.HttpUtility.HtmlDecode(strBal);
    ///         strBal = strBal.Substring(1).Trim();
    /// } ... catch exceptions ...
    /// </code>
    /// This is the flow of events in the code:
    /// <remarks>
    /// <list>
    /// <item>Set username and password values.</item>
    /// <item>Create an instance of the form processor.</item>
    /// <item>Load the login page for VerizonWireless.</item>
    /// <item>Assign the username and password values to 
    /// the form using indexer syntax.</item>
    /// <item>Submit the form and get the result.</item>
    /// <item>Use XPath syntax to extract the data from the HTML markup.</item>
    /// </list>
    /// Until such time that your provider makes their data available as
    /// true data, perhaps as XML, this class can help you translate markup
    /// into data for practical usage.
    /// </remarks>
    /// </example>    
    public class FormProcessor
    {
        // Class properties
        private static AttributeReferenceAbsolutizer _formAbsolutizer =
            new AttributeReferenceAbsolutizer("");

        /// <summary>
        /// Static class constructor configures the AttributeSelectStatement
        /// used to absolutize a FORM's action attribute.
        /// </summary>
        static FormProcessor()
        {
            // Configure the static instance to only look for the action attribute
            _formAbsolutizer.AttributesSelectStatement = "@action";
        }
        /// <summary>
        /// Absolutizes only the "action" attribute of a FORM node.
        /// </summary>
        /// <param name="startNode">The start node.</param>
        /// <param name="baseUrl">The base URL.</param>
        public static void AbsolutizeForm(HtmlNode startNode, string baseUrl)
        {
            // Set the base URL for this call:
            _formAbsolutizer.BaseURL = baseUrl;
            _formAbsolutizer.ExecuteAbsolutization(startNode);
        }

        private HtmlWeb _web;
        /// <summary>
        /// The HtmlWeb instance used to request content from URLs.
        /// </summary>
        public HtmlWeb Web
        {
            get { return _web; }
            set { _web = value; }
        }

        private string  _nodeSelectPath = "//input | //select | //textarea | //button";

        /// <summary>
        /// The XPath used to select the FORM elements. Defaults to input, select, textarea,
        /// and button.
        /// </summary>
        /// <value>The node select path.</value>
        public string  NodeSelectPath
        {
            get { return _nodeSelectPath; }
            set { _nodeSelectPath = value; }
        }

        private HtmlNodeCollection _formNodes = null;

        /// <summary>
        /// Default constructor initializes the Web property with a new
        /// instance of FormProcessorWeb.
        /// </summary>
        public FormProcessor()
        { 
            FormProcessorWeb web = new FormProcessorWeb(true);
            this.Init(web);
        }

        /// <summary>
        /// Constructor initalizes the Web property with the specified
        /// HtmlWeb instance.
        /// </summary>
        /// <param name="web">The web.</param>
        public FormProcessor(HtmlWeb web)
        {
            this.Init(web);
        }

        /// <summary>
        /// Initializes the FormProcessor instance with the specified
        /// HtmlWeb object and also removes the "form" element
        /// from the HtmlAgilityPack.HtmlNode.ElementsFlags to enable
        /// FORM to be a parent node of other nodes.
        /// </summary>
        /// <param name="web">The HtmlWeb instance.</param>
        private void Init(HtmlWeb web)
        {
            _web = web;
            HtmlAgilityPack.HtmlNode.ElementsFlags.
                Remove("form");
        }

        /// <summary>
        /// Attempts to extract, parse, and return an HTMLFormElement object
        /// from the content contained in the doc object, using the specified
        /// XPath statement and queryMode.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="url">The URL.</param>
        /// <param name="xpath">The xpath.</param>
        /// <param name="queryMode">The query mode.</param>
        /// <returns></returns>
        public Form GetForm(HtmlDocument doc, string url, string xpath, // # FIX THIS FUNCTION
            FormQueryModeEnum queryMode)
        {
            HtmlNode formNode
                = doc.DocumentNode.SelectSingleNode(xpath);
            
			if (formNode == null)
                return null;

            HtmlNodeCollection formNodes = null;
            Form form = null;

            // Absolutize the form's action attribute
            AbsolutizeForm(formNode, url);

            if (formNode != null) // TODO : fix this
            {                
                // If queryMode is nested then just apply the path from
                // the point of the containing element         
                if (queryMode == FormQueryModeEnum.Nested)
                {
                    formNodes = formNode.SelectNodes(NodeSelectPath);                    
                }
                else if (queryMode == FormQueryModeEnum.Adjacent)
                {
                    // Otherwise, the form is not properly the parent of 
                    // all its nodes, so grab all child nodes of the entire document
                    // *TODO* make this take into account the node position
                    formNodes = doc.DocumentNode.SelectNodes(NodeSelectPath);
                    // Add these nodes to the form element                    
                }

                if (formNodes != null)
                {
                    form = new Form(formNode, formNodes);
                }
            }
            return form; // could be null!
        }

        /// <summary>
        /// Attempts to download, extract, parse, and return a Form instance
        /// from the content found at the given url, using the specified
        /// XPath statement and queryMode.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="xpath">The xpath.</param>
        /// <param name="queryMode">The query mode.</param>
        /// <returns></returns>
        public Form GetForm(string url, string xpath, 
            FormQueryModeEnum queryMode)
        {
            HtmlDocument doc = _web.Load(url);
            return GetForm(doc, url, xpath, queryMode);            
        }

        /// <summary>
        /// Posts the form back to the server with a payload
        /// assembled from the specified HtmlNodeCollection collection.
        /// </summary>
        /// <param name="formNodes">The form nodes.</param>
        /// <param name="actionURL">The action URL.</param>
        /// <returns></returns>
        public HtmlDocument SubmitForm(HtmlNodeCollection formNodes, string actionURL)
        {
            // Assign the current form nodes
            _formNodes = formNodes;
            
            // Assemble to POST in application/x-www-form-urlencoded fashion
            _web.PreRequest += new HtmlWeb.PreRequestHandler(preRequestHandler);
            HtmlDocument doc = _web.Load(actionURL, "post");
            AbsolutizeForm(doc.DocumentNode, actionURL);
            _web.PreRequest -= preRequestHandler;

            // Unassign the current form nodes
            _formNodes = null;

            return doc;
        }

        /// <summary>
        /// Posts the form back to the server with a payload
        /// assembled from the specified Form object's HtmlNodeCollection.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns></returns>
        public HtmlDocument SubmitForm(Form form)
        {
            return SubmitForm(form.elements, form.action);
        }

        /// <summary>
        /// Writes the payload for the POST operation to the outbound
        /// HttpWebRequest.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        private bool preRequestHandler(System.Net.HttpWebRequest target)
        {
            string payload = AssemblePostBackPayload();
            byte[] buff = Encoding.ASCII.GetBytes(payload.ToCharArray());
            target.ContentLength = buff.Length;
            target.ContentType = "application/x-www-form-urlencoded";
            System.IO.Stream reqStream = target.GetRequestStream();
            reqStream.Write(buff, 0, buff.Length);
            return true;
        }

        /// <summary>
        /// Loops through the current form elements collection and assembles
        /// a payload string to be used during postback to the server.
        /// </summary>
        /// <returns>string that represents the payload to send back to the
        /// server.</returns>        
        public string AssemblePostBackPayload()
        {
            // To use as a buffer
            StringBuilder sb = new StringBuilder();
            // Loop through the nodes and assemble the string
            foreach (HtmlNode nodeIter in _formNodes)
            {                
                HtmlNode node = nodeIter;

                if (node.Attributes["name"] != null)
                {
                    if (node.Name == "input")
                    {
                        if (node.Attributes["type"] != null
                            &&
                            (
                                node.Attributes["type"].Value == "checkbox" 
                                ||
                                node.Attributes["type"].Value == "radio"
                            )
                            && 
                            node.Attributes["checked"] != null)
                        {
							sb.Append("&" + node.Attributes["name"].Value +
									  "=" + HttpUtility.UrlEncode(node.Attributes["checked"].Value));
                        }
                        else
                        {
                            if (node.Attributes["name"] != null)
                            {
                                if (node.Attributes["value"] != null)
                                {
									sb.Append("&" + node.Attributes["name"].Value);
                                    sb.Append("=" + HttpUtility.UrlEncode(node.Attributes["value"].Value));
                                }
                            }
                        }
                    }
                    else if (node.Name == "textarea")
                    {
                        sb.Append("&" + node.Attributes["name"].Value +
                            "=" + HttpUtility.UrlEncode(node.InnerHtml));
                    }
                    else if (node.Name == "select")
                    {
                        HtmlNodeCollection options = node.SelectNodes("//option[@selected]");
                        if (options != null)
                        {
                            foreach (HtmlNode option in options)
                            {
                                sb.Append("&" + node.Attributes["name"].Value +
                                    "=" + HttpUtility.UrlEncode(option.Attributes["value"].Value));
                            }
                        }
                    }
                }
            }
            string strBuff = sb.ToString();
            if (strBuff.Length > 1)
                return strBuff.Substring(1);
            else
                return "";
        }
    }
}

/* FOR XSLT
// Look for a replacement value from the overlaid collection
if (_formReplacementNodes != null)
{
    foreach (HtmlNode repNode in _formReplacementNodes)
    {
        if (repNode.Attributes["name"] != null &&
            repNode.Attributes["name"].Value.ToLower() ==
            node.Attributes["name"].Value.ToLower())
        {
            node = repNode;
        }
    }
}
*/
