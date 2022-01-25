using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.EntityFramework.Entities;
using WebCrawler.Services.Models;

namespace WebCrawler.Services.Services
{
    public class DataStorageService
    {
        private readonly IRepository<TestEntity> _testRepository;
        private readonly IRepository<LinkEntity> _linkRepository;

        public DataStorageService(IRepository<TestEntity> testRepository, IRepository<LinkEntity> linkRepository)
        {
            _testRepository = testRepository;
            _linkRepository = linkRepository;
        }

        /// <returns>returns the Id of the created record (Entity.Test)</returns>
        public virtual async Task<int> SaveAsync(string userUrl, IEnumerable<WebCrawler.Models.Link> links, IEnumerable<WebCrawler.Models.PerfomanceData> perfomances)
        {
            var colectionForSave = CreateSavedCollection(links, perfomances);
            return await SaveTestResultAsync(userUrl, colectionForSave);
        }

        public virtual async Task<TestsPage> GetTestsByPageAsync(int pageNumber, int pageSize)
        {
            var page = new TestsPage() { CurrentPage = pageNumber, ItemsCount = pageSize, Tests = new List<Test>() };
            var query = _testRepository.GetAll().OrderByDescending(x => x.TestDateTime);
            var dbPage = await _testRepository.GetPageAsync(query, pageNumber, pageSize);
            page.TotalPages = (int)Math.Ceiling( dbPage.TotalCount / (decimal)pageSize);
            page.Tests = dbPage.Result.Select(x => new Test
            {
                Id = x.Id,
                TestDateTime = x.TestDateTime,
                UserLink = x.UserLink
            });

            return page;
        }

        public virtual async Task<LinksPage> GetLinksPageByTestIdAsync(int testId)
        {
            var page = new LinksPage();
            page.Url = (await _testRepository.GetByIdAsync(testId)).UserLink;

            var links = _linkRepository
               .GetAllAsNoTracking()
               .Where(x => x.TestId == testId)
               .OrderBy(x => x.ElapsedMilliseconds);

            page.Links = links
               .Select(x => new Link
               {
                   ElapsedMilliseconds = x.ElapsedMilliseconds,
                   FromHtml = x.FromHtml,
                   FromSitemap = x.FromSitemap,
                   Id = x.Id,
                   Url = x.Url
               });

            return page;
        }

        private async Task<int> SaveTestResultAsync(string userUrl, IEnumerable<LinkEntity> links)
        {
            var test = new TestEntity
            {
                UserLink = userUrl,
                TestDateTime = DateTimeOffset.Now,
                Links = links.ToList()
            };
            await _testRepository.AddAsync(test);
            await _testRepository.SaveChangesAsync();

            return test.Id;
        }

        private IEnumerable<LinkEntity> CreateSavedCollection(IEnumerable<WebCrawler.Models.Link> links, IEnumerable<WebCrawler.Models.PerfomanceData> perfomances)
        {
            var listForSave = new List<LinkEntity>();

            foreach (var item in links)
            {
                var link = new LinkEntity();
                link.Url = item.Url.AbsoluteUri;
                link.FromHtml = item.FromHtml;
                link.FromSitemap = item.FromSitemap;
                var currentItemPerfomance = perfomances.FirstOrDefault(x => x.Url == item.Url);
                link.ElapsedMilliseconds = currentItemPerfomance != null ? currentItemPerfomance.ElapsedMilliseconds : null;
                listForSave.Add(link);
            }

            return listForSave;
        }
    }
}
