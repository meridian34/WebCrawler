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
        private readonly List<CrawlResult> _crawlResults;

        public WebCrawlerService()
        {
            _siteMapService = new SiteMapService();
            _siteScanService = new SiteScanService();
            _crawlResults = new List<CrawlResult>();
        }

        public virtual async Task<IEnumerable<CrawlResult>> RunCrawler(string url)
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

            AddResults(await _siteMapService.MapAsync(new Uri(url)));
            AddResults(await _siteScanService.ScanSiteAsync(url));

            //List<CrawlResult> results1 = new List<CrawlResult>();

            //var crawlingResult = new WebCrawlingResult();
            //crawlingResult.SitemapResults = siteMapResults.Select(x => new CrawlResult { Url = x.Url, ElapsedMilliseconds = (int)x.ElapsedMilliseconds }).ToList();
            //crawlingResult.ScanResults = siteScanResults.Select(x => new CrawlResult { Url = x.Url, ElapsedMilliseconds = (int)x.ElapsedMilliseconds }).ToList();
            //crawlingResult.SitemapUniqueResults = crawlingResult.SitemapResults
            //    .Where(x => !crawlingResult.ScanResults.Any(y => y.Url == x.Url))
            //    .ToList();

            //crawlingResult.ScanUniqueResults = crawlingResult.ScanResults
            //    .Where(x => !crawlingResult.SitemapResults.Any(y => y.Url == x.Url))
            //    .ToList();

            //var results = crawlingResult.ScanResults.ToList();
            //results.AddRange(crawlingResult.SitemapUniqueResults.ToList());
            //crawlingResult.AllResults = results.OrderBy(x => x.ElapsedMilliseconds).ToList();

            return _crawlResults;
        }

        private void AddResults(IEnumerable<HttpScanResult> scanResults)
        {
            foreach(var result in scanResults)
            {
                var findResult = _crawlResults.Where(x => x.Url == result.Url).FirstOrDefault();
                if(findResult != null)
                {
                    findResult.IsSiteMap = findResult.IsSiteMap == false ? result.IsSiteMap : findResult.IsSiteMap;
                    findResult.IsSiteScan = findResult.IsSiteScan == false ? result.IsSiteScan : findResult.IsSiteScan;
                }
                else
                {
                    _crawlResults.Add(new CrawlResult
                    {
                        ElapsedMilliseconds = (int)result.ElapsedMilliseconds,
                        Url = result.Url,
                        IsSiteScan = result.IsSiteScan,
                        IsSiteMap = result.IsSiteMap
                    });
                }
            }
            
        }
    }
}
