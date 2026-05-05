using Application.Common.Abstractions.Services;
using Application.Examples.Queries.GetExamplesByFIlters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Presentation.Controllers
{
    [Route("app/examples")]
    [ApiController]
    //[EnableRateLimiting("per-user-policy")]
    public class ExampleController : PaginationController
    {
        public ExampleController(IAuthCachedService authCachedService) : base(authCachedService)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetExamplesByFiltersQuery query, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
