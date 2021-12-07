using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using WebCrawler.Models;
using WebCrawler.Services.Abstractions;

namespace WebCrawler.Services
{
    public class SiteMapService : ISiteMapService
    {
        private const string _sitemapindexName = "sitemapindex";
        private const string _urlsetName = "urlset";
        private const string _linkTag = "loc";
        private const string _urlTag = "url";
        private const string _sitemapTag = "sitemap";

        private IWebHandlerService _webHandlerService;
       
        public SiteMapService(IWebHandlerFactory webHandlerFactory)
        {
            _webHandlerService = webHandlerFactory.CreateForSiteMap();
        }

        public async Task<IReadOnlyCollection<HttpScanResult>> MapAsync(string sitemapXmlUrl)
        {
            var webResult = await _webHandlerService.ScanUrlAsync(sitemapXmlUrl);
            var isValidResult = webResult.Exception == null && webResult.Content != null;
            if (!isValidResult)
            {
                throw new ArgumentException();
            }

            var scanResults = new List<HttpScanResult>();
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(webResult.Content);

            var isSitemapIndexDocument = xmlDocument.DocumentElement.Name == _sitemapindexName;
            var isUrlSetDocument = xmlDocument.DocumentElement.Name == _urlsetName;

            if (isUrlSetDocument)
            {
                var nodeList = xmlDocument.GetElementsByTagName(_urlTag);
                var urls = GetUrlsCollection(nodeList);                
                return await _webHandlerService.ScanUrlConcurencyAsync(urls);
            }
            else if (isSitemapIndexDocument)
            {
                var nodeList = xmlDocument.GetElementsByTagName(_sitemapTag);
                var urls = GetUrlsCollection(nodeList);
                var sitemapIndexResults = await _webHandlerService.ScanUrlConcurencyAsync(urls);

                foreach (var resuslt in sitemapIndexResults)
                {
                    scanResults.AddRange(await MapAsync(resuslt.Content));
                }

                return scanResults;
            }
            else
            {
                return new List<HttpScanResult>();
            }
        }
        
        private IReadOnlyCollection<HttpScanResult> GetUrlsCollection(XmlNodeList nodeList)
        {
            var resultList = new List<HttpScanResult>();
            foreach (XmlNode nodeItem in nodeList)
            {
                resultList.Add(new HttpScanResult() { Url = nodeItem[_linkTag].InnerText, IsCrawled = false });
            }

            return resultList;
        }
    }
}
