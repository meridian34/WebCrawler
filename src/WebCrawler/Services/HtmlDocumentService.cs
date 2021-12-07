using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using WebCrawler.Services.Abstractions;

namespace WebCrawler.Services
{
    public class HtmlDocumentService : IHtmlDocumentService
    {
        private const string _linkTag = "a";
        private const string _anchor = "#";
        private const string _hrefAttribute = "href";

        public IReadOnlyCollection<string> GetLinks(string htmlBody)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlBody);
            var links = htmlDocument.DocumentNode.Descendants(_linkTag)
                                              .Select(a => a.GetAttributeValue(_hrefAttribute, null))
                                              .Where(u => !string.IsNullOrEmpty(u) && !u.Contains(_anchor)).ToList();

            return links;
        }
    }
}
