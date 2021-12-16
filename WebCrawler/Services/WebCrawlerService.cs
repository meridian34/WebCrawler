using System;
using System.Collections.Generic;
using System.Linq;
using WebCrawler.Models;
using WebCrawler.Services.Crawlers;

namespace WebCrawler.Services
{
    public class WebCrawlerService 
    {
        private readonly HtmlCrawler _htmlCrawler;
        private readonly SitemapCrawler _sitemapCrawler;
        private readonly UrlValidatorService _validator;
        private readonly RequestService _requestService;

        public WebCrawlerService(HtmlCrawler htmlCrawler, SitemapCrawler sitemapCrawler, UrlValidatorService urlValidatorService, RequestService requestService)
        {
            _validator = urlValidatorService;
            _htmlCrawler = htmlCrawler;
            _sitemapCrawler = sitemapCrawler;
            _requestService = requestService;
        }

        public virtual IEnumerable<Link> GetLinks(Uri url)
        {
            ValidateUrl(url);

            var htmlResults = _htmlCrawler.RunCrawler(url);
            var sitemapResults = _sitemapCrawler.RunCrawler(url);
            var results = new List<Link>();
            results.AddRange(htmlResults);
            
            foreach(var result in results)
            {
                var contains = sitemapResults.Any(x => x.Url == result.Url);
                if (contains)
                {
                    result.IsSitemap = true;
                }
            }

            results.AddRange(sitemapResults.Where(x => !htmlResults.Any(z => z.Url == x.Url)));

            return results;
        }

        public virtual IEnumerable<PerfomanceData> GetPerfomanceDataCollection(IEnumerable<Link> links)
        {
            return _requestService.GetElapsedTimeForLinks(links);
        }

        private void ValidateUrl(Uri url)
        {
            bool urlIsNotValid = !_validator.UrlIsValid(url);
            if (urlIsNotValid)
            {
                throw new ArgumentException("Url in not valid");
            }
        }
    }
}
