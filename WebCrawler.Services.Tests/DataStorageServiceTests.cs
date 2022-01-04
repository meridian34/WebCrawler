using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xunit;
using WebCrawler.Models;
using System.Threading;
using WebCrawler.Services.Services;

namespace WebCrawler.Logic.Tests
{
    public class DataStorageServiceTests
    {
        [Fact(Timeout = 1000)]
        public async Task SaveAsync_WebCrawlerResults_ShouldConvrtrAndTransferToRepository()
        {
            // arrange
            var testRepository = new Mock<IRepository<EntityFramework.Entities.TestEntity>>();
            var linkRepository = new Mock<IRepository<EntityFramework.Entities.LinkEntity>>();
            var storage = new DataStorageService(testRepository.Object, linkRepository.Object);
            var inputUrl = "https://www.example.com/";
            var inputLinkCollection = new Link[] { new Link { Url = new Uri(inputUrl), FromHtml = true, FromSitemap = true } };
            var inputPerfomanceCollection = new PerfomanceData[] { new PerfomanceData { Url = new Uri(inputUrl), ElapsedMilliseconds = 200 } };
            var testDateTime = DateTimeOffset.Now;
            var expectedLinkList = new List<EntityFramework.Entities.LinkEntity>()
            {
                new EntityFramework.Entities.LinkEntity
                {
                    Url = inputUrl,
                    FromHtml = true,
                    FromSitemap = true,
                    ElapsedMilliseconds = 200
                }
            };
            var expectedResult = new EntityFramework.Entities.TestEntity { TestDateTime = testDateTime, UserLink = inputUrl, Links = expectedLinkList };

            //act
            await storage.SaveAsync(inputUrl, inputLinkCollection, inputPerfomanceCollection);

            //assert
            testRepository.Verify(x => x.AddAsync(It.Is<EntityFramework.Entities.TestEntity>(t =>
                t.UserLink == inputUrl
                && t.Links.Count == expectedLinkList.Count), It.IsAny<CancellationToken>()));
            testRepository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }
    }
}
