using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.EntityFramework.Entities;
using WebCrawler.Logic.Providers;

namespace WebCrawler.Logic.Services
{
    public class DataStorageService
    {
        private readonly DataProvider _provider;

        public DataStorageService(DataProvider provider)
        {
            _provider = provider;
        }

        public virtual async Task SaveAsync(string userUrl, IEnumerable<Models.Link> links, IEnumerable<Models.PerfomanceData> perfomances)
        {
            var colectionForSave = CreateSavedCollection(links, perfomances);
            await _provider.SaveTestResultAsync(userUrl, colectionForSave);
        }

        private IEnumerable<Link> CreateSavedCollection(IEnumerable<Models.Link> links, IEnumerable<Models.PerfomanceData> perfomances)
        {
            var listForSave = new List<Link>();

            foreach (var item in links)
            {
                var buff = new Link();
                buff.Url = item.Url.AbsoluteUri;
                buff.FromHtml = item.FromHtml;
                buff.FromSitemap = item.FromSitemap;
                var currentItemPerfomance = perfomances.FirstOrDefault(x => x.Url == item.Url);
                buff.ElapsedMilliseconds = currentItemPerfomance != null ? currentItemPerfomance.ElapsedMilliseconds : null;
                listForSave.Add(buff);
            }

            return listForSave;
        }
    }
}
