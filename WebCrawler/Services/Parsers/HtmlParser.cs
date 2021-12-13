using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Services.Parsers
{
    public class HtmlParser
    {
        public virtual IEnumerable<string> GetHtmlLinks(string html)
        {
            var linkList = new List<string>();

            if (string.IsNullOrEmpty(html))
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

                if (startPositionTag == -1)
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
