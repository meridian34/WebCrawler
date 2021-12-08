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
        private readonly string _sitemapLink = "sitemap.xml";
        private IReadOnlyCollection<HttpScanResult> _sitemapResults;
        private IReadOnlyCollection<HttpScanResult> _scanResults;
        private IReadOnlyCollection<HttpScanResult> _sitemapUniqueResults;
        private IReadOnlyCollection<HttpScanResult> _scanUniqueResults;

        public WebCrawlerService()
        {
            var factory = new WebHandlerFactory();
            _siteMapService = new SiteMapService(factory);
            _siteScanService = new SiteScanService(factory);
        }

        public virtual async Task RunCrawler(string url)
        {
            var isHttpOrHttpsShema = url.Contains(Uri.UriSchemeHttp) || url.Contains(Uri.UriSchemeHttp);

            if (!isHttpOrHttpsShema)
            {
                url = new UriBuilder(url).Uri.ToString();
            }

            Uri uriResult;
            bool isValidUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult);

            if (!isValidUrl)
            {
                throw new ArgumentException();
            }

            _sitemapResults = await _siteMapService.MapAsync(GetSitemapXmlUrl(url));
            _scanResults = await _siteScanService.ScanSiteAsync(GetRootUrl(url));
            _sitemapUniqueResults = _sitemapResults.Where(x => !_scanResults.Any(y => y.Url == x.Url)).ToList();
            _scanUniqueResults = _scanResults.Where(x => !_sitemapResults.Any(y => y.Url == x.Url)).ToList();
        }

        public virtual IReadOnlyCollection<HttpScanResult> GetSitemapUniqueResults()
        {
            return _sitemapUniqueResults;
        }

        public virtual IReadOnlyCollection<HttpScanResult> GetScanUniqueResults()
        {
            return _scanUniqueResults;
        }

        public  virtual IReadOnlyCollection<HttpScanResult> GetSitemapResults()
        {
            return _sitemapResults;
        }

        public virtual IReadOnlyCollection<HttpScanResult> GetScanResults()
        {
            return _scanResults;
        }

        public virtual IReadOnlyCollection<HttpScanResult> GetAllSortedResults()
        {
            var results = _scanResults.ToList();
            results.AddRange(_sitemapResults.ToList());
            return results.OrderBy(x => x.ElapsedMilliseconds).ToList();
        }

        private string GetRootUrl(string url)
        {
            Uri uriResult = new Uri(url);
            return uriResult.GetLeftPart(UriPartial.Authority);
        }

        private string GetSitemapXmlUrl(string url)
        {
            var baseUri = new Uri(GetRootUrl(url));
            var myUri = new Uri(baseUri, _sitemapLink);
            return myUri.ToString();
        }

        
    }
}
