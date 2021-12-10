using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Services
{
    public class ParserService
    {
        public virtual IEnumerable<string> GetLinks(string data)
        {
            
            var linkList = new List<string>();
            var tags = GetTags();
            foreach(var tag in tags)
            {
                var position = 0;
                while (position < data.Length)
                {
                    var startIndexTag = data.IndexOf(tag.Key, position);
                    if(startIndexTag == -1)
                    {
                        break;
                    }

                    var endIndexTag = data.IndexOf(tag.Value, startIndexTag + tag.Key.Length);
                    if(endIndexTag == -1)
                    {
                        break;
                    }

                    linkList.Add(data.Substring(startIndexTag + tag.Key.Length, endIndexTag  - (startIndexTag + tag.Key.Length)));
                    position = endIndexTag + tag.Value.Length;
                }
            }
            return linkList;
        }

        private Dictionary<string,string> GetTags()
        {
            return new Dictionary<string, string>
            {
                {@"href=""", @""""},
                {"<loc>",  "</loc>"}
            };
        }
    }
}
