using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.EntityFramework.Entities;

namespace WebCrawler.Logic.Providers
{
    public class DataProvider
    {
        private readonly IRepository<Test> _testRepository;
        
        public DataProvider(IRepository<Test> testRepository)
        {
            _testRepository = testRepository;
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
    }
}
