using System;

namespace WebCrawler.Models
{
    public class HttpScanResult
    {
        public string Url { get; set; }

        public bool IsCrawled { get; set; }

        public bool IsSiteScan { get; set; }

        public bool IsSiteMap { get; set; }

        public long ElapsedMilliseconds { get; set; }

        public int HttpStausCode { get; set; }

        public string HttpContentType { get; set; }

        public string Content { get; set; }

        public Exception Exception { get; set; }
    }
}
