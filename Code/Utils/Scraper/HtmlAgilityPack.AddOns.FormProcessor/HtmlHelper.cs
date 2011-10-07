using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;

namespace HtmlAgilityPack.AddOns.FormProcessor
{
    /// <summary>
    /// Provides stand-alone utlity services for cleaning HTML or fetching documents and converting
    /// their content to XML.
    /// </summary>
    public class HtmlHelper
    {
        /// <summary>
        /// Returns an XML document from a given URL.
        /// </summary>
        /// <param name="web">The web.</param>
        /// <param name="url">The URL.</param>
        /// <param name="format">The format.</param>
        /// <param name="absolutizeLinks">if set to <c>true</c> [absolutize links].</param>
        /// <returns></returns>
        public static XmlDocument LoadHtmlAsXml(HtmlWeb web, string url, string format,
            bool absolutizeLinks)
        {
            // Declare necessary stream and writer objects
            MemoryStream m = new MemoryStream();
            XmlTextWriter xtw = new XmlTextWriter(m, null);

            // Load the content into the writer
            if (format == "html")
            {
                web.LoadHtmlAsXml(url, xtw);
                // Rewind the memory stream
                m.Position = 0;
                // Create, fill, and return the xml document
                XmlDocument xdoc = new XmlDocument();
                string content = (new StreamReader(m)).ReadToEnd();

                HtmlDocument doc = new HtmlDocument();
                doc.OptionOutputAsXml = true;
                doc.LoadHtml(content);

                if (absolutizeLinks == true)
                {
                    AttributeReferenceAbsolutizer.ExecuteDefaultAbsolutization
                        (doc.DocumentNode, url);
                }

                xdoc.LoadXml(doc.DocumentNode.OuterHtml);

                return xdoc;
            }
            else
            {
                HtmlDocument doc = web.Load(url);
                doc.OptionOutputAsXml = true;
                XmlDocument xdoc = new XmlDocument();

                if (absolutizeLinks == true)
                {
                    AttributeReferenceAbsolutizer.ExecuteDefaultAbsolutization
                        (doc.DocumentNode, url);
                }

                xdoc.LoadXml(doc.DocumentNode.OuterHtml);

                return xdoc;
            }
        }
    }
}
