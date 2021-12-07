using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Models;
using WebCrawler.Services.Abstractions;

namespace WebCrawler.Services
{
    public class WebCrawlerService : IWebCrawlerService
    {
        private readonly ISiteMapService _siteMapService;
        private readonly ISiteScanService _siteScanService;
        private readonly string _sitemapLink = "sitemap.xml";
        private readonly string _httpScheme = "http://";
        private readonly string _httpsScheme = "https://";
        private IReadOnlyCollection<HttpScanResult> _sitemapResults;
        private IReadOnlyCollection<HttpScanResult> _scanResults;
        private IReadOnlyCollection<HttpScanResult> _sitemapUniqueResults;
        private IReadOnlyCollection<HttpScanResult> _scanUniqueResults;

        public WebCrawlerService(ISiteMapService siteMapService, ISiteScanService siteScanService)
        {
            _siteMapService = siteMapService;
            _siteScanService = siteScanService;
        }

        public event Action<IReadOnlyCollection<HttpScanResult>> SendingSitemapUniqueResults;

        public event Action<IReadOnlyCollection<HttpScanResult>> SendingScanUniqueResults;

        public event Action<IReadOnlyCollection<HttpScanResult>> SendingSitemapResults;

        public event Action<IReadOnlyCollection<HttpScanResult>> SendingScanResults;

        public event Action<IReadOnlyCollection<HttpScanResult>> SendingAllSortedResults;

        public async Task RunCrawler(string url)
        {
            if (!UrlIsHttpScheme(url) && !UrlIsHttpsScheme(url))
            {
                url = PrepareUrl(url);
            }

            if (!UrlIsValid(url))
            {
                throw new ArgumentException();
            }

            _sitemapResults = await _siteMapService.MapAsync(GetSitemapXmlUrl(url));
            _scanResults = await _siteScanService.ScanSiteAsync(GetRootUrl(url));
            _sitemapUniqueResults = _sitemapResults.Where(x => _scanResults.Where(y => y.Url == x.Url).FirstOrDefault() == null).ToList();
            _scanUniqueResults = _scanResults.Where(x => _sitemapResults.Where(y => y.Url == x.Url).FirstOrDefault() == null).ToList();

            SendResults();
        }

        public IReadOnlyCollection<HttpScanResult> GetSitemapUniqueResults()
        {
            return _sitemapUniqueResults;
        }

        public IReadOnlyCollection<HttpScanResult> GetScanUniqueResults()
        {
            return _scanUniqueResults;
        }

        public IReadOnlyCollection<HttpScanResult> GetSitemapResults()
        {
            return _sitemapResults;
        }

        public IReadOnlyCollection<HttpScanResult> GetScanResults()
        {
            return _scanResults;
        }

        public IReadOnlyCollection<HttpScanResult> GetAllSortedResults()
        {
            var results = _scanResults.ToList();
            results.AddRange(_sitemapResults.ToList());
            return results.OrderBy(x => x.ElapsedMilliseconds).ToList();
        }

        private void SendResults()
        {
            if (SendingSitemapUniqueResults != null)
            {
                SendingSitemapUniqueResults.Invoke(_sitemapUniqueResults);
            }

            if (SendingScanUniqueResults != null)
            {
                SendingScanUniqueResults.Invoke(_scanUniqueResults);
            }

            if (SendingAllSortedResults != null)
            {
                SendingAllSortedResults.Invoke(_scanResults.OrderBy(x => x.ElapsedMilliseconds).ToList());
            }

            if (SendingSitemapResults != null)
            {
                SendingSitemapResults.Invoke(_sitemapResults);
            }

            if (SendingScanResults != null)
            {
                SendingScanResults.Invoke(_scanResults);
            }
        }

        private static bool UrlIsValid(string url)
        {
            Uri uriResult;
            bool tryCreateResult = Uri.TryCreate(url, UriKind.Absolute, out uriResult);
            if (tryCreateResult == true && uriResult != null)
            {
                return true;
            }
            else
            {
                return false;
            }
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

        private bool UrlIsHttpsScheme(string url)
        {
            return url.Contains(_httpsScheme);
        }

        private bool UrlIsHttpScheme(string url)
        {
            return url.Contains(_httpScheme);
        }

        private string PrepareUrl(string url)
        {
            return new UriBuilder(url).Uri.ToString();
        }
    }
}
