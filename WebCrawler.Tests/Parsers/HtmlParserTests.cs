using System;
using System.Linq;
using WebCrawler.Services.Parsers;
using Xunit;

namespace WebCrawler.Tests
{
    public class HtmlParserTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(@" <a href=""/latest-projects/"" class=""navigation - new__link"" title=""PORTFOLIO""></a>", "/latest-projects/")]
        [InlineData(@"<a href=""/latest-projects/""></a> <a href=""/company/about-us/"" ></a>", "/latest-projects/", "/company/about-us/")]
        [InlineData(@"<link href=""/bundles/all.css?v=NmSNb6C7uOf2wrP1bmEYwyF9Ny5FWIRON_d9xDAtIw41"" rel=""preload"" as=""style"" media=""screen"">")]
        [InlineData(@" <a href=""/latest-projects-param/?param1=24""></a>", "/latest-projects-param/?param1=24")]
        [InlineData(@" <a href=""/latest-projects-point/#point1"" ></a>", "/latest-projects-point/#point1")]
        [InlineData(@" <a href=""mailto:hi@ukad-group.com"" ></a>", "mailto:hi@ukad-group.com")]
        public void GetHtmlLinks_HtmlData_ShouldReturnLinksList(string html, params string[] expectedValue)
        {
            // arrange
            var service = new HtmlParser();

            //act
            var result = service.GetHtmlLinks(html).ToArray();
            
            //assert
            Assert.Equal(result, expectedValue);
        }
    }
}
