using Application.Common.Exceptions;
using Application.Common.Exceptions.Base;
using Presentation.Dtos;
using System.Net;

namespace Presentation.Middlewares
{
    public class ExceptionhandlerMidleware
    {
        private IEnumerable<string> BadRequestExceptions { get; set; }
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionhandlerMidleware> _logger;

        public ExceptionhandlerMidleware(RequestDelegate next, ILogger<ExceptionhandlerMidleware> logger)
        {
            _next = next ?? throw new ArgumentNullException();
            _logger = logger ?? throw new ArgumentNullException();
            BadRequestExceptions = new List<string> {
                 typeof(ValidationBaseException).Name,
                 //Agregar Exceptions que se quieran manejar como BadRequest
                 //typeof(DeleteIdNotFoundException).Name,
                 //typeof(EntityToDeleteNotFoundException).Name,
                 //typeof(EntityToUpdateNotFoundException).Name,
                 //typeof(InvalidEncryptedIdException).Name
            };
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (BaseException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, BaseException exception)
        {
            ErrorResponseDetailsDto result;
            context.Response.Clear();
            context.Response.ContentType = "application/json";

            var statusCode = GetStatusCode(exception);

            result = new ErrorResponseDetailsDto() { Message = exception.Message, StatusCode = statusCode };
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(result.ToString());
            return;
        }

        private int GetStatusCode(BaseException exception)
        {
            if (BadRequestExceptions.Contains(exception.GetType().Name)) return (int)HttpStatusCode.BadRequest;
            if (exception is UnauthorizeAccessException) return (int)HttpStatusCode.Unauthorized;

            return (int)HttpStatusCode.InternalServerError;
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ErrorResponseDetailsDto result;
            context.Response.Clear();
            context.Response.ContentType = "application/json";

            result = new ErrorResponseDetailsDto() { Message = exception.Message, StatusCode = (int)HttpStatusCode.InternalServerError };
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(result.ToString());
            return;
        }
    }

    public static class ExceptionHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionhandlerMidleware>();
        }
    }
}
