using FourLines.Application.Interfaces;
using FourLines.Domain.Results;
using FourLines.Tests.Shared;
using FourLines.Domain.Models;

namespace FourLines.Tests.Matches;

[Collection(FourLinesCollection.Name)]
public class TestMatchesRead(FourLinesFixture fixtures)
{
    [Fact]
    public async Task Should_GetMatch()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();
        await fixtures.EnsureGoalKeeperReservationCreatedAsync();

        Guid matchId = fixtures.GoalKeeperReservationResult.Value.Match.Id;

        IMatchHandler handler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        // Act
        Result<Match?> result = await handler.GetMatch(matchId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(matchId, result.Value.Id);
    }

    [Fact]
    public async Task Should_GetMatch_Empty()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        Guid matchId = Guid.NewGuid();

        IMatchHandler handler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        // Act
        Result<Match?> result = await handler.GetMatch(matchId);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
