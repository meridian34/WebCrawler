using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
    public class CrawlResult
    {
        public string Url { get; set; }
        public int ElapsedMilliseconds { get; set; }
    }
}
