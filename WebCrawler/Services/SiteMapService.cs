using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using WebCrawler.Models;

namespace WebCrawler.Services
{
    public class SiteMapService 
    {
        const string SitemapindexName = "sitemapindex";
        const string UrlsetName = "urlset";
        const string LinkTag = "loc";
        const string UrlTag = "url";
        const string SitemapTag = "sitemap";

        private WebHandlerService _webHandlerService;
       
        public SiteMapService(WebHandlerFactory webHandlerFactory)
        {
            _webHandlerService = webHandlerFactory.CreateForSiteMap();
        }

        public virtual async Task<IReadOnlyCollection<HttpScanResult>> MapAsync(string sitemapXmlUrl)
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

            var isSitemapIndexDocument = xmlDocument.DocumentElement.Name == SitemapindexName;
            var isUrlSetDocument = xmlDocument.DocumentElement.Name == UrlsetName;

            if (isUrlSetDocument)
            {
                var nodeList = xmlDocument.GetElementsByTagName(UrlTag);
                var urls = GetUrlsCollection(nodeList);

                return await _webHandlerService.ScanUrlConcurencyAsync(urls);
            }
            else if (isSitemapIndexDocument)
            {
                var nodeList = xmlDocument.GetElementsByTagName(SitemapTag);
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
                resultList.Add(new HttpScanResult() { Url = nodeItem[LinkTag].InnerText, IsCrawled = false });
            }

            return resultList;
        }
    }
}
