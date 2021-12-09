using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Models;

namespace WebCrawler.Services
{
    public class WebCrawlerService 
    {
        private readonly SiteMapService _siteMapService ;
        private readonly SiteScanService _siteScanService;
        
        public WebCrawlerService()
        {
            _siteMapService = new SiteMapService();
            _siteScanService = new SiteScanService();
        }

        public virtual async Task<WebCrawlingResult> RunCrawler(string url)
        {
            var isHttpOrHttpsShema = url.Contains(Uri.UriSchemeHttp) || url.Contains(Uri.UriSchemeHttp);

            if (!isHttpOrHttpsShema)
            {
                url = new UriBuilder(url).Uri.ToString();
            }

            bool isValidUrl = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult);

            if (!isValidUrl)
            {
                throw new ArgumentException("Url in not valid");
            }

            var siteMapResults = await _siteMapService.MapAsync(new Uri(url));
            var siteScanResults = await _siteScanService.ScanSiteAsync(url);

            var crawlingResult = new WebCrawlingResult();
            crawlingResult.SitemapResults = siteMapResults.Select(x => new CrawlResult { Url = x.Url, ElapsedMilliseconds = (int)x.ElapsedMilliseconds }).ToList();
            crawlingResult.ScanResults = siteScanResults.Select(x => new CrawlResult { Url = x.Url, ElapsedMilliseconds = (int)x.ElapsedMilliseconds }).ToList();
            crawlingResult.SitemapUniqueResults = crawlingResult.SitemapResults
                .Where(x => !crawlingResult.ScanResults.Any(y => y.Url == x.Url))
                .ToList();

            crawlingResult.ScanUniqueResults = crawlingResult.ScanResults
                .Where(x => !crawlingResult.SitemapResults.Any(y => y.Url == x.Url))
                .ToList();

            var results = crawlingResult.ScanResults.ToList();
            results.AddRange(crawlingResult.SitemapUniqueResults.ToList());
            crawlingResult.AllResults = results.OrderBy(x => x.ElapsedMilliseconds).ToList();

            return crawlingResult;
        }
    }
}
