using FourLines.Application.DTOs.Courts;
using FourLines.Application.Handlers;
using FourLines.Domain.Constants;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.Courts;

public class TestCourtDelete(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures = fixtures;


    private static CreateCourtDTO _createCourtTest = new()
    {
        OwnerId = InMemoryDataSource.userOwner.Id,
        FacilityId = InMemoryDataSource.facility1.Id,
        SportId = InMemoryDataSource.sport.Id,
        Name = "Test Court",
        IsActive = true,
    };

    [Fact]
    public async Task Should_DeleteFacility()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.facility1);
        await _fixtures.CreateEntityInMemory<Sport>(InMemoryDataSource.sport);

        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        // Act
        Result<bool> result = await facilityHandler.Delete(InMemoryDataSource.userOwner.Id, InMemoryDataSource.facility1.Id);

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Should_Not_DeleteFacility()
    {
        // Arrange
        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        // Act
        Result<bool> result = await facilityHandler.Delete(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.False(result.Value);
    }
}
