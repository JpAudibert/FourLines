using Microsoft.AspNetCore.Http.Features;
using System.Diagnostics;

namespace FourLines.Api.DependencyInjection;

public static class ProblemDetailsExtensions
{
    public static IServiceCollection ConfigureProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = (context) =>
            {
                context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

                Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                context.ProblemDetails.Extensions.TryAdd("traceId", activity?.TraceId.ToString() ?? context.HttpContext.TraceIdentifier);
            };
        });

        return services;
    }
}
