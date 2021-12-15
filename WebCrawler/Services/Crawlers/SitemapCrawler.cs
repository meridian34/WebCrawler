using System.Collections.Generic;
using System.Linq;
using WebCrawler.Models;
using WebCrawler.Services.Parsers;

namespace WebCrawler.Services.Crawlers
{
    public class SitemapCrawler
    {
        private readonly RequestService _requestService;
        private readonly SitemapParser _parserService;
        private readonly LinkConvertorService _convertor;

        public SitemapCrawler(
            RequestService requestService,
            SitemapParser parserService,
            LinkConvertorService convertorService)
        {
            _requestService = requestService;
            _parserService = parserService;
            _convertor = convertorService;
        }

        public virtual IEnumerable<Link> RunCrawler(string url)
        {
            var sitemapUrl = _convertor.GetDefaultSitemap(url);
            var linkQueue = new Queue<string>();
            var resultList = new List<Link>();
            linkQueue.Enqueue(sitemapUrl);

            while (linkQueue.Count > 0)
            {
                var link = linkQueue.Dequeue();
                var xml = _requestService.Download(link);
                var parsedLinks = _parserService.GetSitemapLinks(xml);

                foreach (var newLink in parsedLinks)
                {
                    var resultsContainsLink = resultList.Any(x => x.Url == newLink);
                    var containsXmlExtension = newLink.Contains(".xml");
                    
                    if (containsXmlExtension)
                    {
                        linkQueue.Enqueue(newLink);
                    }
                    else if (!containsXmlExtension && !resultsContainsLink)
                    {
                        resultList.Add(new Link() { Url = newLink, IsSitemap = true });
                    }
                }
            }

            return resultList;
        }
    }
}
