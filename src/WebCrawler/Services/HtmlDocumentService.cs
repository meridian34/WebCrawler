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
            var parser = new HtmlLinkParserService();
            var res = parser.GetLink(htmlBody);
            var res2 = res.Where(x => !x.Contains(_anchor)).ToList();
            return res2;
        }
    }
}
