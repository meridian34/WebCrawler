using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawler.WebApi.DTOs
{
    public class TestDetailsPageDto
    {
        public string Url { get; set; }

        public IEnumerable<LinkDto> Links { get; set; }
    }
}
