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

        public WebCrawlerService(HtmlCrawler htmlCrawler, SitemapCrawler sitemapCrawler, UrlValidatorService urlValidatorService)
        {
            _validator = urlValidatorService;
            _htmlCrawler = htmlCrawler;
            _sitemapCrawler = sitemapCrawler;
        }

        public int GetHtmlLinksCount(string url)
        {
            ValidateUrl(url);

            return _htmlCrawler.RunCrawler(url).Count();
        }

        public IEnumerable<Link> GetHtmlLinks(string url)
        {
            ValidateUrl(url);

            return _htmlCrawler.RunCrawler(url);
        }

        public IEnumerable<PerfomanceData> GetHtmlLinksWithPerfomance(string url)
        {
            ValidateUrl(url);

            return _htmlCrawler.RunCrawler(url);
        }

        public IEnumerable<Link> GetUniqueHtmlLinks(string url)
        {
            return GetUniqueCrawlingResult(url, false, true);
        }

        public int GetSitemapLinksCount(string url)
        {
            ValidateUrl(url);

            return _sitemapCrawler.RunCrawler(url).Count();
        }

        public IEnumerable<Link> GetSitemapLinks(string url)
        {
            ValidateUrl(url);

            return _sitemapCrawler.RunCrawler(url);
        }

        public IEnumerable<PerfomanceData> GetSitemapLinksWithPerfomance(string url)
        {
            ValidateUrl(url);

            return _sitemapCrawler.RunCrawler(url);
        }
        public IEnumerable<Link> GetUniqueSitemapLinks(string url)
        {
            return GetUniqueCrawlingResult(url, true, false);
        }

        public IEnumerable<PerfomanceData> GetUniqueCrawlingResult(string url)
        {
            return GetUniqueCrawlingResult(url, true, true);
        }

        public IEnumerable<PerfomanceData> GetUniqueCrawlingResult(string url, bool unicalSitemapResults, bool unicalHtmlResults)
        {
            ValidateUrl(url);

            var htmlResults = _htmlCrawler.RunCrawler(url);
            var sitemapResults = _sitemapCrawler.RunCrawler(url);
            var unicalResults = new List<PerfomanceData>();

            if (unicalHtmlResults && unicalSitemapResults)
            {
                unicalResults.AddRange(htmlResults);
                unicalResults.AddRange(sitemapResults.Where(x => !htmlResults.Any(y => y.Url == x.Url)));
            }
            else if (unicalHtmlResults && !unicalSitemapResults)
            {
                unicalResults.AddRange(htmlResults.Where(x => !sitemapResults.Any(y => y.Url == x.Url)));
            }
            else if (!unicalHtmlResults && unicalSitemapResults)
            {
                unicalResults.AddRange(sitemapResults.Where(x => !htmlResults.Any(y => y.Url == x.Url)));
            }

            return unicalResults;
        }

        private void ValidateUrl(string url)
        {
            bool urlIsNotValid = !_validator.IsCorrectLink(url) && !_validator.IsValidLink(url);
            if (urlIsNotValid)
            {
                throw new ArgumentException("Url in not valid");
            }
        }
    }
}
