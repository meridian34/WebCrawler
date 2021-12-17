using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebCrawler.Services
{
    public class HttpClientService
    {
        public virtual async Task<HttpResponseMessage> GetAsync(Uri url)
        {
            var client = new HttpClient();
            return await client.GetAsync(url);
        }
    }
}
