using Moq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebCrawler.Models;
using WebCrawler.Services;
using Xunit;

namespace WebCrawler.Tests.Services
{
    public class WebRequestServiceTests
    {
        [Fact]
        public async Task DownloadAsync_Url_ShouldReturnStringDataAsync()
        {
            // arrange
            var url = new Uri("https://www.ukad-group.com/");
            var expectedValue = "xml or html data";
            var httpClient = new Mock<HttpClientService>();
            var response = new HttpResponseMessage();
            response.Content = new StringContent(expectedValue);
            httpClient.Setup(x => x.GetAsync(It.IsAny<Uri>())).ReturnsAsync(response);

            var webRequestService = new WebRequestService(httpClient.Object);

            //act
            var data = await webRequestService.DownloadAsync(url);

            //assert
            Assert.Equal(expectedValue, data);
        }

        [Fact]
        public async Task GetElapsedTimeForLinksAsync_Url_ShouldReturnPerfomanceDataAsync()
        {
            // arrange
            var url = new Uri("https://www.ukad-group.com/");
            var inputValue = new Link[] { new Link { Url = url } };

            var expectedValue = new PerfomanceData[] { new PerfomanceData { ElapsedMilliseconds = 50, Url = url } };
            var httpClient = new Mock<HttpClientService>();
            var response = new HttpResponseMessage();
            httpClient.Setup(x => x.GetAsync(It.IsAny<Uri>())).ReturnsAsync(
                () => 
                {
                    Task.Delay(50).GetAwaiter().GetResult();
                    return response;
                });

            var webRequestService = new WebRequestService(httpClient.Object);

            //act
            var data = await webRequestService.GetElapsedTimeForLinksAsync(inputValue);

            //assert
            Assert.InRange<int>(data.First().ElapsedMilliseconds, 50, 500);
        }
    }
}
