using System.Collections.Generic;
using WebCrawler.Services.Abstractions;

namespace WebCrawler.Services
{
    public class WebHandlerFactory : IWebHandlerFactory
    {
        private readonly IReadOnlyCollection<string> _contentTypesToHtml;
        private readonly IReadOnlyCollection<string> _contentTypesToXml;
        private readonly int _maxConcarency;
        private readonly int _dalayMilliseconds;

        public WebHandlerFactory(int maxConcarency, int dalayMilliseconds, IReadOnlyCollection<string> contentTypesToHtml, IReadOnlyCollection<string> contentTypesToXml)
        {
            _contentTypesToHtml = contentTypesToHtml;
            _contentTypesToXml = contentTypesToXml;
            _maxConcarency = maxConcarency;
            _dalayMilliseconds = dalayMilliseconds;
        }

        public WebHandlerService CreateForSiteScan()
        {
            return new WebHandlerService(_contentTypesToHtml, _maxConcarency, _dalayMilliseconds);
        }

        public WebHandlerService CreateForSiteMap()
        {
            return new WebHandlerService(_contentTypesToXml, _maxConcarency, _dalayMilliseconds);
        }
    }
}
