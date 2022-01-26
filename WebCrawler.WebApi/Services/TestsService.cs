using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Services.Services;
using WebCrawler.WebApi.Exceptions;
using WebCrawler.WebApi.Requests;
using WebCrawler.WebApi.Responses;

namespace WebCrawler.WebApi.Services
{
    public class TestsService
    {
        private readonly DataProcessingService _processingService;
        private readonly MapperService _mapper;

        public TestsService(DataProcessingService processingService, MapperService mapper)
        {
            _processingService = processingService;
            _mapper = mapper;
        }

        public async Task<TestPageResponse> GetTestsAsync(int pageNumber, int pageSize)
        {
            if(pageNumber <= 0 || pageSize <= 0)
            {
                throw new ValidationException("Params 'pageNumber' must be > 0 and 'pageSize' must be > 0 ");
            }

            var result = await _processingService.GetTestsPageAsync(pageNumber, pageSize);
            var respose = new TestPageResponse() { TestsPage = _mapper.MapModelToTestsDto(result) };

            return respose;
        }

        public async Task StartTest(string inputUrl)
        {
            Uri url;
            var urlCreated = Uri.TryCreate(inputUrl, UriKind.Absolute, out url);
            if (!urlCreated)
            {
                throw new ValidationException("Invalid URL");
            }

            await _processingService.StartCrawlingSiteAsync(url);
        }
    }
}
