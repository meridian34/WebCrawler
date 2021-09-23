using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebCrawler.Models;
using WebCrawler.Services.Abstractions;

namespace WebCrawler.Services
{
    public class SiteMapService : ISiteMapService
    {
        private IWebHandlerService _webHandlerService;
        private ISitemapDataService _xmlDocumentService;

        public SiteMapService(IWebHandlerFactory webHandlerFactory, ISitemapDataService xmlDocumentService)
        {
            _xmlDocumentService = xmlDocumentService;
            _webHandlerService = webHandlerFactory.CreateForSiteMap();
        }

        public async Task<IReadOnlyCollection<HttpScanResult>> MapAsync(string sitemapXmlUrl)
        {
            var result = await _webHandlerService.ScanUrlAsync(sitemapXmlUrl);
            if (result.Exception == null && result.Content != null)
            {
                return await GetResults(result.Content);
            }

            throw new ArgumentException();
        }

        private async Task<IReadOnlyCollection<HttpScanResult>> GetResults(string content)
        {
            if (_xmlDocumentService.IsUrlSetDocument(content))
            {
                var urls = _xmlDocumentService.GetUrls(content);
                return await _webHandlerService.ScanUrlConcurencyAsync(urls);
            }
            else if (_xmlDocumentService.IsSitemapIndexDocument(content))
            {
                var results = new List<HttpScanResult>();
                var urls = _xmlDocumentService.GetUrls(content);
                var resultsSitemapIndex = await _webHandlerService.ScanUrlConcurencyAsync(urls);

                foreach (var res in resultsSitemapIndex)
                {
                    results.AddRange(await GetResults(res.Content));
                }

                return results;
            }
            else
            {
                return new List<HttpScanResult>();
            }
        }
    }
}
