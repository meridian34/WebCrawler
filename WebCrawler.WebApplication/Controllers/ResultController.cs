using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebCrawler.Services.Services;
using WebCrawler.WebApplication.Services;

namespace WebCrawler.WebApplication.Controllers
{
    public class ResultController : Controller
    {
        private readonly DataProcessingService _processingService;
        private readonly Mapper _mapper;

        public ResultController(DataProcessingService processingService, Mapper mapper)
        {
            _processingService = processingService;
            _mapper = mapper;
        }

        [HttpGet("[controller]/[action]")]
        public async Task<IActionResult> TestData([FromQuery] int testId)
        {
            var result = await _processingService.GetLinksPage(testId);
            var resultViewModel = _mapper.LinksPageToLinksPageViewModel(result);
            return View(resultViewModel);
        }
        
    }
}
