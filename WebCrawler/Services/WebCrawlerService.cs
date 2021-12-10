using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Models;
using WebCrawler.Services.New;

namespace WebCrawler.Services
{
    public class WebCrawlerService 
    {
        private readonly SiteMapService _siteMapService;
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

            AddCrawlResults(await _siteMapService.MapAsync(new Uri(url)));
            //AddCrawlResults(await _siteScanService.ScanSiteAsync(url));
            var htmlService = new HtmlCrawlerService(new UrlValidatorService(), new RequestService(), new ParserService(), new LinkConverterService());
            AddCrawlResults(htmlService.RunCrawler(url));
            return _crawlResults;
        }

        private void AddCrawlResults(IEnumerable<CrawlResult> scanResults)
        {
            foreach (var result in scanResults)
            {
                var findResult = _crawlResults.Where(x => x.Url == result.Url).FirstOrDefault();
                if (findResult != null)
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

        private void AddCrawlResults(IEnumerable<HttpScanResult> scanResults)
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
