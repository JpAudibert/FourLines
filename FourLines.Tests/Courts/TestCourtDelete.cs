using FourLines.Application.DTOs.Courts;
using FourLines.Application.Interfaces;
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
        OwnerId = InMemoryDataSource.UserOwner.Id,
        FacilityId = InMemoryDataSource.Facility1.Id,
        SportId = InMemoryDataSource.TestSport.Id,
        Name = "Test Court",
        IsActive = true,
    };

    [Fact]
    public async Task Should_DeleteCourt()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1);
        await _fixtures.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport);
        await _fixtures.CreateEntityInMemory<Court>(InMemoryDataSource.Court1);

        ICourtHandler courtHandler = _fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<bool> result = await courtHandler.Delete(new DeleteCourtDTO
        {
            OwnerId = InMemoryDataSource.UserOwner.Id,
            FacilityId = InMemoryDataSource.Facility1.Id,
            CourtId = InMemoryDataSource.Court1.Id
        });

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Should_Not_DeleteCourt()
    {
        // Arrange
        ICourtHandler courtHandler = _fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<bool> result = await courtHandler.Delete(new DeleteCourtDTO
        {
            OwnerId = Guid.NewGuid(),
            FacilityId = Guid.NewGuid(),
            CourtId = Guid.NewGuid()
        });

        // Assert
        Assert.False(result.Value);
    }
}
