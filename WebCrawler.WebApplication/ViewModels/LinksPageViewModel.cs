using System.Collections.Generic;

namespace WebCrawler.WebApplication.ViewModels
{
    public class LinksPageViewModel
    {
        public string Url { get; set; }
        public IEnumerable<LinkViewModel> Links { get; set; }
        public IEnumerable<string> SitemapLinks { get; set; }
        public IEnumerable<string> HtmlLinks { get; set; }
    }
}
