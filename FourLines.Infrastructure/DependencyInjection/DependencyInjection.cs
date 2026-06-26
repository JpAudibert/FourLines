using FourLines.Infrastructure.Contexts;
using FourLines.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FourLinesContext>(options =>
            options
                .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .UseSnakeCaseNamingConvention());

        services.AddScoped(typeof(IStandardRepository<>), typeof(StandardRepository<>));

        return services;
    }
}
