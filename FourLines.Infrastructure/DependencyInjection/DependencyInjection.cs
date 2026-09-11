namespace FourLines.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        string defaultConnectionString = default!
    )
    {
        string connectionString = defaultConnectionString;

        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString =
                Environment.GetEnvironmentVariable("ConnectionStrings__Postgres")
                ?? configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.");
        }

        services.AddDbContext<FourLinesContext>(
            (serviceProvider, options) =>
            {
                if (configuration.GetValue<bool>("UseInMemory", false))
                {
                    options
                        .UseNpgsql(connectionString)
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors()
                        .LogTo(Console.WriteLine, LogLevel.Information)
                        .UseSnakeCaseNamingConvention();
                }
                else
                {
                    options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
                }
            }
        );

        services.AddScoped(typeof(IStandardRepository<>), typeof(StandardRepository<>));

        return services;
    }
}
