using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
    public class CrawlerData
    {
        public Uri Url { get; set; }
        public bool Scanned { get; set; }
    }
}
