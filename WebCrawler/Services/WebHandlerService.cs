using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Models;

namespace WebCrawler.Services
{
    public class WebHandlerService
    {
        private readonly IReadOnlyCollection<string> _contentTypesToHtml = new[] { "text/html; charset=utf-8", "text/html" };
        private readonly IReadOnlyCollection<string> _contentTypesToXml = new[] { "text/xml", "application/xml", "text/xml; charset=UTF-8" };
        private readonly IReadOnlyCollection<string> _targetContentTypes;
        private readonly int _maxConcarency;
        private readonly int _dalayMilliseconds;

        public WebHandlerService(WebHandlerType webHandlerType, int maxConcarency = 4, int dalayMilliseconds = 50)
        {
            _targetContentTypes = webHandlerType == WebHandlerType.SiteMap ? _contentTypesToXml : _contentTypesToHtml;
            _maxConcarency = maxConcarency > 1 ? maxConcarency : 1;
            _dalayMilliseconds = dalayMilliseconds;
        }

        public virtual async Task<HttpScanResult> ScanUrlAsync(string url)
        {
            var result = new HttpScanResult();
            result.Url = url;

            return await ScanUrlAsync(result);
        }

        private async Task<HttpScanResult> ScanUrlAsync(HttpScanResult result)
        {   
            var timer = new Stopwatch();
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(result.Url);
                timer.Start();
                var response = (HttpWebResponse)(await request.GetResponseAsync());
                timer.Stop();
                result.HttpStausCode = (int)response.StatusCode;
                result.HttpContentType = response.ContentType;
                result.ElapsedMilliseconds = timer.ElapsedMilliseconds;

                if (_targetContentTypes.Contains(response.ContentType))
                {
                    using Stream receiveStream = response.GetResponseStream();
                    using StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    result.Content = readStream.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                timer.Stop();
                result.Exception = e;
                result.ElapsedMilliseconds = timer.ElapsedMilliseconds;
            }

            result.IsCrawled = true;

            return result;
        }

        public virtual async Task<IReadOnlyCollection<HttpScanResult>> ScanUrlConcurencyAsync(IReadOnlyCollection<HttpScanResult> urls)
        {
            var tasks = new List<Task<HttpScanResult>>();
            var queue = new Queue<HttpScanResult>(urls);

            while (queue.Count > 0)
            {
                await Task.Delay(_dalayMilliseconds);
                for (var i = 0; i < _maxConcarency; i++)
                {
                    if (queue.Count > 0)
                    {
                        var item = queue.Dequeue();
                        tasks.Add(Task.Run(async () =>
                        {   
                            var result = await ScanUrlAsync(item);

                            return result;
                        }));
                    }
                }

                await Task.WhenAll(tasks);
            }

            return tasks.Where(t => t.IsFaulted == false)
                        .Select(x => x.Result)
                        .ToList();
        }
    }
}
