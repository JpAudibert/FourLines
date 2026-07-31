using FourLines.Application.Handlers;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.FacilitySchedules;

public class TestFacilitySchedulesRead(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures = fixtures;

    [Fact]
    public async Task Should_GetFacilitiesSchedules()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.facility1);
        await _fixtures.CreateEntityInMemory<FacilitySchedule>(
            InMemoryDataSource.facilitySchedule1
        );
        await _fixtures.CreateEntityInMemory<FacilitySchedule>(
            InMemoryDataSource.facilitySchedule2
        );

        FacilityScheduleHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityScheduleHandler>();

        // Act
        Result<IEnumerable<FacilitySchedule>> result = await facilityHandler.GetSchedules(
            InMemoryDataSource.userOwner.Id,
            InMemoryDataSource.facility1.Id
        );

        // Assert
        Assert.NotEmpty(result.Value);
        Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public async Task Should_Not_GetFacilitiesSchedules()
    {
        // Arrange
        await _fixtures.RemoveAllDataFromMemory<Facility>();

        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        // Act
        Result<IEnumerable<Facility>> result = await facilityHandler.GetAllFacilities();

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.RetrieveNoFacilities, result.Error);
    }
}
