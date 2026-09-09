using FourLines.Application.DependencyInjection;
using FourLines.Application.DTOs.Reservations;
using FourLines.Application.Interfaces;
using FourLines.Domain.DependencyInjection;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Infrastructure.Contexts;
using FourLines.Infrastructure.DependencyInjection;

namespace FourLines.Tests.Shared;

public abstract class DefaultInitializationFixture
{
    public IConfiguration Configuration { get; set; }
    public HostApplicationBuilder Builder { get; set; }
    public IServiceProvider ServiceProvider { get; set; }

    public DefaultInitializationFixture()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.Tests.json")
            .AddInMemoryCollection()
            .Build();

        Builder = new HostApplicationBuilder();

        Builder.Services
            .AddInfrastructure(Configuration)
            .AddApplication(Configuration)
            .AddDomain();

        Builder.Configuration.AddConfiguration(Configuration);

        IHost host = Builder.Build();

        ServiceProvider = host.Services;
    }

    public async Task DefaultSeedAsync()
    {
        await using var context = CreateContext();

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        await DbOperations.CreateRecord<Role>(TestDataSource.RoleOwner, context);
        await DbOperations.CreateRecord<Role>(TestDataSource.RolePlayer, context);

        await DbOperations.CreateRecord<Sport>(TestDataSource.DefaultSport, context);

        await DbOperations.CreateRecord<User>(TestDataSource.UserOwner, context);
        await DbOperations.CreateRecord<User>(TestDataSource.UserPlayer, context);

        await DbOperations.CreateRecord<Facility>(TestDataSource.DefaultFacility, context);

        await DbOperations.CreateRecord<Court>(TestDataSource.DefaultCourt, context);

        await DbOperations.CreateRecord<FacilitySchedule>(TestDataSource.DefaultFacilitySchedule, context);
    }

    public async Task DeleteDatabaseAsync()
    {
        await using var context = CreateContext();
        await context.Database.EnsureDeletedAsync();
    }

    public FourLinesContext CreateContext()
    {
        string connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__Postgres") ??
            Configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string not found.");

        DbContextOptions<FourLinesContext> options = new DbContextOptionsBuilder<FourLinesContext>()
            .EnableDetailedErrors()
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention()
            .Options;

        return new FourLinesContext(options);
    }

    public abstract Task SeedLocalTestingDataAsync();
}
