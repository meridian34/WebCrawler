using System;

namespace WebCrawler.Models
{
    public class Link
    {
        public Uri Url { get; set; }
        public bool IsSitemap { get; set; }
        public bool IsCrawler { get; set; }
    }
}
