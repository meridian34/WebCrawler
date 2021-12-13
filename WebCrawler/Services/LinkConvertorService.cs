﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Services
{
    public class LinkConvertorService
    {
        public virtual string ConvertRelativeToAbsolute(string link, string basePath)
        {
            var baseUrl = new Uri(basePath);
            var url = new Uri(baseUrl, link).ToString();

            return url;
        }

        public string GetRootUrl(string url)
        {
            var basePath = $"{new Uri(url).GetLeftPart(UriPartial.Authority)}/";

            return basePath;
        }

        public string GetDefaultSitemap(string url)
        {
            var basePath = new Uri(url).GetLeftPart(UriPartial.Authority);
            var baseUrl = new Uri(basePath);
            var defaultSitemapUri = new Uri(baseUrl, "sitemap.xml").ToString();

            return defaultSitemapUri;
        }
    }
}