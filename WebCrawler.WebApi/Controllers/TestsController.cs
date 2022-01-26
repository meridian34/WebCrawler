using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Services.Models;
using WebCrawler.Services.Services;
using WebCrawler.WebApi.Requests;
using WebCrawler.WebApi.Services;
using WebCrawler.WebApi.Responses;

namespace WebCrawler.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestsController : ControllerBase
    {
        private readonly TestsService _testsService;

        public TestsController(TestsService testsService)
        {
            _testsService = testsService;
        }

        /// <summary>
        /// Get tests by page
        /// </summary>
        /// <param name="pageNumber">Number of page</param>
        /// <param name="pageSize">Number of elements per page</param>
        /// <returns>Tests</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Invalid one of the input parameters</response>
        [HttpGet]
        public async Task<ActionResult<TestPageResponse>> GetTests([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var respose = await _testsService.GetTestsAsync(pageNumber, pageSize);

            return Ok(respose);
        }

        /// <summary>
        /// Start site testing
        /// </summary>
        /// <param name="request">Request with URL for crawling</param>
        /// <returns></returns>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Invalid input URL</response>
        [HttpPost]
        public async Task<IActionResult> StartTest([FromBody] StartTestRequest request)
        {
            await _testsService.StartTest(request.CrawlUrl);

            return Ok("Crawling completed!");
        }
    }
}
