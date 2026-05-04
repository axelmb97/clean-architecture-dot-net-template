using Microsoft.EntityFrameworkCore.Diagnostics;
using Presentation.Dtos;
using Presentation.Resources;
using System.Security.Claims;
using System.Threading.RateLimiting;

namespace Presentation.Extensions
{
    public static class RateLimiterConfigurations
    {
        public static IServiceCollection AddRateLimiterPolicies(this IServiceCollection services)
        {

            services.AddRateLimiter(rateLimiterOptions => {

                rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                rateLimiterOptions.OnRejected = async (context, token) => {

                    context.HttpContext.Response.Headers.RetryAfter = $"{CodeStrings.RetryAfterMinutes}";

                    ErrorResponseDetailsDto errorDetails = new ErrorResponseDetailsDto
                    {
                        StatusCode = StatusCodes.Status429TooManyRequests,
                        Message = $"Too many requests. Please try again after {CodeStrings.RetryAfterMinutes} seconds",
                        Title = "Too Many Requests"
                    };

                    await context.HttpContext.Response.WriteAsJsonAsync(errorDetails, cancellationToken: token);

                };

                rateLimiterOptions.AddPolicy("per-user-policy", httpContext => {

                    string? userId = httpContext.User.FindFirstValue("UserId");

                    if (!string.IsNullOrEmpty(userId))
                    {

                        return RateLimitPartition.GetSlidingWindowLimiter(
                           partitionKey: userId,
                           factory: _ => new SlidingWindowRateLimiterOptions
                           {
                               PermitLimit = 60,
                               Window = TimeSpan.FromMinutes(1),
                               SegmentsPerWindow = 2,
                               QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                               QueueLimit = 0
                           });
                    }

                    return RateLimitPartition.GetFixedWindowLimiter(
                            "anonymous",
                            factory: _ => new FixedWindowRateLimiterOptions
                            {
                                PermitLimit = 5,
                                Window = TimeSpan.FromMinutes(1),
                                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                                QueueLimit = 0
                            }
                        );
                });
            });

            return services;
        }
    }
}
