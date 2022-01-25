using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xunit;
using WebCrawler.Models;
using System.Threading;
using WebCrawler.Services.Services;
using WebCrawler.EntityFramework.Entities;
using System.Linq;

namespace WebCrawler.Services.Tests
{
    public class DataStorageServiceTests
    {
        [Fact(Timeout = 1000)]
        public async Task SaveAsync_WebCrawlerResults_ShouldConvrtrAndTransferToRepository()
        {
            // arrange
            var testRepository = new Mock<IRepository<TestEntity>>();
            var linkRepository = new Mock<IRepository<LinkEntity>>();
            var storage = new DataStorageService(testRepository.Object, linkRepository.Object);
            var inputUrl = "https://www.example.com/";
            var inputLinkCollection = new Link[] { new Link { Url = new Uri(inputUrl), FromHtml = true, FromSitemap = true } };
            var inputPerfomanceCollection = new PerfomanceData[] { new PerfomanceData { Url = new Uri(inputUrl), ElapsedMilliseconds = 200 } };
            var testDateTime = DateTimeOffset.Now;
            var expectedLinkList = new List<LinkEntity>()
            {
                new LinkEntity
                {
                    Url = inputUrl,
                    FromHtml = true,
                    FromSitemap = true,
                    ElapsedMilliseconds = 200
                }
            };
            var expectedResult = new TestEntity { TestDateTime = testDateTime, UserLink = inputUrl, Links = expectedLinkList };

            //act
            await storage.SaveAsync(inputUrl, inputLinkCollection, inputPerfomanceCollection);

            //assert
            testRepository.Verify(x => x.AddAsync(It.Is<TestEntity>(t =>
                t.UserLink == inputUrl
                && t.Links.Count == expectedLinkList.Count), It.IsAny<CancellationToken>()));
            testRepository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }

        [Fact(Timeout = 1000)]
        public async Task GetLinksPageByTestIdAsync_TestId_ShouldReturnLinksPage()
        {
            // arrange
            var testRepository = new Mock<IRepository<TestEntity>>();
            var linkRepository = new Mock<IRepository<LinkEntity>>();
            var inputId = 100;
            var expectedResultCount = 1;
            var expectedLink = "www.example.com";

            var item1 = new TestEntity { UserLink = expectedLink };
            var listItems = new List<LinkEntity> { new LinkEntity { TestId = inputId, ElapsedMilliseconds = 50 } };

            testRepository.SetupSequence(x => x.GetByIdAsync(It.Is<int>(x => x == inputId), default)).ReturnsAsync(item1);

            linkRepository.Setup(x => x.GetAllAsNoTracking()).Returns(listItems.AsQueryable());
            

            var storage = new DataStorageService(testRepository.Object, linkRepository.Object);

            // act 
            var result = await storage.GetLinksPageByTestIdAsync(100);

            //assert
            Assert.Equal(expectedLink, result.Url);
            Assert.Equal(expectedResultCount, result.Links.Count());
        }
    }
}
