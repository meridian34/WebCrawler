using System.Collections.Generic;

namespace WebCrawler.WebApi.Models
{
    public class TestsPage : BasePage
    {
        public int ItemsCount { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<TestResponse> Tests { get; set; }
    }
}
