using HtmlAgilityPack;
using HtmlAgilityPack.AddOns.FormProcessor;

namespace Inbox2.Utils.Scraper
{
	public interface IHttpScraper
	{
		HtmlWeb Web { get; }

		FormProcessor Processor { get; }

		Form Form { get; }
	}
}