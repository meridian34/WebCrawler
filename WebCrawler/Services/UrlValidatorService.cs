using System;
using System.Linq;

namespace WebCrawler.Services
{
    public class UrlValidatorService
    {
        private readonly string[] _notValidWebExtension =
            new[]
            {
                ".png",
                ".jpeg",
                ".jpg",
                ".ico",
                ".woff",
                ".woff2",
                ".js",
                ".css",
                ".json",
                ".git",
                ".ttf",                
                "@",
                "?",
                "#",
                "%"
            };
        
        public virtual bool LinkIsValid(string url)
        {
            return !_notValidWebExtension.Any(x => url.Contains(x));
        }

        public virtual bool UrlIsValid(string url)
        {
            var isAbsoluteLink = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult);
            if (!isAbsoluteLink)
            {
                return false;
            }

            var containsSheme = url.Contains(Uri.UriSchemeHttp) || url.Contains(Uri.UriSchemeHttps);
            if (!containsSheme)
            {
                return false;
            }

            return LinkIsValid(url);
        }

        public virtual bool ContainsBaseUrl(string url, string baseUrl)
        {
            return url.Contains(baseUrl);
        }
    }
}
