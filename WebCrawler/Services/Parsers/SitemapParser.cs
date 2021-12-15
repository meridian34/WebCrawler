using System.Collections.Generic;
using System.Linq;

namespace WebCrawler.Services.Parsers
{
    public class SitemapParser
    {
        private readonly UrlValidatorService _urlValidatorService;

        public SitemapParser(UrlValidatorService urlValidatorService)
        {
            _urlValidatorService = urlValidatorService;
        }

        public virtual IEnumerable<string> GetSitemapLinks(string data)
        {
            var linkList = new List<string>();
            if (data == null)
            {
                return linkList;
            }

            var startTag = "<loc>";
            var endTag = "</loc>";
            var position = 0;

            while (position < data.Length)
            {
                var startIndexTag = data.IndexOf(startTag, position);
                if (startIndexTag == -1)
                {
                    break;
                }

                var endIndexTag = data.IndexOf(endTag, startIndexTag + startTag.Length);
                if (endIndexTag == -1)
                {
                    break;
                }

                var link = data.Substring(startIndexTag + startTag.Length, endIndexTag - (startIndexTag + startTag.Length));
                if (_urlValidatorService.UrlIsValid(link))
                {
                    linkList.Add(link);
                }
                
                position = endIndexTag + endTag.Length;
            }

            return linkList;
        }
    }
}
