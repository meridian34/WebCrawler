using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Models;
using WebCrawler.Services.Crawlers;

namespace WebCrawler.Services
{
    public class WebCrawlerService 
    {
        private readonly HtmlCrawler _htmlCrawler;
        private readonly SitemapCrawler _sitemapCrawler;
        private readonly UrlValidatorService _validator;
        private readonly WebRequestService _requestService;

        public WebCrawlerService(HtmlCrawler htmlCrawler, SitemapCrawler sitemapCrawler, UrlValidatorService urlValidatorService, WebRequestService requestService)
        {
            _validator = urlValidatorService;
            _htmlCrawler = htmlCrawler;
            _sitemapCrawler = sitemapCrawler;
            _requestService = requestService;
        }

        public virtual async Task<IEnumerable<Link>> GetLinksAsync(Uri url)
        {
            ValidateUrl(url);

            var htmlResults = await _htmlCrawler.RunCrawlerAsync(url);
            var sitemapResults = await _sitemapCrawler.RunCrawlerAsync(url);

            return SummarizeLists(htmlResults, sitemapResults);
        }

        public virtual async Task<IEnumerable<PerfomanceData>> GetPerfomanceDataCollectionAsync(IEnumerable<Link> links)
        {
            return await _requestService.GetElapsedTimeForLinksAsync(links);
        }

        private IEnumerable<Link> SummarizeLists(IEnumerable<Link> htmlResults, IEnumerable<Link> sitemapResults)
        {
            var results = new List<Link>();
            results.AddRange(htmlResults);

            foreach (var result in results)
            {
                var contains = sitemapResults.Any(x => x.Url == result.Url);
                if (contains)
                {
                    result.FromSitemap = true;
                }
            }

            results.AddRange(sitemapResults.Where(x => !htmlResults.Any(z => z.Url == x.Url)));
            return results;
        }

        private void ValidateUrl(Uri url)
        {
            bool urlIsNotValid = !_validator.ValidateUrl(url);
            if (urlIsNotValid)
            {
                throw new ArgumentException("Url in not valid");
            }
        }
    }
}
