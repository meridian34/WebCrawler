using System.Collections.Generic;

namespace WebCrawler.WebApi.Models
{
    public class TestsPage : BasePage
    {
        public IEnumerable<TestResponse> Tests { get; set; }
    }
}
