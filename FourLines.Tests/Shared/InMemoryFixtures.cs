using FourLines.Application.DependencyInjection;
using FourLines.Application.DTOs.Reservations;
using FourLines.Application.Interfaces;
using FourLines.Domain.DependencyInjection;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Infrastructure.Contexts;
using FourLines.Infrastructure.DependencyInjection;

namespace FourLines.Tests.Shared;

public class InMemoryFixtures : IAsyncLifetime
{
    public IConfiguration Configuration { get; set; }
    public HostApplicationBuilder Builder { get; set; }
    public IServiceProvider ServiceProvider { get; set; }

    private bool _isGoalKeeperReservationCreated = false;
    private bool _isNoGoalKeeperReservationCreated = false;
    public Result<ConfirmReservationResponseDTO> GoalKeeperReservationResult = default!;
    public Result<ConfirmReservationResponseDTO> NoGoalKeeperReservationResult = default!;

    public InMemoryFixtures()
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

    public async Task InitializeAsync()
    {
        await using var context = CreateContext();

        await context.Database.MigrateAsync();

        await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
        await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RolePlayer, context);
        await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
        await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserPlayer, context);
        await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserPlayer2, context);
        await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserPlayer3, context);
        await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
        await DbOperations.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport, context);
        await DbOperations.CreateEntityInMemory<Court>(InMemoryDataSource.Court1, context);
        await DbOperations.CreateEntityInMemory<FacilitySchedule>(
            InMemoryDataSource.FacilitySchedule3,
            context
        );
    }

    public async Task DisposeAsync()
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

    public async Task EnsureGoalKeeperReservationCreatedAsync()
    {
        if (!_isGoalKeeperReservationCreated)
        {
            IReservationHandler reservationHandler =
                ServiceProvider.GetRequiredService<IReservationHandler>();
            GoalKeeperReservationResult = await reservationHandler.Create(InMemoryDataSource.CreateGoalKeeperReservationTest);

            _isGoalKeeperReservationCreated = true;
        }
    }

    public async Task EnsureNoGoalKeeperReservationCreatedAsync()
    {
        if (!_isNoGoalKeeperReservationCreated)
        {
            IReservationHandler reservationHandler =
                ServiceProvider.GetRequiredService<IReservationHandler>();
            NoGoalKeeperReservationResult = await reservationHandler.Create(InMemoryDataSource.CreateNoGoalKeeperReservationTest);

            _isNoGoalKeeperReservationCreated = true;
        }
    }


}
