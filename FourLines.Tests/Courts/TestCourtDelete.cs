using FourLines.Application.DTOs.Courts;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Courts;

public class TestCourtDelete(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    [Fact]
    public async Task Should_DeleteCourt()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
            await DbOperations.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport, context);
            await DbOperations.CreateEntityInMemory<Court>(InMemoryDataSource.Court1, context);
        }

        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

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
        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

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
