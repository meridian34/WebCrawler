using System.Threading.Tasks;
using WebCrawler.Services.Services;
using WebCrawler.WebApi.Models;

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
            var result = await _processingService.GetTestsPageAsync(pageNumber, pageSize);
            var response = new TestPageResponse() { TestsPage = _mapper.MapModelToTestsDto(result) };

            return response;
        }

        public async Task StartTest(string inputUrl)
        {
            await _processingService.StartCrawlingSiteAsync(inputUrl);
        }
    }
}
