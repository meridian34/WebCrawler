using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using WebCrawler.Services.Abstractions;

namespace WebCrawler.Services
{
    public class HtmlDocumentService : IHtmlDocumentService
    {
        private readonly string _tag = "a";
        private readonly string _anchor = "#";
        private readonly string _hrefAttribute = "href";

        public IReadOnlyCollection<string> GetLinks(string htmlBody)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlBody);
            var links = GetLinkByTag(htmlDocument, _tag).ToList();
            return links;
        }

        private IReadOnlyCollection<string> GetLinkByTag(HtmlDocument document, string tag)
        {
            var linkedPages = document.DocumentNode.Descendants(tag)
                                              .Select(a => a.GetAttributeValue(_hrefAttribute, null))
                                              .Where(u => !string.IsNullOrEmpty(u)).ToList();
            return PrepareLinks(linkedPages);
        }

        private IReadOnlyCollection<string> PrepareLinks(IReadOnlyCollection<string> links)
        {
            var linksList = new List<string>();
            foreach (var link in links)
            {
                if (link.Contains(_anchor))
                {
                    linksList.Add(DeleteAnchor(link));
                }
                else
                {
                    linksList.Add(link);
                }
            }

            return linksList;
        }

        private string DeleteAnchor(string link)
        {
            return link.Substring(0, link.LastIndexOf("#"));
        }
    }
}
