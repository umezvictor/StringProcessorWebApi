using Hangfire;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using Shared;
using System.Diagnostics;
using System.Threading.RateLimiting;
using Webly.Infrastructure;
using Webly.Jobs;

namespace Webly
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors();

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Instance =
                        $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

                    Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                    context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
                };
            });
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                options.AddPolicy(Constants.RateLimitingPolicy, httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 30,
                            Window = TimeSpan.FromMinutes(1),
                        }));
            });


            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddHangfire(
                config => config.UseSqlServerStorage(
                configuration.GetConnectionString("DefaultConnection")));

            services.AddHangfireServer(options => options.SchedulePollingInterval = TimeSpan.FromSeconds(2));

            services.AddTransient<StringProcessorJob>();

            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, DefaultUserIdProvider>();

            return services;
        }
    }
}
