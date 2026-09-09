using FourLines.Domain.Models;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Courts;

public class CourtFixture : DefaultInitializationFixture, IAsyncLifetime
{
    public CourtFixture() : base()
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
        await DbOperations.CreateRecord<Sport>(TestDataSource.Sport2, context);
        await DbOperations.CreateRecord<Court>(TestDataSource.Court2, context);
        await DbOperations.CreateRecord<Court>(TestDataSource.Court4, context);

        await DbOperations.CreateRecord<Facility>(TestDataSource.Facility2, context);
        await DbOperations.CreateRecord<Court>(TestDataSource.Court3, context);
    }

}
