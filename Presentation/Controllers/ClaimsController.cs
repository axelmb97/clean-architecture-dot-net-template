using Application.Common.Abstractions.Services;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Resources;

namespace Presentation.Controllers
{
    [ApiController]
    public class ClaimsController : BaseController
    {
        public string UserId => User.Claims.First(c => c.Type == CodeStrings.UserIdClaimName).Value;

        private readonly IAuthCachedService _authCachedService;

        public ClaimsController(IAuthCachedService authCachedService)
        {
            _authCachedService = authCachedService;
        }

        [NonAction]
        internal void CatchClaims()
        {
            if (_authCachedService is null) throw new AuthCachedServiceNotFoundException();

            _authCachedService.Push(UserId);
        }
    }
}
