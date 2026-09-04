using FourLines.Application.DTOs.FacilitySchedules;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Tests.Shared;

namespace FourLines.Tests.FacilitySchedules;

public class TestFacilitySchedulesDelete(InMemoryFixtures fixtures)
    : IClassFixture<InMemoryFixtures>
{
    [Fact]
    public async Task Should_DeleteFacilitySchedule()
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
        }

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        // Act
        Result<bool> result = await facilityScheduleHandler.Delete(new DeleteFacilityScheduleDTO
        {
            OwnerId = InMemoryDataSource.UserOwner.Id,
            FacilityId = InMemoryDataSource.Facility1.Id,
            ScheduleId = InMemoryDataSource.FacilitySchedule1.Id
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
            OwnerId = Guid.NewGuid(),
            FacilityId = Guid.NewGuid(),
            ScheduleId = Guid.NewGuid()
        });

        // Assert
        Assert.False(result.Value);
    }
}
