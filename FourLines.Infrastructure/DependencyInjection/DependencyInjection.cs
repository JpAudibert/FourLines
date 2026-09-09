using Microsoft.Data.Sqlite;

namespace FourLines.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<FourLinesContext>(
            (serviceProvider, options) =>
            {
                options
                    .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                    .UseSnakeCaseNamingConvention();
            }
        );

        services.AddScoped(typeof(IStandardRepository<>), typeof(StandardRepository<>));

        return services;
    }
}
