using DotNet.Testcontainers.Builders;
using FourLines.Application.DependencyInjection;
using FourLines.Domain.DependencyInjection;
using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;
using FourLines.Infrastructure.DependencyInjection;
using Testcontainers.PostgreSql;

namespace FourLines.Tests.Shared;

public class FourLinesFixture : IAsyncLifetime
{
    public IConfiguration Configuration { get; set; }
    public HostApplicationBuilder Builder { get; set; }
    public IServiceProvider ServiceProvider { get; set; }

    private readonly PostgreSqlContainer _postgres =
        new PostgreSqlBuilder("postgres:18")
            .WithDatabase("fourlines_test")
            .WithUsername("fourlines")
            .WithPassword("fourlines")
            .WithPortBinding(5433, 5432)
            .WithWaitStrategy(
                Wait.ForUnixContainer()
                    .UntilCommandIsCompleted(
                        "pg_isready",
                        "-U", "fourlines",
                        "-d", "fourlines_test"))
            .Build();

    public FourLinesFixture()
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
        await using var scope = CreateAsyncServiceScope();
        await _postgres.StartAsync();

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
    }

    public async Task SeedDefaultUsers(FourLinesContext context)
    {
        await CreateRecord<User>(TestDataSource.UserOwner, context);
        await CreateRecord<User>(TestDataSource.UserPlayer, context);
        await CreateRecord<User>(TestDataSource.UserPlayer2, context);
    }

    public async Task SeedDefaultFacilities(FourLinesContext context)
    {
        await CreateRecord<Facility>(TestDataSource.DefaultFacility, context);
        await CreateRecord<Facility>(TestDataSource.DefaultFacility2, context);
        await CreateRecord<Facility>(TestDataSource.DefaultNoSchedulesFacility, context);

        await CreateRecord<Facility>(TestDataSource.ToBeUpdatedFacility, context);
        await CreateRecord<Facility>(TestDataSource.ToBeDeletedFacility, context);
    }

    public async Task SeedDefaultCourts(FourLinesContext context)
    {
        await CreateRecord<Court>(TestDataSource.DefaultCourt, context);
        await CreateRecord<Court>(TestDataSource.CourtWithNoSchedule, context);

        await CreateRecord<Court>(TestDataSource.ToBeUpdatedCourt, context);
        await CreateRecord<Court>(TestDataSource.ToBeDeletedCourt, context);
    }

    public async Task SeedDefaultFacilitySchedules(FourLinesContext context)
    {
        await CreateRecord<FacilitySchedule>(TestDataSource.DefaultFacilityScheduleSunday, context);
        await CreateRecord<FacilitySchedule>(TestDataSource.DefaultFacilityScheduleMonday, context);
        await CreateRecord<FacilitySchedule>(TestDataSource.DefaultFacilityScheduleTuesday, context);
        await CreateRecord<FacilitySchedule>(TestDataSource.DefaultFacilityScheduleWednesday, context);
        await CreateRecord<FacilitySchedule>(TestDataSource.DefaultFacilityScheduleThursday, context);
        await CreateRecord<FacilitySchedule>(TestDataSource.DefaultFacilityScheduleFriday, context);
        await CreateRecord<FacilitySchedule>(TestDataSource.DefaultFacilityScheduleSaturday, context);

        await CreateRecord<FacilitySchedule>(TestDataSource.ToBeUpdatedFacilitySchedule, context);
        await CreateRecord<FacilitySchedule>(TestDataSource.ToBeDeletedFacilitySchedule, context);
    }

    public async Task SeedDefaultReservations(FourLinesContext context)
    {
        await CreateRecord<Reservation>(TestDataSource.DefaultReservation, context);

        await CreateRecord<Reservation>(TestDataSource.ToBeUpdatedReservation, context);
        await CreateRecord<Reservation>(TestDataSource.ToBeDeletedReservation, context);
    }

    //public async Task RemoveAllRecords<T>()
    //    where T : BaseEntity
    //{
    //    Context.Set<T>().RemoveRange(Context.Set<T>());
    //    await Context.SaveChangesAsync();
    //}

    //public async Task RemoveRecord<T>(Guid id)
    //    where T : BaseEntity
    //{
    //    T? entity = await Context.Set<T>().FindAsync(id);

    //    if (entity != null)
    //    {
    //        Context.Set<T>().Remove(entity);
    //        await Context.SaveChangesAsync();
    //    }
    //}

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
}
