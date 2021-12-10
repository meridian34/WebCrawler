using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Services.New
{
    public class LinkConverterService
    {
        public virtual string ConvertRelativeToAbsolute(string link, string basePath)
        {
            var baseUrl = new Uri(basePath);
            var url = new Uri(baseUrl, link).ToString();
            return url;
        }
    }
}
