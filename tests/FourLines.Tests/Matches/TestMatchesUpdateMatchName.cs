using FourLines.Application.DTOs.Matches;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Matches;

[Collection(FourLinesCollection.Name)]
public class TestMatchesUpdateMatchName(FourLinesFixture fixtures)
{
    [Fact]
    public async Task Should_UpdateMatchName()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();
        await fixtures.EnsureGoalKeeperReservationCreatedAsync();

        IMatchHandler handler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        Guid matchId = fixtures.GoalKeeperReservationResult.Value.Match.Id;
        string newMatchName = "New Match Name";

        // Act
        Result<Match> result = await handler.UpdateMatchName(
            new UpdateMatchNameDTO { MatchId = matchId, NewName = newMatchName }
        );

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(newMatchName, result.Value.Name);
    }

    [Fact]
    public async Task ShouldNot_UpdateMatchName_MatchNotFound()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IMatchHandler handler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        Guid matchId = Guid.NewGuid();
        string newMatchName = "New Match Name";

        // Act
        Result<Match> result = await handler.UpdateMatchName(
            new UpdateMatchNameDTO { MatchId = matchId, NewName = newMatchName }
        );

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(MatchesErrorResults.MatchNotFound, result.Error);
    }
}
