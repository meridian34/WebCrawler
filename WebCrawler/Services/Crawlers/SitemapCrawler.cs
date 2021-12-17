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
            
            var resultList = new List<Link>();
            resultList.Add( new Link { Url = sitemapUrl });

            while (resultList.Any(x=>!x.IsSitemap))
            {
                var link = resultList.First(x => !x.IsSitemap);
                var xml = await _requestService.DownloadAsync(link.Url);
                link.IsSitemap = true;
                var parsedLinks = _parserService.GetSitemapLinks(xml);

                FilterLinksAndAdd(parsedLinks, resultList);
            }

            return resultList;
        }

        private void FilterLinksAndAdd(IEnumerable<Uri> filterLinks, List<Link> resultList)
        {
            foreach (var link in filterLinks)
            {
                var resultsContainsLink = resultList.Any(x => x.Url == link);
                var containsXmlExtension = link.OriginalString.Contains(".xml");

                if (!containsXmlExtension && !resultsContainsLink)
                {
                    resultList.Add(new Link() { Url = link });
                }
            }
        }
    }
}
