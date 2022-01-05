using System.Collections.Generic;
using WebCrawler.Services.Models;

namespace WebCrawler.WebApplication.ViewModels
{
    public class TestsPageViewModel
    {
        public int ItemsCount { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public IEnumerable<Test> Tests { get; set; }
    }
}
