using System.Threading.Tasks;
using WebCrawler.Services.Services;
using WebCrawler.WebApi.Exceptions;
using WebCrawler.WebApi.Responses;

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
            _mapper = null;
        }

        public async Task<LinkPageResponse> GetByTestIdAsync(int testId)
        {
            if (testId < 0)
            {
                throw new ValidationException("Input parameter 'testId' must be >= 0");
            }

            var linkPage = await _processingService.GetLinksPageAsync(testId);
            var mapLinkPage = _mapper.MapModelToDetailsDto(linkPage);
            var respose = new LinkPageResponse { DetailsPage = mapLinkPage };
            return respose;
        }

        public async Task<LinkPageResponse> GetDetailsByPageAsync(int testId, int pageNumber, int pageSize)
        {
            if (testId < 0)
            {
                throw new ValidationException("Input parameter 'testId' must be >= 0");
            }
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ValidationException("Input parameters must be >= 0");
            }

            var linkPage = await _processingService.GetLinksPageAsync(pageNumber, pageSize, testId);
            var mapLinkPage = _mapper.MapModelToDetailsDto(linkPage);
            var respose = new LinkPageResponse { DetailsPage = mapLinkPage };
            return respose;
        }
    }
}
