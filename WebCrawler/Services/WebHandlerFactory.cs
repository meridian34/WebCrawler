using System.Collections.Generic;
using WebCrawler.Services.Abstractions;

namespace WebCrawler.Services
{
    public class WebHandlerFactory 
    {
        private readonly IReadOnlyCollection<string> _contentTypesToHtml = new[]{"text/html; charset=utf-8", "text/html" };
        private readonly IReadOnlyCollection<string> _contentTypesToXml = new[] {"text/xml", "application/xml", "text/xml; charset=UTF-8" };
        private const int MaxConcarency = 4;
        private const int DelayMilliseconds = 100;

        public WebHandlerService CreateForSiteScan()
        {
            return new WebHandlerService(_contentTypesToHtml, MaxConcarency, DelayMilliseconds);
        }

        public WebHandlerService CreateForSiteMap()
        {
            return new WebHandlerService(_contentTypesToXml, MaxConcarency, DelayMilliseconds);
        }
    }
}
