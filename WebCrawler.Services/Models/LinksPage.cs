using System.Collections.Generic;

namespace WebCrawler.Services.Models
{
    public class LinksPage
    {
        public string Url { get; set; }
        public IEnumerable<Link> Links { get; set; }
        public IEnumerable<string> SitemapLinks { get; set; }
        public IEnumerable<string> HtmlLinks { get; set; }
    }
}
