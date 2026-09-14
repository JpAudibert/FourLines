using FourLines.Application.DTOs.Matches;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;
using Org.BouncyCastle.Bcpg;

namespace FourLines.Tests.Matches;

[Collection(FourLinesCollection.Name)]
public class TestMatchesLeave(FourLinesFixture fixtures)
{
    [Fact]
    public async Task Should_LeaveTheMatch()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();
        await fixtures.EnsureGoalKeeperReservationCreatedAsync();

        IMatchHandler handler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        Guid matchId = fixtures.GoalKeeperReservationResult.Value.Match.Id;
        Guid userId = fixtures.GoalKeeperReservationResult.Value.Reservation.UserId;

        TestCreateIngressDTO ingressDTO = new()
        {
            MatchId = matchId,
            UserId = userId,
            Code = fixtures.GoalKeeperReservationResult.Value.Match.Code,
            IngressAsGoalKeeper = false,
        };

        await handler.Ingress(ingressDTO);

        // Act
        Result<bool> result = await handler.LeaveMatch(
            new LeaveMatchDTO { MatchId = matchId, UserId = userId }
        );

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value);
    }

    [Fact]
    public async Task ShouldNot_LeaveTheMatch_MatchNotFound()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();
        await fixtures.EnsureGoalKeeperReservationCreatedAsync();

        IMatchHandler handler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        Guid matchId = fixtures.GoalKeeperReservationResult.Value.Match.Id;
        Guid userId = fixtures.GoalKeeperReservationResult.Value.Reservation.UserId;

        // Act
        Result<bool> result = await handler.LeaveMatch(
            new LeaveMatchDTO { MatchId = matchId, UserId = userId }
        );

        // Assert
        Assert.False(result.IsSuccess);
        Assert.False(result.Value);
        Assert.Equal(MatchesErrorResults.LeaveMatchNotFound, result.Error);
    }
}
