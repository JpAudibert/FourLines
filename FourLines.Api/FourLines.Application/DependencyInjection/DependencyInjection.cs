using FourLines.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<TestService>();

        return services;
    }
}
