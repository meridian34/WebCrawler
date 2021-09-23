using System;
using System.Collections.Generic;
using System.Xml;
using WebCrawler.Services.Abstractions;

namespace WebCrawler.Services
{
    public class SitemapDataService : ISitemapDataService
    {
        private const string _sitemapindexName = "sitemapindex";
        private const string _urlsetName = "urlset";
        private const string _linkTag = "loc";
        private const string _urlTag = "url";
        private const string _sitemapTag = "sitemap";

        public bool IsSitemapIndexDocument(string sitemapXml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(sitemapXml);
            return IsSitemapIndexDocument(doc);
        }

        public bool IsSitemapIndexDocument(XmlDocument xmlDoc)
        {
            return xmlDoc.DocumentElement.Name == _sitemapindexName ? true : false;
        }

        public bool IsUrlSetDocument(string sitemapXml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(sitemapXml);
            return IsUrlSetDocument(doc);
        }

        public bool IsUrlSetDocument(XmlDocument xmlDoc)
        {
            return xmlDoc.DocumentElement.Name == _urlsetName ? true : false;
        }

        public IReadOnlyCollection<string> GetUrls(string sitemapXml)
        {
            var doc2 = new XmlDocument();
            var list = new List<string>();
            doc2.LoadXml(sitemapXml);
            if (IsSitemapIndexDocument(doc2))
            {
                return FindUrls(GetXmlSitemapList(doc2));
            }
            else if (IsUrlSetDocument(doc2))
            {
                return FindUrls(GetXmlUrlList(doc2));
            }
            else
            {
                throw new ArgumentException();
            }
        }

        private IReadOnlyCollection<string> FindUrls(XmlNodeList nodeList)
        {
            var resultList = new List<string>();
            foreach (XmlNode nodeItem in nodeList)
            {
                resultList.Add(nodeItem[_linkTag].InnerText);
            }

            return resultList;
        }

        private XmlNodeList GetXmlUrlList(XmlDocument xmlDocument)
        {
            XmlNodeList xmlSitemapList = xmlDocument.GetElementsByTagName(_urlTag);
            return xmlSitemapList;
        }

        private XmlNodeList GetXmlSitemapList(XmlDocument xmlDocument)
        {
            XmlNodeList xmlSitemapList = xmlDocument.GetElementsByTagName(_sitemapTag);
            return xmlSitemapList;
        }
    }
}
