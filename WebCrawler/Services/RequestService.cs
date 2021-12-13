using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using WebCrawler.Models;

namespace WebCrawler.Services
{
    public class RequestService
    {
        private readonly IEnumerable<string> _targetContentTypes = new[] { "text/html; charset=utf-8", "text/html", "text/xml", "application/xml", "text/xml; charset=UTF-8" };
       
        public PerfomanceData ScanUrlAsync(string url, out string page)
        {
            var timer = new Stopwatch();
            var result = new PerfomanceData();
            result.Url = url;
            page = null;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                timer.Start();
                var response = (HttpWebResponse)(request.GetResponse());
                timer.Stop();
                result.ElapsedMilliseconds = (int)timer.ElapsedMilliseconds;

                using var receiveStream = response.GetResponseStream();
                using var readStream = new StreamReader(receiveStream, Encoding.UTF8);
                page = readStream.ReadToEnd();
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
