using FourLines.Domain.Models;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Facilities;

public class FacilityFixture : DefaultInitializationFixture, IAsyncLifetime
{
    public FacilityFixture() : base()
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
    }
}
