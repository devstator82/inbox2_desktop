using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using HtmlAgilityPack.AddOns.FormProcessor;

namespace Inbox2.Utils.Scraper
{
	public class HttpScraper : IHttpScraper
	{
		private readonly HtmlWeb web;

	    public FormProcessor processor;

	    public Form form;

		HtmlWeb IHttpScraper.Web
		{
			get { return web; }
		}

		FormProcessor IHttpScraper.Processor
		{
			get { return processor; }
		}

		Form IHttpScraper.Form
		{
			get { return form; }
		}

		public string UserAgent
		{
			get { return web.UserAgent; }
			set { web.UserAgent = value; }
		}

		public Uri ResponseUri
		{
			get { return (this as IHttpScraper).Web.ResponseUri; }
		}

		public HttpScraper()
		{
			web = new FormProcessorWeb(true);
			web.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
			
		}

		public HttpScraper(string sourceUri, string xpath)
		{			
			web = new FormProcessorWeb(true);
			web.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
			processor = new FormProcessor(web);

			form = processor.GetForm(sourceUri, xpath, FormQueryModeEnum.Nested);
		}		

		public HtmlDocument NavigateTo(string url)
		{
			return web.Load(url);
		}

		public void SetForm(HtmlDocument doc, string url, string xpath)
		{
			form = processor.GetForm(doc, url, xpath, FormQueryModeEnum.Nested);
		}

        public void SetForm(Form _form)
        {
            form = _form;
        }

		public HtmlDocument Submit()
		{	
			HtmlDocument doc = processor.SubmitForm(form);

			(this as IHttpScraper).Web.Referer = String.Empty;

			return doc;
		}

		public HtmlDocument Submit(string url)
		{
			var doc = processor.SubmitForm(form.elements, url);

			(this as IHttpScraper).Web.Referer = String.Empty;

			return doc;
		}

		#region Setting field values

		public void SetTextFieldValue(string fieldname, string value)
		{
			foreach (var node in form.elements)
			{
				if (node.Attributes["name"] != null && node.Attributes["name"].Value == fieldname)
				{
					node.SetAttributeValue("value", value);
				}
			}
		}

		public void SetHiddenFieldValue(string fieldname, string value)
		{
			foreach (var node in form.elements)
			{
				if (node.Attributes["name"] != null && node.Attributes["name"].Value == fieldname)
				{
					node.SetAttributeValue("value", value);
				}
			}
		}

		public void SetTextAreaValue(string fieldname, string value)
		{
			foreach (var node in form.elements)
			{
				if (node.Attributes["name"] != null && node.Attributes["name"].Value == fieldname)
				{
					node.InnerHtml = value;
				}
			}
		}

		public void SetCheckboxValue(string fieldname, bool value)
		{
			foreach (var node in form.elements)
			{
				if (node.Attributes["name"] != null && node.Attributes["name"].Value == fieldname)
				{
					node.SetAttributeValue("checked", value ? "checked" : "");
				}
			}
		}

		public void SetCheckboxValue(string fieldname, string value)
		{
			foreach (var node in form.elements)
			{
				if (node.Attributes["name"] != null && node.Attributes["name"].Value == fieldname)
				{
					node.SetAttributeValue("checked", value);
				}
			}
		}

		public void SetRadioValue(string fieldname, bool value)
		{
			foreach (var node in form.elements)
			{
				if (node.Attributes["name"] != null && node.Attributes["name"].Value == fieldname)
				{
					node.SetAttributeValue("checked", value ? "checked" : "");
				}
			}
		}

        public void SetRadioValue(string fieldname, string value)
        {
            foreach (var node in form.elements)
            {
                if (node.Attributes["name"] != null && node.Attributes["name"].Value == fieldname)
                {
                    node.SetAttributeValue("value", value);
                }
            }
        }

		public void UnsetFieldValue(string fieldname)
		{
			foreach (var node in form.elements)
			{
				if (node.Attributes["name"] != null && node.Attributes["name"].Value == fieldname)
				{
					node.Attributes.Remove("value");
				}
			}
		}

		public string GetFieldValue(string fieldname)
		{
			foreach (var node in form.elements)
			{
				if (node.Attributes["name"] != null && node.Attributes["name"].Value == fieldname)
				{
					return node.GetAttributeValue("value", String.Empty);
				}
			}

			return "";
		}

		public void SetReferer(Uri uri)
		{
			(this as IHttpScraper).Web.Referer = uri.ToString();
		}

		#endregion
	}
}
