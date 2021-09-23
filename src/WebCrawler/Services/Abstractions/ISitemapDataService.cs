using System.Collections.Generic;
using System.Xml;

namespace WebCrawler.Services.Abstractions
{
    public interface ISitemapDataService
    {
        public bool IsSitemapIndexDocument(string sitemapXml);

        public bool IsSitemapIndexDocument(XmlDocument xmlDoc);

        public bool IsUrlSetDocument(string sitemapXml);

        public bool IsUrlSetDocument(XmlDocument xmlDoc);

        public IReadOnlyCollection<string> GetUrls(string sitemapXml);
    }
}
