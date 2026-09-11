using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.FacilitySchedules;

[Collection(FourLinesCollection.Name)]
public class TestFacilitySchedulesRead(FourLinesFixture fixtures)
{
    [Fact]
    public async Task Should_GetFacilitiesSchedules()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        // Act
        Result<IEnumerable<FacilitySchedule>> result = await facilityScheduleHandler.GetSchedules(
            TestDataSource.DefaultFacility.Id
        );

        // Assert
        Assert.NotEmpty(result.Value);
        Assert.Equal(7, result.Value.Count());
    }

    [Fact]
    public async Task Should_Not_GetFacilitiesSchedules()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        // Act
        Result<IEnumerable<FacilitySchedule>> result = await facilityScheduleHandler.GetSchedules(
            Guid.NewGuid()
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitySchedulesErrorResults.RetrieveFacilitySchedules, result.Error);
    }
}
