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

            var results = new List<HttpScanResult>();
            var doc2 = new XmlDocument();
            doc2.LoadXml(webResult.Content);

            var isSitemapIndexDocument = doc2.DocumentElement.Name == _sitemapindexName;
            var isUrlSetDocument = doc2.DocumentElement.Name == _urlsetName;

            if (isUrlSetDocument)
            {
                var nodeList = doc2.GetElementsByTagName(_urlTag);
                var urls = GetUrlsCollection(nodeList);                
                return await _webHandlerService.ScanUrlConcurencyAsync(urls);
            }
            else if (isSitemapIndexDocument)
            {
                var nodeList = doc2.GetElementsByTagName(_sitemapTag);
                var urls = GetUrlsCollection(nodeList);
                var sitemapIndexResults = await _webHandlerService.ScanUrlConcurencyAsync(urls);

                foreach (var resuslt in sitemapIndexResults)
                {
                    results.AddRange(await MapAsync(resuslt.Content));
                }

                return results;
            }
            else
            {
                return new List<HttpScanResult>();
            }
        }
        
        private IReadOnlyCollection<string> GetUrlsCollection(XmlNodeList nodeList)
        {
            var resultList = new List<string>();
            foreach (XmlNode nodeItem in nodeList)
            {
                resultList.Add(nodeItem[_linkTag].InnerText);
            }

            return resultList;
        }
    }
}
