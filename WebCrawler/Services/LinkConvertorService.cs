using System;

namespace WebCrawler.Services
{
    public class LinkConvertorService
    {
        public virtual Uri ConvertRelativeToAbsolute(Uri link, Uri basePath)
        {
            var absoluteIsCreated = Uri.TryCreate(basePath, link, out Uri absolute);
            if (!absoluteIsCreated)
            {
                throw new ArgumentException("Failed to convert, check the input parameters!");
            }

            return absolute;
        }

        public virtual Uri GetRootUrl(Uri url)
        {
            var basePath = $"{url.GetLeftPart(UriPartial.Authority)}/";

            return new Uri(basePath);
        }

        public virtual Uri GetDefaultSitemap(Uri url)
        {
            var baseUrl = new Uri(url.GetLeftPart(UriPartial.Authority));
            var defaultSitemapUri = new Uri(baseUrl, "sitemap.xml");

            return defaultSitemapUri;
        }
    }
}
