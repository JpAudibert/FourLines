using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Domain.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        return services;
    }
}
