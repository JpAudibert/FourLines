using FourLines.Domain.Interfaces;
using FourLines.Domain.Strategies;

namespace FourLines.Domain.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddSingleton<IShuffleStrategy, DotnetShuffleStrategy>();
        
        return services;
    }
}
