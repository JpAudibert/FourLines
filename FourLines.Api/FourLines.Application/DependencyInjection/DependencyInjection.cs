using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}
