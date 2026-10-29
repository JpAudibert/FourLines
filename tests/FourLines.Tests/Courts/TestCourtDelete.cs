using FourLines.Application.DTOs.Courts;
using FourLines.Application.Interfaces;
using FourLines.Domain.Results;
using FourLines.Tests.Shared;
using FourLines.Tests.Shared.Seed;

namespace FourLines.Tests.Courts;

[Collection(FourLinesCollection.Name)]
public class TestCourtDelete(FourLinesFixture fixtures)
{
    private static readonly DeleteCourtDTO _deleteCourt = new()
    {
        OwnerId = UserSeed.Owner.Id,
        CourtId = CourtSeed.ToBeDeleted.Id,
        FacilityId = CourtSeed.ToBeDeleted.FacilityId,
    };

    [Fact]
    public async Task Should_DeleteCourt()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();
        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<bool> result = await courtHandler.Delete(_deleteCourt);

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Should_Not_DeleteCourt()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();
        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        DeleteCourtDTO inexistentCourt = _deleteCourt with { CourtId = Guid.NewGuid() };

        // Act
        Result<bool> result = await courtHandler.Delete(inexistentCourt);

        // Assert
        Assert.False(result.Value);
    }

    [Fact]
    public async Task Should_Not_DeleteCourtForAnotherOwnersFacility()
    {
        await using var scope = fixtures.CreateAsyncServiceScope();
        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        Result<bool> result = await courtHandler.Delete(
            _deleteCourt with { OwnerId = UserSeed.Player.Id }
        );

        Assert.True(result.IsFailure);
    }
}
