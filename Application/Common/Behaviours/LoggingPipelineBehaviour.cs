

using Application.Common.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Behaviours
{
    public class LoggingPipelineBehaviour<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggingPipelineBehaviour(
            ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> logger,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {

            try
            {
                var userId = GetUserId();

                _logger.LogInformation(
                    "Starting request {RequestName} for UserId: {UserId}: , {DateTimeUtc}",
                    typeof(TRequest).Name,
                    userId,
                    DateTime.UtcNow);

                var result = await next();

                _logger.LogInformation(
                       "Completed request {RequestName}, {DateTimeUtc}",
                           typeof(TRequest).Name,
                           DateTime.UtcNow);

                return result;
            }
            catch (Exception ex)
            {
                var userId = GetUserId();

                _logger.LogError(
                        "Request failure {RequestName} for UserId: {UserId}, Errors: {Error}, {DateTimeUtc} with body: {@Request}",
                            typeof(TRequest).Name,
                            userId,
                            ex.Message,
                            DateTime.UtcNow,
                            request);

                throw ex;
            }
        }

        private int GetUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst("UserId")?.Value;

            //if (String.IsNullOrEmpty(userId)) throw new UserIdNotFoundException();
            return 1;
            return Int32.Parse(userId);
        }
    }
}
