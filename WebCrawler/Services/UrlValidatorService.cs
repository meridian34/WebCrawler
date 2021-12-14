using System;

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
                "#"
            };
        
        public virtual bool IsValidLink(string url)
        {
            foreach(var extension in _notValidWebExtension)
            {
                if (url.Contains(extension))
                {
                    return false;
                }
            }

            return true;
        }

        public virtual bool IsCorrectLink(string link)
        {
            var isAbsoluteLink = Uri.TryCreate(link, UriKind.Absolute, out Uri uriResult);
            if (!isAbsoluteLink)
            {
                return false;
            }

            var containsSheme = link.Contains(Uri.UriSchemeHttp) || link.Contains(Uri.UriSchemeHttps);
            if (!containsSheme)
            {
                return false;
            }

            return true;
        }

        public virtual bool ContainsBaseUrl(string url, string baseUrl)
        {
            return url.Contains(baseUrl);
        }
    }
}
