using DotNet.Testcontainers.Builders;
using FourLines.Application.DependencyInjection;
using FourLines.Application.DTOs.Reservations;
using FourLines.Application.Interfaces;
using FourLines.Domain.DependencyInjection;
using FourLines.Domain.Results;
using FourLines.Infrastructure.Contexts;
using FourLines.Infrastructure.DependencyInjection;
using FourLines.Tests.Shared.Seed;
using Testcontainers.PostgreSql;

namespace FourLines.Tests.Shared;

public class FourLinesFixture : IAsyncLifetime
{
    public IConfiguration Configuration { get; set; } = default!;
    public HostApplicationBuilder Builder { get; set; } = default!;
    public IServiceProvider ServiceProvider { get; set; } = default!;

    private bool _isGoalKeeperReservationCreated = false;
    private bool _isNoGoalKeeperReservationCreated = false;
    private bool _isShuffleReservationCreated = false;
    public Result<ConfirmReservationResponseDTO> GoalKeeperReservationResult = default!;
    public Result<ConfirmReservationResponseDTO> NoGoalKeeperReservationResult = default!;
    public Result<ConfirmReservationResponseDTO> ShuffleReservationResult = default!;

    private const string _databaseName = "fourlines_test";
    private const string _username = "fourlines";
    private const string _password = "fourlines";
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:18")
        .WithDatabase(_databaseName)
        .WithUsername(_username)
        .WithPassword(_password)
        .WithWaitStrategy(
            Wait.ForUnixContainer()
                .UntilCommandIsCompleted("pg_isready", "-U", _username, "-d", _databaseName)
        )
        .Build();

    private string _connectionString => _postgres.GetConnectionString();

    public async Task InitializeAsync()
    {
        await InjectDependencies();
        await using var scope = CreateAsyncServiceScope();

        FourLinesContext context = scope.ServiceProvider.GetRequiredService<FourLinesContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();

        await TestDataSeeder.SeedAsync(context);
    }

    public async Task DisposeAsync()
    {
        await using var scope = CreateAsyncServiceScope();
        FourLinesContext context = scope.ServiceProvider.GetRequiredService<FourLinesContext>();

        await context.Database.EnsureDeletedAsync();

        await _postgres.StopAsync();
        await _postgres.DisposeAsync();
    }

    public async Task InjectDependencies()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.Tests.json")
            .AddInMemoryCollection()
            .Build();

        Builder = new HostApplicationBuilder();

        await _postgres.StartAsync();

        Builder
            .Services.AddInfrastructure(Configuration, _connectionString)
            .AddApplication()
            .AddDomain();

        Builder.Configuration.AddConfiguration(Configuration);

        IHost host = Builder.Build();

        ServiceProvider = host.Services;
    }

    public AsyncServiceScope CreateAsyncServiceScope()
    {
        return ServiceProvider.CreateAsyncScope();
    }

    public async Task EnsureGoalKeeperReservationCreatedAsync()
    {
        if (!_isGoalKeeperReservationCreated)
        {
            IReservationHandler reservationHandler =
                ServiceProvider.GetRequiredService<IReservationHandler>();
            GoalKeeperReservationResult = await reservationHandler.Create(
                ReservationSeed.CreateGoalKeeperReservationTest
            );

            _isGoalKeeperReservationCreated = true;
        }
    }

    public async Task EnsureNoGoalKeeperReservationCreatedAsync()
    {
        if (!_isNoGoalKeeperReservationCreated)
        {
            IReservationHandler reservationHandler =
                ServiceProvider.GetRequiredService<IReservationHandler>();
            NoGoalKeeperReservationResult = await reservationHandler.Create(
                ReservationSeed.CreateNoGoalKeeperReservationTest
            );

            _isNoGoalKeeperReservationCreated = true;
        }
    }

    public async Task EnsureShuffleReservationCreatedAsync()
    {
        if (!_isShuffleReservationCreated)
        {
            IReservationHandler reservationHandler =
                ServiceProvider.GetRequiredService<IReservationHandler>();
            ShuffleReservationResult = await reservationHandler.Create(
                ReservationSeed.ReservationToShuffle
            );

            _isShuffleReservationCreated = true;
        }
    }
}
