using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawler.WebApi.DTOs
{
    public class TestDto
    {
        public int Id { get; set; }

        public string UserLink { get; set; }

        public DateTimeOffset TestDateTime { get; set; }
    }
}
