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
                // Allow tests to opt-in to an in-memory provider using configuration
                if (configuration.GetValue<bool>("UseInMemory", false))
                {
                    options
                        .UseSqlite(serviceProvider.GetService<SqliteConnection>()!)
                        .UseSnakeCaseNamingConvention()
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors()
                        .LogTo(Console.WriteLine, LogLevel.Information);
                }
                else
                {
                    options
                        .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                        .UseSnakeCaseNamingConvention();
                }
            }
        );

        services.AddScoped(typeof(IStandardRepository<>), typeof(StandardRepository<>));

        return services;
    }
}
