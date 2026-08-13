using FourLines.Application.DTOs.Courts;
using FourLines.Application.Handlers;
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
    public async Task Should_DeleteCourt()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.facility1);
        await _fixtures.CreateEntityInMemory<Sport>(InMemoryDataSource.sport);
        await _fixtures.CreateEntityInMemory<Court>(InMemoryDataSource.court1);

        CourtHandler courtHandler = _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        // Act
        Result<bool> result = await courtHandler.Delete(
            InMemoryDataSource.userOwner.Id,
            InMemoryDataSource.facility1.Id,
            InMemoryDataSource.court1.Id
        );

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Should_Not_DeleteCourt()
    {
        // Arrange
        CourtHandler courtHandler = _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        // Act
        Result<bool> result = await courtHandler.Delete(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        // Assert
        Assert.False(result.Value);
    }
}
