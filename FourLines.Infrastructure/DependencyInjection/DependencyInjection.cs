using Microsoft.Data.Sqlite;

namespace FourLines.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        string connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Postgres") ??
            configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string not found.");

        services.AddDbContext<FourLinesContext>(
            (serviceProvider, options) =>
            {
                options
                    .UseNpgsql(connectionString)
                    .UseSnakeCaseNamingConvention();
            }
        );

        services.AddScoped(typeof(IStandardRepository<>), typeof(StandardRepository<>));

        return services;
    }
}
