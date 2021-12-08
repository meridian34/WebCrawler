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

        public IReadOnlyCollection<string> GetLinksV2(string htmlBody)
        {
            var parser = new HtmlParserService();
            var res = parser.Parse(htmlBody);
            var links = res.Where(x=>x.Tag==_linkTag).Select(a=>a.Attributes.Where(b=>b.Key == _hrefAttribute ).Select(n=>n.Value));
            return links.ToList();
        }
    }
}
