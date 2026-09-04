using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.FacilitySchedules;

public class TestFacilitySchedulesRead(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    [Fact]
    public async Task Should_GetFacilitiesSchedules()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
            await DbOperations.CreateEntityInMemory<FacilitySchedule>(
                InMemoryDataSource.FacilitySchedule1,
                context
            );
            await DbOperations.CreateEntityInMemory<FacilitySchedule>(
                InMemoryDataSource.FacilitySchedule2,
                context
            );
        }

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        // Act
        Result<IEnumerable<FacilitySchedule>> result = await facilityScheduleHandler.GetSchedules(
            InMemoryDataSource.UserOwner.Id,
            InMemoryDataSource.Facility1.Id
        );

        // Assert
        Assert.NotEmpty(result.Value);
        Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public async Task Should_Not_GetFacilitiesSchedules()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.RemoveAllDataFromMemory<Facility>(context);
        }

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        // Act
        Result<IEnumerable<FacilitySchedule>> result = await facilityScheduleHandler.GetSchedules(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitySchedulesErrorResults.RetrieveFacilitySchedules, result.Error);
    }
}
