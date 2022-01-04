using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WebCrawler.Services;
using WebCrawler.Services.Services;
using WebCrawler.WebApplication.Services;
using WebCrawler.WebApplication.ViewModels;

namespace WebCrawler.WebApplication.Controllers
{
    public class TestController : Controller
    {
        private readonly DataStorageService _storage;
        private readonly WebCrawlerService _webCrawler;
        private readonly DataProcessingService _processingService;
        private readonly Mapper _mapper;

        public TestController(DataStorageService storage, WebCrawlerService webCrawler, DataProcessingService processingService, Mapper mapper)
        {
            _storage = storage;
            _webCrawler = webCrawler;
            _processingService = processingService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Tests");
        }

        [HttpGet("[controller]/[action]")]
        public async Task<IActionResult> Tests([FromQuery] int page = 1, [FromQuery] int count = 18)
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
            var links = await _webCrawler.GetLinksAsync(new Uri(url));
            var perfomanceData = await _webCrawler.GetPerfomanceDataCollectionAsync(links);
            await _storage.SaveAsync(url, links, perfomanceData);

            return RedirectToAction("Tests");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
