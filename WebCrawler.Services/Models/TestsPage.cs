using System.Collections.Generic;

namespace WebCrawler.Services.Models
{
    public class TestsPage
    {
        public int ItemsCount { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<Test> Tests { get; set; }
    }
}
