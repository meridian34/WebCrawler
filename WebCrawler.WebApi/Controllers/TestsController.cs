using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebCrawler.WebApi.Models;
using WebCrawler.WebApi.Services;

namespace WebCrawler.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
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
            var response = await _testsService.GetTestsAsync(pageNumber, pageSize);

            return Ok(response);
        }

        /// <summary>
        /// Start site testing
        /// </summary>
        /// <param name="startRequest">Request with URL for crawling</param>
        /// <returns></returns>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Invalid input URL</response>
        [HttpPost]
        public async Task<IActionResult> StartTest([FromBody] StartTestRequest startRequest)
        {
            await _testsService.StartTest(startRequest.CrawlUrl);
           
            return Ok("Crawling completed!");
        }
    }
}
