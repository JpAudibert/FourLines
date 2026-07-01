using FourLines.Infrastructure.Contexts;
using FourLines.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace FourLines.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FourLinesContext>(options =>
        {
            // Allow tests to opt-in to an in-memory provider using configuration
            if (configuration.GetValue<bool>("UseInMemory", false))
            {
                options.UseInMemoryDatabase("FourLines_InMemory_Db");
            }
            else
            {
                options
                    .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                    .UseSnakeCaseNamingConvention();
            }
        });

        services.AddScoped(typeof(IStandardRepository<>), typeof(StandardRepository<>));

        return services;
    }
}
