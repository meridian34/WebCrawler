using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using WebCrawler.Services.Abstractions;

namespace WebCrawler.Services
{
    public class HtmlDocumentService : IHtmlDocumentService
    {
        public IReadOnlyCollection<string> GetLinks(string htmlBody)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlBody);
            var links = GetLinkByTag(htmlDocument, "a").ToList();
            return links;
        }

        private IEnumerable<string> GetLinkByTag(HtmlDocument document, string tag)
        {
            var linkedPages = document.DocumentNode.Descendants(tag)
                                              .Select(a => a.GetAttributeValue("href", null))
                                              .Where(u => !string.IsNullOrEmpty(u)).ToList();
            return linkedPages;
        }
    }
}
