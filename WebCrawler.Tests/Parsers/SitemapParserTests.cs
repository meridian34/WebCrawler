using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Services.Parsers;
using Xunit;

namespace WebCrawler.Tests
{
    public class SitemapParserTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(@"<loc>https://www.ukad-group.com/</loc>", "https://www.ukad-group.com/")]
        [InlineData(@"<loc>https://www.ukad-group.com/latest-projects/</loc> <loc>https://www.ukad-group.com/</loc>", "https://www.ukad-group.com/latest-projects/", "https://www.ukad-group.com/")]
        [InlineData(@"<loc>https://www.ukad-group.com/latest-projects/?param1=24</loc>", "https://www.ukad-group.com/latest-projects/?param1=24")]
        [InlineData(@"<loc>https://www.ukad-group.com/latest-projects/#point1</loc>", "https://www.ukad-group.com/latest-projects/#point1")]
        [InlineData(@"<loc>mailto:hi@ukad-group.com</loc>", "mailto:hi@ukad-group.com")]
        public void GetSitemapLinks_SitemapXmlData_ShouldReturnLinksList(string html, params string[] expectedValue)
        {
            // arrange
            var service = new SitemapParser();

            //act
            var result = service.GetSitemapLinks(html).ToArray();

            //assert
            Assert.Equal(result, expectedValue);
        }
    }
}
