using System.Threading.Tasks;
using WebCrawler.Services.Services;
using WebCrawler.WebApi.Models;

namespace WebCrawler.WebApi.Services
{
    public class TestDetailsService
    {
        private readonly DataProcessingService _processingService;
        private readonly MapperService _mapper;

        public TestDetailsService(DataProcessingService processingService, MapperService mapper)
        {
            _processingService = processingService;
            _mapper = mapper;
        }

        public async Task<LinkPageResponse> GetByTestIdAsync(int testId)
        {
            var linkPage = await _processingService.GetLinksPageAsync(testId);
            var mapLinkPage = _mapper.MapModelToDetailsDto(linkPage);
            var response = new LinkPageResponse { DetailsPage = mapLinkPage };

            return response;
        }
    }
}
