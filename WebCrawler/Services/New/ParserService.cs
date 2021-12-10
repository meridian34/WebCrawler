using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Services
{
    public class ParserService
    {
        public virtual IEnumerable<string> GetSitemapLinks(string data)
        {
            var linkList = new List<string>();
            if(data == null)
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
        public virtual IEnumerable<string> GetHtmlLinks(string html)
        {
            var linkList = new List<string>();

            if (html == null)
            {
                return linkList;
            }

            var startATag = "<a";
            var endTag = ">";
            var href = @"href=""";
            var endHref = @"""";
            var position = 0;

            while (position < html.Length)
            {
                var startPositionTag = html.IndexOf(startATag, position);

                if(startPositionTag == -1)
                {
                    break;
                }

                var endPositionTag = html.IndexOf(endTag, startPositionTag);
                var tagBody = html.Substring(startPositionTag, endPositionTag - startPositionTag);
                var positionHref = tagBody.IndexOf(href, 0);

                if (positionHref > -1)
                {
                    var startLink = positionHref + href.Length;
                    var endLink = tagBody.IndexOf(endHref, startLink);
                    var link = tagBody.Substring(startLink, endLink - startLink);

                    linkList.Add(link);
                }

                position = endPositionTag + endTag.Length;
            }

            return linkList;
        }
    }
}
