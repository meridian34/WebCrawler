using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebCrawler.Services.Services;
using WebCrawler.WebApplication.Services;

namespace WebCrawler.WebApplication.Controllers
{
    public class TestController : Controller
    {
        private readonly DataProcessingService _processingService;
        private readonly Mapper _mapper;

        public TestController(DataProcessingService processingService, Mapper mapper)
        {
            _processingService = processingService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Tests");
        }

        [HttpGet]
        public async Task<IActionResult> Tests([FromQuery] int page = 1, [FromQuery] int count = 2)
        {
            if (page < 0 || count < 0)
            {
                return BadRequest();
            }

            var testsPage = await _processingService.GetTestsPage(page, count);
            var testPageViewModel = _mapper.TestPageToTestPageViewModel(testsPage);

            return View(testPageViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Tests(string url)
        {
            bool urlIsValid = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (!urlIsValid)
            {
                return BadRequest("Input url is not valid");
            }

            await _processingService.StartCrawlingSite(uriResult);

            return RedirectToAction("Tests");
        }
    }
}
