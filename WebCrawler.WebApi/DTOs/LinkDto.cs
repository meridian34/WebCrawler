using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawler.WebApi.DTOs
{
    public class LinkDto
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public bool FromSitemap { get; set; }

        public bool FromHtml { get; set; }

        public int? ElapsedMilliseconds { get; set; }
    }
}
