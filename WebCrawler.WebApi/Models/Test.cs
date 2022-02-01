using System;

namespace WebCrawler.WebApi.Models
{
    public class TestResponse
    {
        public int Id { get; set; }

        public string UserLink { get; set; }

        public DateTimeOffset TestDateTime { get; set; }
    }
}
