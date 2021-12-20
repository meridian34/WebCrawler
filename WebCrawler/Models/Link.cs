using System;

namespace WebCrawler.Models
{
    public class Link
    {
        public Uri Url { get; set; }
        public bool FromSitemap { get; set; }
        public bool FromHtml { get; set; }
    }
}
