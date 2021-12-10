using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Models;
using WebCrawler.Services.New;

namespace WebCrawler.Services
{
    public class SitemapCrawlerService
    {
        private readonly UrlValidatorService _urlValidator;
        private readonly RequestService _requestService;
        private readonly ParserService _parserService;
        public SitemapCrawlerService(
            UrlValidatorService urlValidatorService,
            RequestService requestService,
            ParserService parserService)
        {
            _urlValidator = urlValidatorService;
            _requestService = requestService;
            _parserService = parserService;
        }
       
        public virtual IEnumerable<CrawlResult> RunCrawler(string url)
        {
            var sitemapUrl = GetDefaultSitemap(url);
            var linkQueue = new Queue<string>();
            var resultList = new List<CrawlResult>();
            linkQueue.Enqueue(sitemapUrl);

            while (linkQueue.Count > 0)
            {
                var link = linkQueue.Dequeue();
                var resultContainsLink = resultList.Any(x => x.Url == link);

                if (!_urlValidator.IsValidLink(link) || resultContainsLink)
                {
                    continue;
                }

                var result = _requestService.ScanUrlAsync(link, out string html);
                result.IsSiteScan = true;
                resultList.Add(result);
                var parsedLinks = _parserService.GetSitemapLinks(html);

                foreach (var newLink in parsedLinks)
                {   
                    AnalyzeLink(newLink, linkQueue);
                }
            }

            return resultList;
        }

        private void AnalyzeLink(string parsedLinks, Queue<string> queue)
        {
            if (!queue.Any(x => x == parsedLinks))
            {
                queue.Enqueue(parsedLinks);
            }
        }

        private string GetDefaultSitemap(string url)
        {
            var basePath = new Uri(url).GetLeftPart(UriPartial.Authority);
            var baseUrl = new Uri(basePath);
            var defaultSitemapUri = new Uri(baseUrl, "sitemap.xml").ToString();
            return defaultSitemapUri;
        }
    }
}
