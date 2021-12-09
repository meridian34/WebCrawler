using System;
using System.Linq;
using WebCrawler.Services;
using Xunit;

namespace WebCrawler.Tests
{
    public class HtmlDocumentServiceTests
    {
        private readonly HtmlDocumentService _service = new HtmlDocumentService();
        
        [Theory]
        [InlineData("")]
        [InlineData(@" <a href=""/latest-projects/"" class=""navigation - new__link"" title=""PORTFOLIO""></a>", "/latest-projects/")]
        [InlineData(@"<a href=""/latest-projects/""></a> <a href=""/company/about-us/"" ></a>", "/latest-projects/", "/company/about-us/")]
        [InlineData(@"<link href=""/bundles/all.css?v=NmSNb6C7uOf2wrP1bmEYwyF9Ny5FWIRON_d9xDAtIw41"" rel=""preload"" as=""style"" media=""screen"">")]
        [InlineData(@" <a href=""/latest-projects-param/?param1=24""></a>")]
        [InlineData(@" <a href=""/latest-projects-point/#point1"" ></a>", "/latest-projects-point/")]
        [InlineData(@" <a href=""mailto:hi@ukad-group.com"" ></a>", "mailto:hi@ukad-group.com")]
        public void GetLinks_HtmlData_ShouldReturnLinksList(string html, params string[] expectedValue)
        {
            // arrange
            
            //act
            var result = _service.GetLinks(html).ToArray();
            
            //assert
            
            Assert.Equal(result, expectedValue);
        }
        
        [Fact]
        public void GetLinks_Null_ShouldReturnNullReferenceException()
        {
            // arrange
            string html = null;
            var expectedMessage = "Object reference not set to an instance of an object.";

            //act
            var res = Assert.Throws<NullReferenceException>(() => _service.GetLinks(html));

            //assert

            Assert.Equal(res.Message, expectedMessage);
        }

    }
}
