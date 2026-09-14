using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Infrastructure.Contexts;
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
        FourLinesContext context = scope.ServiceProvider.GetRequiredService<FourLinesContext>();

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        Guid facilityId = TestDataSource.DefaultFacility.Id;

        // Act
        Result<IEnumerable<FacilitySchedule>> result = await facilityScheduleHandler.GetSchedules(
            facilityId
        );

        // Assert
        int facilitySchedules = context
            .FacilitySchedules.Where(s => s.FacilityId == facilityId)
            .Count();

        Assert.NotEmpty(result.Value);
        Assert.Equal(facilitySchedules, result.Value.Count());
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
