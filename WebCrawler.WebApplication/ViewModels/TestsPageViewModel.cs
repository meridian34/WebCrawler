using System.Collections.Generic;

namespace WebCrawler.WebApplication.ViewModels
{
    public class TestsPageViewModel
    {
        public int ItemsCount { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public IEnumerable<TestViewModel> Tests { get; set; }
    }
}
