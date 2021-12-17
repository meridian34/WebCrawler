using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WebCrawler.Models;

namespace WebCrawler.Services
{
    public class WebRequestService
    {
        private readonly HttpClientService _httpClientService;
        public WebRequestService(HttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        private async Task<PerfomanceData> GetElapsedTimeAsync(Uri url)
        {
            var sw = new Stopwatch();
            sw.Start();
            await _httpClientService.GetAsync(url);
            sw.Stop();

            return new PerfomanceData { ElapsedMilliseconds = (int)sw.ElapsedMilliseconds, Url = url };
        }

        public virtual async Task<IEnumerable<PerfomanceData>> GetElapsedTimeForLinksAsync(IEnumerable<Link> links)
        {
            var results = new List<PerfomanceData>();

            foreach(var link in links)
            {
                var perfomanceItem = await GetElapsedTimeAsync(link.Url);
                results.Add(perfomanceItem);
            }

            return results;
        }

        public virtual async Task<string> DownloadAsync(Uri url)
        {
            var responseBody = string.Empty;
            var sw = new Stopwatch();
            sw.Start();
            var response = await _httpClientService.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                responseBody = await response.Content.ReadAsStringAsync();
            }
            sw.Stop();

            return responseBody;
        }
    }
    
}
