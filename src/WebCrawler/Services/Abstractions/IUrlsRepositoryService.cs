using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebCrawler.Services.Abstractions
{
    public interface IUrlsRepositoryService
    {
        public int Count { get; }

        public Task InitRootAsync(string url);

        public Task AddLinkAsync(string url);

        public Task AddLinkAsync(IEnumerable<string> urls);

        public Task<string> GetUniqueUrlAsync();

        public Task<IReadOnlyCollection<string>> GetListUniqueUrlsAsync();
    }
}
