using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawler.WebApi.Requests
{
    public class StartTestRequest
    {
        public string CrawlUrl { get; set; }
    }
}
