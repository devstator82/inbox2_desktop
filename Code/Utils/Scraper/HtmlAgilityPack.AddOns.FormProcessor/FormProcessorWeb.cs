// FormProcessor V0.1 - Josh Gough http://www.ultravioletconsulting.com
// This code references Simon Mourier's HtmlAgilityPack http://smourier.blogspot.com
using System;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
using System.Collections;
using HtmlAgilityPack;

namespace HtmlAgilityPack.AddOns.FormProcessor
{
    /// <summary>
    /// Interface that defines methods for implementhing a custom login handler
    /// that can be detected when the HtmlWeb.PostResponse event is raised. It should
    /// detect whether the has sent the client to a different URI than the 
    /// one requested, and process the redirection accordingly if so.
    /// </summary>
    public interface ILoginRedirectHandler
    {
        HtmlWeb Web { get; set; }
        void DetectLoginRedirect(HttpWebRequest request, HttpWebResponse response);
        void ProcessRedirectPage(HtmlDocument doc);
    }

    /// <summary>
    /// <para>
    /// Interface that defines methods for implementhing a custom login handler
    /// piror to HtmlDocument handling. It is the responsibility of this method
    /// to inspect the document's contents and determine whether the expected
    /// page was returned. If it instead determines that a login page was
    /// returned, it should process this page accordingly.
    /// </para>
    /// <para>
    /// In most cases, this means that the client cannot detect that the server
    /// has sent the client to a different URI than the requested one,
    /// possibly because it used Server.Transfer, Server.Execute, or the 
    /// equivalent on other environments. 
    /// </para>
    /// </summary>
    public interface ILoginPageHandler
    {
        HtmlWeb Web { get; set; }
        void DetectAndProcessLoginPage(HtmlDocument doc);
    }  

    /// <summary>
    /// Lightweight extension to HtmlWeb that provides cookie support by
    /// storing the cookies retrieved in each request in the same
    /// CookieContainer object. This enables dynamic sessions and navigation
    /// through a site.
    /// </summary>
    public class FormProcessorWeb : HtmlWeb
    {
    	private Uri _lastRequestUri;

        /// <summary>
        /// Contains the cookies to enable persistence across calls
        /// </summary>
        private System.Net.CookieContainer _cookieContainer = null;

        private ILoginRedirectHandler _loginRedirectHandler = null;

        /// <summary>
        /// Assign this to provide login page redirection logic when
        /// the PostResponse event is raised.
        /// </summary>
        public ILoginRedirectHandler LoginRedirectHandler
        {
            get { return _loginRedirectHandler; }
            set
            {
                if (this._loginRedirectHandler == null
                    && value != null)
                {
                    // The login redirect detector will assign a 
                    // PreHandleDocumentHandler callback to the
                    // PreHandleDocument if the redirection is detected
                    // inside of this callback:
                    this._loginRedirectHandler = value;
                    this.PostResponse +=
                        this._loginRedirectHandler.DetectLoginRedirect;
                }
            }
        }

        private ILoginPageHandler _loginPageHandler = null;

        /// <summary>
        /// Assign this to provide login page detection logic when
        /// the PreHandleDocument event is raised.
        /// </summary>
        public ILoginPageHandler LoginPageHandler
        {
            get { return _loginPageHandler; }
            set { _loginPageHandler = value; }
        }

        private bool _persistCookiesAcrossRequests;

        /// <summary>
        /// Indicates whether to use the same CookieCollection from
        /// one request to the next. If set, then you can expect
        /// to collect cookies on an initial request, and then post
        /// them back to the site when requesting an additional URI.
        /// </summary>
        public bool PersistCookiesAcrossRequests
        {
            get { return _persistCookiesAcrossRequests; }
            set
            {
                bool currentVal = _persistCookiesAcrossRequests;
                _persistCookiesAcrossRequests = value;

                // If the callback is not yet assigned, assign it:
                if (currentVal == false && value == true)
                {
                    // Instantiate the container if it's null
                    if (this._cookieContainer == null)
                        this.ResetCookies();

                    this.PreRequest += preRequestAssignCookies;
                }

                // Unregister it if turning it off
                if (value == false && currentVal == true)
                {
                    this.PreRequest -= preRequestAssignCookies;
                }
            }
        }

        /// <summary>
        /// Assigns the private _cookieContainer to the outbound request,
        /// assuring that the same collection is passed across calls.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        private bool preRequestAssignCookies(System.Net.HttpWebRequest target)
        {
			// Hyves switches from http://www.hyves.nl to https://secure.hyves.org, send along the cookies that we have
			//if (_lastRequestUri != null && _lastRequestUri.Scheme != target.RequestUri.Scheme)
			//{
			//    foreach (Cookie cookie in _cookieContainer.GetCookies(_lastRequestUri))
			//    {
			//        _cookieContainer.SetCookies(target.RequestUri, cookie.ToString());
			//    }
			//}

            target.CookieContainer = this._cookieContainer;

        	_lastRequestUri = target.RequestUri;

            return true;
        }

        /// <summary>
        /// Clears the current cookie collection
        /// </summary>
        public void ResetCookies()
        {
            this._cookieContainer = new System.Net.CookieContainer();
        }

        /// <summary>
        /// Default constructor calls Init().
        /// </summary>
        public FormProcessorWeb()
        {
            this.Init();            
        }

        /// <summary>
        /// Constructor that specifies whether to use cookies across multiple requests.
        /// </summary>
        /// <param name="persistCookiesAcrossRequest">if set to <c>true</c> [persist cookies across request].</param>
        public FormProcessorWeb(bool persistCookiesAcrossRequest)
        {
            this.PersistCookiesAcrossRequests = persistCookiesAcrossRequest;
            this.Init();
        }

        /// <summary>
        /// Assigns the callback handler for the login page redirect 
        /// detection, if assigned.
        /// </summary>
        private void Init()
        {
            this.PreHandleDocument += 
                new PreHandleDocumentHandler(OnPreHandleDocument); 
        }

        /* TODO: revise how this is done so the processor is not dependent upon HtmlDOMElementFactory
        this.PreCreateElement += 
            new PreCreateElementEventHandler(HtmlDOMElementFactory.DefaultOnCreateElement);
        */

        /// <summary>
        /// Calls the configured ILoginPageHandler if it has been set.
        /// </summary>
        /// <param name="doc"></param>
        protected void OnPreHandleDocument(HtmlDocument doc)
        {
            if (this._loginPageHandler != null)
            {
                this._loginPageHandler.DetectAndProcessLoginPage(doc);
            }
        }

        /// <summary>
        /// Loads an XML document from a URL, converting HTML into XML.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="format">The format.</param>
        /// <param name="absolutizeLinks">if set to <c>true</c> [absolutize links].</param>
        /// <returns></returns>
        public XmlDocument LoadHtmlAsXml(string url, string format, bool absolutizeLinks)
        {
            XmlDocument xdoc =
                HtmlHelper.LoadHtmlAsXml(this, url, format, absolutizeLinks);
            return xdoc;
        }
    }
}


