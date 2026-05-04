using Application.Common.Abstractions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Presentation.Controllers
{
    [Route("time-tracker/time-entries")]
    [ApiController]
    [EnableRateLimiting("per-user-policy")]
    public class ExampleController : PaginationController
    {
        public ExampleController(IAuthCachedService authCachedService) : base(authCachedService)
        {
        }
    }
}
