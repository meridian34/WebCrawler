using System;
using System.Collections.Generic;

namespace WebCrawler.EntityFramework.Entities
{
    public class TestEntity
    {
        public int Id { get; set; }

        public string UserLink { get; set; }

        public DateTimeOffset TestDateTime { get; set; }

        public List<LinkEntity> Links { get; set; }
    }
}
