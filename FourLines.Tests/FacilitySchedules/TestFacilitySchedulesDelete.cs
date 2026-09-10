using FourLines.Application.DTOs.FacilitySchedules;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Tests.Shared;

namespace FourLines.Tests.FacilitySchedules;

[Collection(FourLinesCollection.Name)]
public class TestFacilitySchedulesDelete(FourLinesFixture fixtures)
{
    [Fact]
    public async Task Should_DeleteFacilitySchedule()
    {
        // Arrange
        await using var context = fixtures.CreateContext();
        FacilitySchedule toBeDeleted = await DbOperations.CreateRecord(
            TestDataSource.ToBeDeletedFacilitySchedule,
            context
        );

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        // Act
        Result<bool> result = await facilityScheduleHandler.Delete(new DeleteFacilityScheduleDTO
        {
            FacilityId = toBeDeleted.FacilityId,
            ScheduleId = toBeDeleted.Id
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
