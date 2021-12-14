using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WebCrawler.Models;
using WebCrawler.Services.Parsers;

namespace WebCrawler.Services.Crawlers
{
    public class SitemapCrawler
    {
        private readonly UrlValidatorService _urlValidator;
        private readonly RequestService _requestService;
        private readonly SitemapParser _parserService;
        private readonly LinkConvertorService _convertor;
        public SitemapCrawler(
            UrlValidatorService urlValidatorService,
            RequestService requestService,
            SitemapParser parserService,
            LinkConvertorService convertorService)
        {
            _urlValidator = urlValidatorService;
            _requestService = requestService;
            _parserService = parserService;
            _convertor = convertorService;
        }
       
        public virtual IEnumerable<Link> RunCrawler(string url)
        {
            var delay = 50;
            var sitemapUrl = _convertor.GetDefaultSitemap(url);
            var linkQueue = new Queue<string>();
            var resultList = new List<Link>();
            linkQueue.Enqueue(sitemapUrl);

            while (linkQueue.Count > 0)
            {
                var link = linkQueue.Dequeue();
                var resultsContainsLink = resultList.Any(x => x.Url == link);
                var notValidLink = !_urlValidator.LinkIsValid(link);

                if (notValidLink || resultsContainsLink)
                {
                    continue;
                }

                Thread.Sleep(delay);
                
                var xml = _requestService.Download(link);
                
                var notContainsLinkSitemap = !link.Contains(".xml.gz");

                if (notContainsLinkSitemap)
                {
                    resultList.Add(new Link() { Url = link, IsSitemap = true });
                }

                var parsedLinks = _parserService.GetSitemapLinks(xml);

                foreach (var newLink in parsedLinks)
                {
                    if (!linkQueue.Any(x => x == newLink))
                    {
                        linkQueue.Enqueue(newLink);
                    }
                }
            }

            return resultList;
        }
    }
}
