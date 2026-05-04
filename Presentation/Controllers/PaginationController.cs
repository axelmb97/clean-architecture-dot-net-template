using Application.Common.Abstractions.Services;
using Application.Common.Dtos.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Presentation.Controllers
{
    [ApiController]
    public class PaginationController : ClaimsController
    {
        public PaginationController(IAuthCachedService authCachedService) : base(authCachedService)
        {

        }

        protected void AddPaginationHeaders(PaginationHeadersDto paginationHeaders)
        {
            if (paginationHeaders == null) return;

            Response.Headers.Add("x-pagination", JsonSerializer.Serialize(paginationHeaders));
        }
    }
}
