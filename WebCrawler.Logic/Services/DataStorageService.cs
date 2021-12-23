using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.EntityFramework.Entities;

namespace WebCrawler.Logic.Services
{
    public class DataStorageService
    {
        private readonly IRepository<Test> _testRepository;

        public DataStorageService(IRepository<Test> testRepository)
        {
            _testRepository = testRepository;
        }

        public virtual async Task SaveAsync(string userUrl, IEnumerable<Models.Link> links, IEnumerable<Models.PerfomanceData> perfomances)
        {
            var colectionForSave = CreateSavedCollection(links, perfomances);
            await SaveTestResultAsync(userUrl, colectionForSave);
        }

        private async Task SaveTestResultAsync(string userUrl, IEnumerable<Link> links)
        {
            await _testRepository.AddAsync(new Test
            {
                UserLink = userUrl,
                TestDateTime = DateTimeOffset.Now,
                Links = links.ToList()
            });
            await _testRepository.SaveChangesAsync();
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
