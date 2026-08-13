using FourLines.Application.Handlers;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.FacilitySchedules;

public class TestFacilitySchedulesDelete(InMemoryFixtures fixtures)
    : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures = fixtures;

    [Fact]
    public async Task Should_DeleteFacilitySchedule()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1);
        await _fixtures.CreateEntityInMemory<FacilitySchedule>(
            InMemoryDataSource.FacilitySchedule1
        );

        FacilityScheduleHandler facilityScheduleHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityScheduleHandler>();

        // Act
        Result<bool> result = await facilityScheduleHandler.Delete(
            InMemoryDataSource.UserOwner.Id,
            InMemoryDataSource.Facility1.Id,
            InMemoryDataSource.FacilitySchedule1.Id
        );

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Should_Not_DeleteFacilitySchedule()
    {
        // Arrange
        FacilityScheduleHandler facilityScheduleHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityScheduleHandler>();

        // Act
        Result<bool> result = await facilityScheduleHandler.Delete(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        // Assert
        Assert.False(result.Value);
    }
}
