using System.Collections.Generic;

namespace WebCrawler.Services.Models
{
    public class TestsPage : BasePage
    {
        public IEnumerable<Test> Tests { get; set; }
    }
}
