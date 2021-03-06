using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Models;
using WebCrawler.Services.Abstractions;

namespace WebCrawler.Services
{
    public class WebHandlerService : IWebHandlerService
    {
        private readonly IReadOnlyCollection<string> _contentTypes;
        private readonly int _maxConcarency;
        private readonly int _dalayMilliseconds;

        public WebHandlerService(IReadOnlyCollection<string> contentTypeToLoadBody, int maxConcarency, int dalayMilliseconds)
        {
            _contentTypes = contentTypeToLoadBody;
            _maxConcarency = maxConcarency > 1 ? maxConcarency : 1;
            _dalayMilliseconds = dalayMilliseconds;
        }

        public async Task<HttpScanResult> ScanUrlAsync(string url)
        {
            var result = new HttpScanResult();
            result.Url = url;
            var timer = new Stopwatch();

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                timer.Start();
                var response = (HttpWebResponse)(await request.GetResponseAsync());
                timer.Stop();
                result.HttpStausCode = (int)response.StatusCode;
                result.HttpContentType = response.ContentType;
                result.ElapsedMilliseconds = timer.ElapsedMilliseconds;
                if (_contentTypes.Contains(response.ContentType))
                {
                    using Stream receiveStream = response.GetResponseStream();
                    using StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    result.Content = readStream.ReadToEnd();
                }

                return result;
            }
            catch (Exception e)
            {
                timer.Stop();
                result.Exception = e;
                result.ElapsedMilliseconds = timer.ElapsedMilliseconds;
            }

            return await Task.FromResult(result);
        }

        public async Task<IReadOnlyCollection<HttpScanResult>> ScanUrlConcurencyAsync(IReadOnlyCollection<string> urls)
        {
            try
            {
                var tasks = new List<Task<HttpScanResult>>();
                var queue = new Queue<string>(urls);

                while (queue.Count > 0)
                {
                    for (var i = 0; i < _maxConcarency; i++)
                    {
                        if (queue.Count > 0)
                        {
                            var item = queue.Dequeue();
                            tasks.Add(Task.Run(async () =>
                            {
                                await Task.Delay(_dalayMilliseconds);
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}
