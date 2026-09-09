using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.FacilitySchedules;

[Collection(FacilitySchedulesCollection.Name)]
public class TestFacilitySchedulesRead(FacilitySchedulesFixture fixtures)
{
    [Fact]
    public async Task Should_GetFacilitiesSchedules()
    {
        // Arrange
        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        // Act
        Result<IEnumerable<FacilitySchedule>> result = await facilityScheduleHandler.GetSchedules(
            TestDataSource.FacilitySchedule1.FacilityId
        );

        // Assert
        Assert.NotEmpty(result.Value);
        Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public async Task Should_Not_GetFacilitiesSchedules()
    {
        // Arrange
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
