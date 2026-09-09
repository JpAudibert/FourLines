using FourLines.Application.DTOs.FacilitySchedules;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Tests.Facilities;
using FourLines.Tests.Shared;

namespace FourLines.Tests.FacilitySchedules;

[Collection(FacilitySchedulesCollection.Name)]
public class TestFacilitySchedulesDelete(FacilitySchedulesFixture fixtures)
{
    [Fact]
    public async Task Should_DeleteFacilitySchedule()
    {
        // Arrange
        await using var context = fixtures.CreateContext();
        FacilitySchedule testSchedule = await DbOperations.CreateRecord<FacilitySchedule>(
            TestDataSource.FacilitySchedule2,
            context
        );

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        // Act
        Result<bool> result = await facilityScheduleHandler.Delete(new DeleteFacilityScheduleDTO
        {
            FacilityId = testSchedule.FacilityId,
            ScheduleId = testSchedule.Id
        });

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Should_Not_DeleteFacilitySchedule()
    {
        // Arrange
        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        // Act
        Result<bool> result = await facilityScheduleHandler.Delete(new DeleteFacilityScheduleDTO
        {
            FacilityId = Guid.NewGuid(),
            ScheduleId = Guid.NewGuid()
        });

        // Assert
        Assert.False(result.Value);
    }
}
