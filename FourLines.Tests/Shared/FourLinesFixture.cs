using DotNet.Testcontainers.Builders;
using FourLines.Application.DependencyInjection;
using FourLines.Application.DTOs.Reservations;
using FourLines.Application.Interfaces;
using FourLines.Domain.DependencyInjection;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Infrastructure.Contexts;
using FourLines.Infrastructure.DependencyInjection;
using FourLines.Tests.Matches;
using Testcontainers.PostgreSql;

namespace FourLines.Tests.Shared;

public class FourLinesFixture : IAsyncLifetime
{
    public IConfiguration Configuration { get; set; } = default!;
    public HostApplicationBuilder Builder { get; set; } = default!;
    public IServiceProvider ServiceProvider { get; set; } = default!;

    private bool _isGoalKeeperReservationCreated = false;
    private bool _isNoGoalKeeperReservationCreated = false;
    public Result<ConfirmReservationResponseDTO> GoalKeeperReservationResult = default!;
    public Result<ConfirmReservationResponseDTO> NoGoalKeeperReservationResult = default!;

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

        await SeedDefaultRoles(context);
        await SeedDefaultSports(context);
        await SeedDefaultUsers(context);
        await SeedDefaultFacilities(context);
        await SeedDefaultCourts(context);
        await SeedDefaultFacilitySchedules(context);
        await SeedDefaultReservations(context);

        Console.WriteLine();
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
            .AddApplication(Configuration)
            .AddDomain();

        Builder.Configuration.AddConfiguration(Configuration);

        IHost host = Builder.Build();

        ServiceProvider = host.Services;
    }

    public AsyncServiceScope CreateAsyncServiceScope()
    {
        return ServiceProvider.CreateAsyncScope();
    }

    public async Task SeedDefaultRoles(FourLinesContext context)
    {
        await CreateRecord<Role>(TestDataSource.RoleOwner, context);
        await CreateRecord<Role>(TestDataSource.RolePlayer, context);
    }

    public async Task SeedDefaultSports(FourLinesContext context)
    {
        await CreateRecord<Sport>(TestDataSource.DefaultSport, context);
        await CreateRecord<Sport>(TestDataSource.SportWithoutGoalkeeper, context);
    }

    public async Task SeedDefaultUsers(FourLinesContext context)
    {
        await CreateRecord<User>(TestDataSource.UserOwner, context);
        await CreateRecord<User>(TestDataSource.UserPlayer, context);
        await CreateRecord<User>(TestDataSource.UserPlayer2, context);
        await CreateRecord<User>(TestDataSource.UserPlayer3, context);
    }

    public async Task SeedDefaultFacilities(FourLinesContext context)
    {
        await CreateRecord<Facility>(TestDataSource.DefaultFacility, context);
        await CreateRecord<Facility>(TestDataSource.DefaultFacility2, context);
        await CreateRecord<Facility>(TestDataSource.DefaultNoSchedulesFacility, context);
        await CreateRecord<Facility>(TestDataSource.DefaultNoSchedulesFacility2, context);

        await CreateRecord<Facility>(TestDataSource.ToBeUpdatedFacility, context);
        await CreateRecord<Facility>(TestDataSource.ToBeDeletedFacility, context);
    }

    public async Task SeedDefaultCourts(FourLinesContext context)
    {
        await CreateRecord<Court>(TestDataSource.DefaultCourt, context);
        await CreateRecord<Court>(TestDataSource.CourtWithNoSchedule, context);
        await CreateRecord<Court>(TestDataSource.CourtWithNoSchedule2, context);

        await CreateRecord<Court>(TestDataSource.ToBeUpdatedCourt, context);
        await CreateRecord<Court>(TestDataSource.ToBeDeletedCourt, context);

        await CreateRecord<Court>(TestDataSource.CourtWithSportWithoutGoalkeeper, context);
    }

    public async Task SeedDefaultFacilitySchedules(FourLinesContext context)
    {
        await CreateRecord<FacilitySchedule>(TestDataSource.DefaultFacilityScheduleSunday, context);
        await CreateRecord<FacilitySchedule>(TestDataSource.DefaultFacilityScheduleMonday, context);
        await CreateRecord<FacilitySchedule>(
            TestDataSource.DefaultFacilityScheduleTuesday,
            context
        );
        await CreateRecord<FacilitySchedule>(
            TestDataSource.DefaultFacilityScheduleWednesday,
            context
        );
        await CreateRecord<FacilitySchedule>(
            TestDataSource.DefaultFacilityScheduleThursday,
            context
        );
        await CreateRecord<FacilitySchedule>(TestDataSource.DefaultFacilityScheduleFriday, context);
        await CreateRecord<FacilitySchedule>(
            TestDataSource.DefaultFacilityScheduleSaturday,
            context
        );

        await CreateRecord<FacilitySchedule>(TestDataSource.ToBeUpdatedFacilitySchedule, context);
        await CreateRecord<FacilitySchedule>(TestDataSource.ToBeDeletedFacilitySchedule, context);
    }

    public async Task SeedDefaultReservations(FourLinesContext context)
    {
        await CreateRecord<Reservation>(TestDataSource.DefaultReservation, context);

        await CreateRecord<Reservation>(TestDataSource.ToBeUpdatedReservation, context);
        await CreateRecord<Reservation>(TestDataSource.ToBeDeletedReservation, context);
    }

    public async Task RemoveAllRecords<T>(FourLinesContext context)
        where T : BaseEntity
    {
        context.Set<T>().RemoveRange(context.Set<T>());
        await context.SaveChangesAsync();
    }

    public async Task RemoveRecord<T>(Guid id, FourLinesContext context)
        where T : BaseEntity
    {
        T? entity = await context.Set<T>().FindAsync(id);

        if (entity != null)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }
    }

    public async Task<T> CreateRecord<T>(T entity, FourLinesContext context)
        where T : BaseEntity
    {
        T? objectexists = await context.FindAsync<T>(entity.Id);

        if (objectexists is null)
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }

        return entity;
    }

    public async Task EnsureGoalKeeperReservationCreatedAsync()
    {
        if (!_isGoalKeeperReservationCreated)
        {
            IReservationHandler reservationHandler =
                ServiceProvider.GetRequiredService<IReservationHandler>();
            GoalKeeperReservationResult = await reservationHandler.Create(
                TestDataSource.CreateGoalKeeperReservationTest
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
                TestDataSource.CreateNoGoalKeeperReservationTest
            );

            _isNoGoalKeeperReservationCreated = true;
        }
    }
}
