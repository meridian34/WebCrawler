using Moq;
using System;
using System.Threading.Tasks;
using WebCrawler.ConsoleApplication.Services;
using WebCrawler.Models;
using WebCrawler.Services;
using WebCrawler.Services.Crawlers;
using WebCrawler.Services.Parsers;
using Xunit;

namespace WebCrawler.ConsoleApplication.Tests
{
    public class CrawlerServiceTests
    {
        [Fact]
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

            
            var webCrawler = new Mock<WebCrawlerService>(htmlCrawler.Object, sitemapCrawler.Object, urlValidatorService.Object, requestService.Object);

            var firstUri  = new Uri("https://www.ukad-group.com/");
            var secondUri = new Uri("https://www.ukad-group.com/latest/");
            var thirdUri = new Uri("https://www.ukad-group.com/projects/");
            var crawlerLinkCollection = new Link[]
            {
            new Link { IsCrawler = true, IsSitemap = true, Url = firstUri },
            new Link { IsSitemap = true, Url = secondUri },
            new Link {IsCrawler = true, Url = thirdUri}
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

            var crawler = new CrawlerService(consoleService.Object, webCrawler.Object);

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
    }
}
