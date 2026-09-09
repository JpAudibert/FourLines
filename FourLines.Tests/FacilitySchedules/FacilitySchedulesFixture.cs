using FourLines.Domain.Models;
using FourLines.Tests.Shared;

namespace FourLines.Tests.FacilitySchedules;

public class FacilitySchedulesFixture : DefaultInitializationFixture, IAsyncLifetime
{
    public FacilitySchedulesFixture() : base()
    { }

    public async Task DisposeAsync()
    {
        await DeleteDatabaseAsync();
    }

    public async Task InitializeAsync()
    {
        await DefaultSeedAsync();
        await SeedLocalTestingDataAsync();
    }

    public override async Task SeedLocalTestingDataAsync()
    {
        await using var context = CreateContext();
        await DbOperations.CreateRecord<Facility>(TestDataSource.Facility2, context);
        await DbOperations.CreateRecord<Facility>(TestDataSource.Facility3, context);
        await DbOperations.CreateRecord<Facility>(TestDataSource.Facility4, context);

        await DbOperations.CreateRecord<FacilitySchedule>(TestDataSource.FacilitySchedule1, context);
        await DbOperations.CreateRecord<FacilitySchedule>(TestDataSource.FacilitySchedule2, context);
        await DbOperations.CreateRecord<FacilitySchedule>(TestDataSource.FacilitySchedule4, context);
    }

}
