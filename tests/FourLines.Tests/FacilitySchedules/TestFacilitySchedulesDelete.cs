using FourLines.Application.DTOs.FacilitySchedules;
using FourLines.Application.Interfaces;
using FourLines.Domain.Results;
using FourLines.Tests.Shared;
using FourLines.Tests.Shared.Seed;

namespace FourLines.Tests.FacilitySchedules;

[Collection(FourLinesCollection.Name)]
public class TestFacilitySchedulesDelete(FourLinesFixture fixtures)
{
    [Fact]
    public async Task Should_DeleteFacilitySchedule()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        // Act
        Result<bool> result = await facilityScheduleHandler.Delete(new DeleteFacilityScheduleDTO
        {
            FacilityId = FacilityScheduleSeed.ToBeDeleted.FacilityId,
            ScheduleId = FacilityScheduleSeed.ToBeDeleted.Id
        });

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Should_Not_DeleteFacilitySchedule()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

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
