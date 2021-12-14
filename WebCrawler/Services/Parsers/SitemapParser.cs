using System.Collections.Generic;

namespace WebCrawler.Services.Parsers
{
    public class SitemapParser
    {
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

                linkList.Add(data.Substring(startIndexTag + startTag.Length, endIndexTag - (startIndexTag + startTag.Length)));
                position = endIndexTag + endTag.Length;
            }

            return linkList;
        }
    }
}
