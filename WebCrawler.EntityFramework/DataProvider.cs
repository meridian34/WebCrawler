using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.EntityFramework.Entities;

namespace WebCrawler.EntityFramework
{
    public class DataProvider 
    { 
        private readonly IRepository<Test> _testRepository;
        private readonly IRepository<Link> _linkRepository;
        public DataProvider(IRepository<Test> testRepository, IRepository<Link> linkRepository)
        {
            _testRepository = testRepository;
            _linkRepository = linkRepository;
        }

        public virtual async Task SaveTestResultAsync(string userUrl, IEnumerable<Link> links)
        {

            await _testRepository.AddAsync(new Test
            {
                UserLink = userUrl,
                TestDateTime = DateTimeOffset.Now,
                Links = links.ToList()
            });
            await _testRepository.SaveChangesAsync();
        }

        public virtual IEnumerable<Test> GetTestsByPage(int page, int countItems)
        {
            return _testRepository
                .GetAll()         
                .Skip((page - 1) * countItems)
                .Take(countItems)
                .OrderByDescending(x => x.TestDateTime);
        }

        public virtual IEnumerable<Link> GetLinksByPage(int testId, int page, int countItems)
        {
            return _linkRepository
                .GetAll()
                .Where(x=>x.TestId == testId)
                .Skip((page - 1) * countItems)
                .Take(countItems)
                .OrderBy(x => x.ElapsedMilliseconds);
        }

        public virtual IEnumerable<Test> GetTestsByUrl(string url)
        {
            return _testRepository
                .Include(x => x.Links)
                .Where(x => x.UserLink.Contains(url))
                .OrderByDescending(x => x.TestDateTime);
        }
    }
}
