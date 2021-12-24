using System;
using System.Data;
using System.Threading.Tasks;
using WebCrawler.ConsoleApplication.Services;
using WebCrawler.Logic.Services;
using WebCrawler.Models;
using WebCrawler.Services;
using WebCrawler.Services.Crawlers;
using WebCrawler.Services.Parsers;
using Xunit;
using Moq;

namespace WebCrawler.ConsoleApplication.Tests
{
    public class CrawlerServiceTests
    {
        [Fact(Timeout = 1000)]
        public async Task RunAsync_ReadLineConsoleService_ShouldPrintCrawlingResultsAsync()
        {
            // arrange
            var requestService = new Mock<WebRequestService>(new Mock<HttpClientService>().Object);
            var urlValidatorService = new Mock<UrlValidatorService>();
            var linkConvertorService = new Mock<LinkConvertorService>();
            var htmlParser = new Mock<HtmlParser>(urlValidatorService.Object, linkConvertorService.Object);
            var htmlCrawler = new Mock<HtmlCrawler>(requestService.Object, htmlParser.Object, linkConvertorService.Object);
            var sitemapParser = new Mock<SitemapParser>(urlValidatorService.Object);
            var sitemapCrawler = new Mock<SitemapCrawler>(requestService.Object, sitemapParser.Object, linkConvertorService.Object);
            var consoleService = new Mock<ConsoleService>();
            var storage = new Mock<DataStorageService>(new Mock<IRepository<EntityFramework.Entities.Test>>().Object);
            
            var webCrawler = new Mock<WebCrawlerService>(htmlCrawler.Object, sitemapCrawler.Object, urlValidatorService.Object, requestService.Object);

            var firstUri  = new Uri("https://www.example.com/");
            var secondUri = new Uri("https://www.example.com/latest/");
            var thirdUri = new Uri("https://www.example.com/projects/");
            var crawlerLinkCollection = new Link[]
            {
            new Link { FromHtml = true, FromSitemap = true, Url = firstUri },
            new Link { FromSitemap = true, Url = secondUri },
            new Link {FromHtml = true, Url = thirdUri}
            };
            var crawlerPerfomanceCollection = new PerfomanceData[]
            {
                new PerfomanceData { ElapsedMilliseconds = 50, Url = firstUri },
                new PerfomanceData { ElapsedMilliseconds = 50, Url = secondUri },
                new PerfomanceData { ElapsedMilliseconds = 50, Url = thirdUri }
            };

            webCrawler.Setup(x => x.GetLinksAsync(It.Is<Uri>(s=>s==firstUri))).ReturnsAsync(crawlerLinkCollection);
            consoleService.Setup(x => x.ReadLine()).Returns(firstUri.OriginalString);
            webCrawler.Setup(x => x.GetPerfomanceDataCollectionAsync(It.IsAny<Link[]>())).ReturnsAsync(crawlerPerfomanceCollection);

            var crawler = new CrawlerService(consoleService.Object, webCrawler.Object, storage.Object);

            //act
            await crawler.RunAsync();

            //assert
            consoleService.Verify(x => x.WriteLine(It.Is<string>(s => s == "Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site")));
            consoleService.Verify(x => x.WriteLine(It.Is<string>(s => s == secondUri.OriginalString)));
            consoleService.Verify(x => x.WriteLine(It.Is<string>(s => s == "Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml")));
            consoleService.Verify(x => x.WriteLine(It.Is<string>(s => s == thirdUri.OriginalString)));
            consoleService.Verify(x => x.WriteLine(It.Is<string>(s => s == "Urls(html documents) found after crawling a website: 2")));
            consoleService.Verify(x => x.WriteLine(It.Is<string>(s => s == "Urls found in sitemap: 2")));
        }

        [Fact(Timeout =1000)]
        public async Task RunAsync_ReadLineConsoleService_ShouldPushInDataStorageServiceAsync()
        {
            // arrange
            var requestService = new Mock<WebRequestService>(new Mock<HttpClientService>().Object);
            var urlValidatorService = new Mock<UrlValidatorService>();
            var linkConvertorService = new Mock<LinkConvertorService>();
            var htmlParser = new Mock<HtmlParser>(urlValidatorService.Object, linkConvertorService.Object);
            var htmlCrawler = new Mock<HtmlCrawler>(requestService.Object, htmlParser.Object, linkConvertorService.Object);
            var sitemapParser = new Mock<SitemapParser>(urlValidatorService.Object);
            var sitemapCrawler = new Mock<SitemapCrawler>(requestService.Object, sitemapParser.Object, linkConvertorService.Object);
            var consoleService = new Mock<ConsoleService>();
            var storage = new Mock<DataStorageService>(new Mock<IRepository<EntityFramework.Entities.Test>>().Object);

            var webCrawler = new Mock<WebCrawlerService>(htmlCrawler.Object, sitemapCrawler.Object, urlValidatorService.Object, requestService.Object);

            var firstUri = new Uri("https://www.example.com/");
            var secondUri = new Uri("https://www.example.com/latest/");
            var thirdUri = new Uri("https://www.example.com/projects/");
            var crawlerLinkCollection = new Link[]
            {
            new Link { FromHtml = true, FromSitemap = true, Url = firstUri },
            new Link { FromSitemap = true, Url = secondUri },
            new Link {FromHtml = true, Url = thirdUri}
            };
            var crawlerPerfomanceCollection = new PerfomanceData[]
            {
                new PerfomanceData { ElapsedMilliseconds = 50, Url = firstUri },
                new PerfomanceData { ElapsedMilliseconds = 50, Url = secondUri },
                new PerfomanceData { ElapsedMilliseconds = 50, Url = thirdUri }
            };

            webCrawler.Setup(x => x.GetLinksAsync(It.Is<Uri>(s => s == firstUri))).ReturnsAsync(crawlerLinkCollection);
            consoleService.Setup(x => x.ReadLine()).Returns(firstUri.OriginalString);
            webCrawler.Setup(x => x.GetPerfomanceDataCollectionAsync(It.IsAny<Link[]>())).ReturnsAsync(crawlerPerfomanceCollection);

            var crawler = new CrawlerService(consoleService.Object, webCrawler.Object, storage.Object);

            //act
            await crawler.RunAsync();

            //assert
            storage.Verify(x => x.SaveAsync(It.Is<string>(s => s == firstUri.OriginalString), It.IsAny<Link[]>(), It.IsAny<PerfomanceData[]>()));
        }
    }
}
