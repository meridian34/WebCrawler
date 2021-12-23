using System;
using System.Collections.Generic;

namespace WebCrawler.EntityFramework.Entities
{
    public class Test
    {
        public int Id { get; set; }

        public string UserLink { get; set; }

        public DateTimeOffset TestDateTime { get; set; }

        public List<Link> Links { get; set; }
    }
}
