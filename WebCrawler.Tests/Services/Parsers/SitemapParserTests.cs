﻿using Moq;
using System;
using WebCrawler.Services;
using WebCrawler.Services.Parsers;
using Xunit;

namespace WebCrawler.Tests.Services.Parsers
{
    public class SitemapParserTests
    {
        private readonly Mock<UrlValidatorService> _urlValidatorService = new Mock<UrlValidatorService>();

        [Fact]
        public void GetSitemapLinks_ValidSitemapXmlData_ShouldReturnLinksList()
        {
            // arrange
            var html = @"<loc>https://www.ukad-group.com/latest-projects/</loc> <loc>https://www.ukad-group.com/</loc>";
            var expectedValue = new Uri[] { new Uri("https://www.ukad-group.com/latest-projects/"), new Uri ("https://www.ukad-group.com/") };
            _urlValidatorService.Setup(x => x.UrlIsValid(It.IsAny<Uri>())).Returns(true);

            var service = new SitemapParser(_urlValidatorService.Object);

            //act
            var result = service.GetSitemapLinks(html);

            //assert
            Assert.Equal(result, expectedValue);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(@"<loc>https://www.ukad-group.com/latest-projects/?param1=24</loc>")]
        [InlineData(@"<loc>https://www.ukad-group.com/latest-projects/#point1</loc>")]
        [InlineData(@"<loc>mailto:hi@ukad-group.com</loc>")]
        public void GetSitemapLinks_NotValidSitemapXmlData_ShouldReturnEmptyLinksList(string html)
        {
            // arrange
            _urlValidatorService.SetupSequence(x => x.UrlIsValid(It.IsAny<Uri>())).Returns(false);
            var expected = new Uri[] { };

            var service = new SitemapParser(_urlValidatorService.Object);

            //act
            var result = service.GetSitemapLinks(html);

            //assert
            Assert.Equal(result, expected);
        }
    }
}