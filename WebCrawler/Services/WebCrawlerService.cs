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
        
        public WebCrawlerService()
        {
            var factory = new WebHandlerFactory();
            _siteMapService = new SiteMapService(factory);
            _siteScanService = new SiteScanService(factory);
        }

        public virtual async Task<WebCrawlingResult> RunCrawler(string url)
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

            var crawlingResult = new WebCrawlingResult();
            crawlingResult.SitemapResults = await _siteMapService.MapAsync(GetSitemapXmlUrl(url));
            crawlingResult.ScanResults = await _siteScanService.ScanSiteAsync(GetRootUrl(url));
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
