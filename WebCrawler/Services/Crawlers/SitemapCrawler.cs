using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Models;
using WebCrawler.Services.Parsers;

namespace WebCrawler.Services.Crawlers
{
    public class SitemapCrawler
    {
        private readonly WebRequestService _requestService;
        private readonly SitemapParser _parserService;
        private readonly LinkConvertorService _convertor;

        public SitemapCrawler(
            WebRequestService requestService,
            SitemapParser parserService,
            LinkConvertorService convertorService)
        {
            _requestService = requestService;
            _parserService = parserService;
            _convertor = convertorService;
        }

        public virtual async Task<IEnumerable<Link>> RunCrawlerAsync(Uri url)
        {
            var sitemapUrl = _convertor.GetDefaultSitemap(url);
            var resultList = new List<CrawlerData>();

            do
            {
                var link = resultList.Find(x => !x.Scanned);

                if (!resultList.Any())
                {
                    link = new CrawlerData { Url = sitemapUrl };
                }

                var xml = await _requestService.DownloadAsync(link.Url);
                link.Scanned = true;
                var parsedLinks = _parserService.GetSitemapLinks(xml);
                resultList.AddRange(GetUniqueLinks(parsedLinks, resultList));
            }
            while (resultList.Any(x => !x.Scanned));

            return resultList.Select(x => new Link { Url = x.Url, FromSitemap = true });
        }

        private IEnumerable<CrawlerData> GetUniqueLinks(IEnumerable<Uri> filterLinks, List<CrawlerData> targetList)
        {
            var resultList = new Queue<CrawlerData>();
            foreach (var link in filterLinks)
            {
                var resultsContainsLink = targetList.Any(x => x.Url == link);
                var containsXmlExtension = link.OriginalString.Contains(".xml");

                if (!containsXmlExtension && !resultsContainsLink)
                {
                    resultList.Enqueue(new CrawlerData() { Url = link });
                }
            }
            return resultList;
        }
    }
}
