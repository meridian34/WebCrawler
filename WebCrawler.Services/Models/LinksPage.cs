using System.Collections.Generic;

namespace WebCrawler.Services.Models
{
    public class LinksPage
    {
        public string Url { get; set; }
        public int ItemsCount { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<Link> Links { get; set; }
    }
}
