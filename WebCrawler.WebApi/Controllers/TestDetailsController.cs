using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebCrawler.WebApi.Responses;
using WebCrawler.WebApi.Services;

namespace WebCrawler.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TestDetailsController : ControllerBase
    {
        private readonly TestDetailsService _detailsService;

        public TestDetailsController(TestDetailsService detailsService)
        {
            _detailsService = detailsService;
        }

        /// <summary>
        /// Get test results by test id
        /// </summary>
        /// <param name="testId">Id of the test for which you want to get the test results</param>
        /// <returns>Response with test results</returns>
        [HttpGet]
        [Route("[action]/{testId}")]
        public async Task<ActionResult<LinkPageResponse>> GetByTestId([FromRoute]int testId)
        {
            var respose = await _detailsService.GetByTestIdAsync(testId);

            return Ok(respose);
        }

        /// <summary>
        /// Get test results by page
        /// </summary>
        /// <param name="testId">Id of the test for which you want to get the test results</param>
        /// <param name="pageNumber">Number of page</param>
        /// <param name="pageSize">Number of elements per page</param>
        /// <returns>Response with test results</returns>
        [HttpGet]
        [Route("{testId}")]
        public async Task<ActionResult<LinkPageResponse>> GetDetailsByPage([FromRoute] int testId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var respose = await _detailsService.GetDetailsByPageAsync(testId, pageNumber, pageSize);

            return Ok(respose);
        }
    }
}
