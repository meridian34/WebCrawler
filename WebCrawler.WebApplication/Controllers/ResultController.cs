using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Services;
using WebCrawler.Services.Services;
using WebCrawler.WebApplication.Services;

namespace WebCrawler.WebApplication.Controllers
{
    public class ResultController : Controller
    {
        private readonly DataStorageService _storage;
        private readonly DataProcessingService _processingService;
        private readonly Mapper _mapper;

        public ResultController(DataStorageService storage, DataProcessingService processingService, Mapper mapper)
        {
            _storage = storage;
            _processingService = processingService;
            _mapper = mapper;
        }

        [HttpGet("[controller]/[action]")]
        public async Task<IActionResult> TestData([FromQuery] int testId)
        {
            var result = await _storage.GetLinksPageByTestIdAsync(testId);
            var resultViewModel = _mapper.LinksPageToLinksPageViewModel(result);
            return View(resultViewModel);
        }
        public IActionResult GetUrlTable()
        {
            return PartialView("_GetUrlTable");
        }
    }
}
