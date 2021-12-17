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
        
        public virtual bool LinkIsValid(Uri url)
        {
            return !_notValidWebExtension.Any(x => url.OriginalString.Contains(x));
        }

        public virtual bool UrlIsValid(Uri url)
        {
            
            if (!url.IsAbsoluteUri)
            {
                return false;
            }

            var containsSheme = url.Scheme == Uri.UriSchemeHttp || url.Scheme == Uri.UriSchemeHttps;
            if (!containsSheme)
            {
                return false;
            }

            return true;
        }
    }
}
