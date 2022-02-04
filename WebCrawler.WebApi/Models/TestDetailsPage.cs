using System.Collections.Generic;

namespace WebCrawler.WebApi.Models
{
    public class TestDetailsPage : BasePage
    {
        public string Url { get; set; }

        public IEnumerable<Link> Links { get; set; }
    }
}
