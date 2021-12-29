using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using Xunit;
using WebCrawler.Logic.Services;
using WebCrawler.Models;
using System.Threading;

namespace WebCrawler.Logic.Tests
{
    public class DataStorageServiceTests
    {
        [Fact(Timeout = 1000)]
        public async Task SaveAsync_WebCrawlerResults_ShouldConvrtrAndTransferToRepository()
        {
            // arrange
            var repository = new Mock<IRepository<EntityFramework.Entities.Test>>();
            var storage = new DataStorageService(repository.Object);
            var inputUrl = "https://www.example.com/";
            var inputLinkCollection = new Link[] { new Link { Url = new Uri(inputUrl), FromHtml = true, FromSitemap = true } };
            var inputPerfomanceCollection = new PerfomanceData[] { new PerfomanceData { Url = new Uri(inputUrl), ElapsedMilliseconds = 200 } };
            var testDateTime = DateTimeOffset.Now;
            var expectedLinkList = new List<EntityFramework.Entities.Link>()
            {
                new EntityFramework.Entities.Link
                {
                    Url = inputUrl,
                    FromHtml = true,
                    FromSitemap = true,
                    ElapsedMilliseconds = 200
                }
            };
            var expectedResult = new EntityFramework.Entities.Test { TestDateTime = testDateTime, UserLink = inputUrl, Links = expectedLinkList };

            //act
            await storage.SaveAsync(inputUrl, inputLinkCollection, inputPerfomanceCollection);

            //assert
            repository.Verify(x => x.AddAsync(It.Is<EntityFramework.Entities.Test>(t =>
                t.UserLink == inputUrl
                && t.Links.Count == expectedLinkList.Count), It.IsAny<CancellationToken>()));
            repository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }
    }
}
