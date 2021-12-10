using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Models;

namespace WebCrawler.Services.New
{
    public class RequestService
    {
        private readonly IReadOnlyCollection<string> _contentTypesToHtml = new[] { "text/html; charset=utf-8", "text/html" };
        private readonly IReadOnlyCollection<string> _contentTypesToXml = new[] { "text/xml", "application/xml", "text/xml; charset=UTF-8" };
        private readonly IReadOnlyCollection<string> _targetContentTypes;
       
        public CrawlResult ScanUrlAsync(string url, out string page)
        {
            var timer = new Stopwatch();
            var result = new CrawlResult();
            result.Url = url;
            page = null;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                timer.Start();
                var response = (HttpWebResponse)(request.GetResponse());
                timer.Stop();
                result.ElapsedMilliseconds = (int)timer.ElapsedMilliseconds;

                if (_targetContentTypes.Contains(response.ContentType))
                {
                    using Stream receiveStream = response.GetResponseStream();
                    using StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    page = readStream.ReadToEnd();
                }
            }
            catch (WebException)
            {
                timer.Stop();
                result.ElapsedMilliseconds = (int)timer.ElapsedMilliseconds;
            }

            return result;
        }
    }
    
}
