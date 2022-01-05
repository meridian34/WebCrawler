using System.Collections.Generic;
using WebCrawler.Services.Models;

namespace WebCrawler.WebApplication.ViewModels
{
    public class LinksPageViewModel
    {
        public string Url { get; set; }
        public IEnumerable<Link> Links { get; set; }
    }
}
