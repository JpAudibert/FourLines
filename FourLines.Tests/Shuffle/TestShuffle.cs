using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Infrastructure.Contexts;
using FourLines.Tests.Matches;
using FourLines.Tests.Shared;
using FourLines.Tests.Shared.Seed;

namespace FourLines.Tests.Shuffle;

[Collection(FourLinesCollection.Name)]
public class TestShuffle(FourLinesFixture fixtures)
{
    [Fact]
    public async Task Should_ShuffleTeams()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();
        await fixtures.EnsureShuffleReservationCreatedAsync();

        Match match = fixtures.ShuffleReservationResult.Value.Match;

        FourLinesContext context = scope.ServiceProvider.GetRequiredService<FourLinesContext>();

        await IngressPlayers();

        IEnumerable<MatchesUsers> unshuffledPlayers = context.MatchesUsers.Where(mu =>
            mu.MatchId == match.Id
        );

        IShuffleHandler handler = fixtures.ServiceProvider.GetRequiredService<IShuffleHandler>();

        // Act
        Result<IEnumerable<MatchesUsers>> result = await handler.ShufflePlayers(match.Id);

        // Assert
        Assert.NotNull(result.Value);
        Assert.True(result.IsSuccess);
        Assert.NotEqual(unshuffledPlayers, result.Value);
    }

    [Fact]
    public async Task ShouldNot_ShuffleTeams_MatchNotFound()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();
        await fixtures.EnsureShuffleReservationCreatedAsync();

        Match match = fixtures.ShuffleReservationResult.Value.Match;

        FourLinesContext context = scope.ServiceProvider.GetRequiredService<FourLinesContext>();

        IEnumerable<MatchesUsers> unshuffledPlayers = context.MatchesUsers.Where(mu =>
            mu.MatchId == match.Id
        );

        IShuffleHandler handler = fixtures.ServiceProvider.GetRequiredService<IShuffleHandler>();

        // Act
        Result<IEnumerable<MatchesUsers>> result = await handler.ShufflePlayers(Guid.NewGuid());

        // Assert
        Assert.NotNull(result.Error);
        Assert.True(result.IsFailure);
        Assert.Equal(ShufflingErrorResults.ShufflingUnknownMatch, result.Error);
    }

    private async Task IngressPlayers()
    {
        await fixtures.EnsureShuffleReservationCreatedAsync();
        IMatchHandler matchHandler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        TestCreateIngressDTO playerToShuffle1 = new()
        {
            MatchId = fixtures.ShuffleReservationResult.Value.Match.Id,
            UserId = UserSeed.PlayerToShuffle1.Id,
            Code = fixtures.ShuffleReservationResult.Value.Match.Code,
        };

        TestCreateIngressDTO playerToShuffle2 = playerToShuffle1 with
        {
            UserId = UserSeed.PlayerToShuffle2.Id,
        };

        TestCreateIngressDTO playerToShuffle3 = playerToShuffle1 with
        {
            UserId = UserSeed.PlayerToShuffle3.Id,
        };

        TestCreateIngressDTO playerToShuffle4 = playerToShuffle1 with
        {
            UserId = UserSeed.PlayerToShuffle4.Id,
        };

        TestCreateIngressDTO playerToShuffle5 = playerToShuffle1 with
        {
            UserId = UserSeed.PlayerToShuffle5.Id,
        };

        TestCreateIngressDTO playerToShuffle6 = playerToShuffle1 with
        {
            UserId = UserSeed.PlayerToShuffle6.Id,
        };

        TestCreateIngressDTO playerToShuffle7 = playerToShuffle1 with
        {
            UserId = UserSeed.PlayerToShuffle7.Id,
        };

        TestCreateIngressDTO playerToShuffle8 = playerToShuffle1 with
        {
            UserId = UserSeed.PlayerToShuffle8.Id,
        };

        await matchHandler.IngressAsFixedPosition(playerToShuffle1);
        await matchHandler.IngressAsFixedPosition(playerToShuffle2);
        await matchHandler.Ingress(playerToShuffle3);
        await matchHandler.Ingress(playerToShuffle4);
        await matchHandler.Ingress(playerToShuffle5);
        await matchHandler.Ingress(playerToShuffle6);
        await matchHandler.Ingress(playerToShuffle7);
        await matchHandler.Ingress(playerToShuffle8);
    }
}
