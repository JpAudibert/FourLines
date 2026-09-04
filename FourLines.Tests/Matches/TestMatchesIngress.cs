using FourLines.Application.DTOs.Matches;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Matches;

public class TestMatchesIngress(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    [Fact]
    public async Task Should_Ingress_Match_Default()
    {
        // Arrange
        await fixtures.InitializeAsync();
        await fixtures.EnsureGoalKeeperReservationCreatedAsync();

        CreateIngressDTO ingress = new()
        {
            MatchId = fixtures.GoalKeeperReservationResult.Value.Match.Id,
            UserId = fixtures.GoalKeeperReservationResult.Value.Reservation.UserId,
            Code = fixtures.GoalKeeperReservationResult.Value.Match.Code,
            IngressAsGoalKeeper = false,
        };

        IMatchHandler matchHandler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        // Act
        Result<MatchesUsers> result = await matchHandler.Ingress(ingress);

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<MatchesUsers>(result.Value);

        Assert.Equal(ingress.MatchId, result.Value.MatchId);
        Assert.Equal(ingress.UserId, result.Value.UserId);
        Assert.Equal(ingress.IngressAsGoalKeeper, result.Value.IsGoalKeeper);
    }

    [Fact]
    public async Task ShouldNot_Ingress_Match_Default_MatchDoesNotExists()
    {
        // Arrange
        await fixtures.InitializeAsync();
        await fixtures.EnsureGoalKeeperReservationCreatedAsync();

        CreateIngressDTO ingress = new()
        {
            MatchId = Guid.NewGuid(),
            UserId = fixtures.GoalKeeperReservationResult.Value.Reservation.UserId,
            Code = fixtures.GoalKeeperReservationResult.Value.Match.Code,
            IngressAsGoalKeeper = false,
        };

        IMatchHandler matchHandler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        // Act
        Result<MatchesUsers> result = await matchHandler.Ingress(ingress);

        // Assert
        Assert.NotNull(result.Error);
        Assert.True(result.IsFailure);
        Assert.Equal(MatchesErrorResults.IngressMatchNotFound.Code, result.Error.Code);
    }

    [Fact]
    public async Task ShouldNot_Ingress_Match_Default_UserDoesNotExists()
    {
        // Arrange
        await fixtures.InitializeAsync();
        await fixtures.EnsureGoalKeeperReservationCreatedAsync();

        CreateIngressDTO ingress = new()
        {
            MatchId = fixtures.GoalKeeperReservationResult.Value.Match.Id,
            UserId = Guid.NewGuid(),
            Code = fixtures.GoalKeeperReservationResult.Value.Match.Code,
            IngressAsGoalKeeper = false,
        };

        IMatchHandler matchHandler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        // Act
        Result<MatchesUsers> result = await matchHandler.Ingress(ingress);

        // Assert
        Assert.NotNull(result.Error);
        Assert.True(result.IsFailure);
        Assert.Equal(MatchesErrorResults.IngressUserNotFound.Code, result.Error.Code);
    }

    [Fact]
    public async Task Should_Ingress_Match_As_GoalKeeper()
    {
        // Arrange
        await fixtures.InitializeAsync();
        await fixtures.EnsureGoalKeeperReservationCreatedAsync();

        CreateIngressDTO ingress = new()
        {
            MatchId = fixtures.GoalKeeperReservationResult.Value.Match.Id,
            UserId = InMemoryDataSource.UserPlayer2.Id,
            Code = fixtures.GoalKeeperReservationResult.Value.Match.Code,
            IngressAsGoalKeeper = true,
        };

        IMatchHandler matchHandler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        // Act
        Result<MatchesUsers> result = await matchHandler.Ingress(ingress);

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<MatchesUsers>(result.Value);

        Assert.Equal(ingress.MatchId, result.Value.MatchId);
        Assert.Equal(ingress.UserId, result.Value.UserId);
        Assert.Equal(ingress.IngressAsGoalKeeper, result.Value.IsGoalKeeper);
    }

    [Fact]
    public async Task ShouldNot_Ingress_Match_As_GoalKeeper_MatchDoesNotExists()
    {
        // Arrange
        await fixtures.InitializeAsync();
        await fixtures.EnsureGoalKeeperReservationCreatedAsync();

        CreateIngressDTO ingress = new()
        {
            MatchId = Guid.NewGuid(),
            UserId = fixtures.GoalKeeperReservationResult.Value.Reservation.UserId,
            Code = fixtures.GoalKeeperReservationResult.Value.Match.Code,
            IngressAsGoalKeeper = false,
        };

        IMatchHandler matchHandler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        // Act
        Result<MatchesUsers> result = await matchHandler.Ingress(ingress);

        // Assert
        Assert.NotNull(result.Error);
        Assert.True(result.IsFailure);
        Assert.Equal(MatchesErrorResults.IngressMatchNotFound.Code, result.Error.Code);
    }

    [Fact]
    public async Task ShouldNot_Ingress_Match_As_GoalKeeper_UserDoesNotExists()
    {
        // Arrange
        await fixtures.InitializeAsync();
        await fixtures.EnsureGoalKeeperReservationCreatedAsync();

        CreateIngressDTO ingress = new()
        {
            MatchId = fixtures.GoalKeeperReservationResult.Value.Match.Id,
            UserId = Guid.NewGuid(),
            Code = fixtures.GoalKeeperReservationResult.Value.Match.Code,
            IngressAsGoalKeeper = false,
        };

        IMatchHandler matchHandler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        // Act
        Result<MatchesUsers> result = await matchHandler.Ingress(ingress);

        // Assert
        Assert.NotNull(result.Error);
        Assert.True(result.IsFailure);
        Assert.Equal(MatchesErrorResults.IngressUserNotFound.Code, result.Error.Code);
    }

    [Fact]
    public async Task ShouldNot_Ingress_Match_As_GoalKeeper_Sport_DoesNot_Have_Fixed_Goal_Keeper()
    {
        // Arrange
        await fixtures.InitializeAsync();
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport2, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility3, context);
            await DbOperations.CreateEntityInMemory<FacilitySchedule>(InMemoryDataSource.FacilitySchedule4, context);
            await DbOperations.CreateEntityInMemory<Court>(InMemoryDataSource.Court3, context);
        }
        await fixtures.EnsureNoGoalKeeperReservationCreatedAsync();

        CreateIngressDTO ingress = new()
        {
            MatchId = fixtures.NoGoalKeeperReservationResult.Value.Match.Id,
            UserId = InMemoryDataSource.UserPlayer3.Id,
            Code = fixtures.NoGoalKeeperReservationResult.Value.Match.Code,
            IngressAsGoalKeeper = false,
        };

        IMatchHandler matchHandler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        // Act
        Result<MatchesUsers> result = await matchHandler.IngressAsGoalKeeper(ingress);

        // Assert
        Assert.NotNull(result.Error);
        Assert.True(result.IsFailure);
        Assert.Equal(MatchesErrorResults.IngressSportDoesNotHaveFixedGoalKeeper.Code, result.Error.Code);
    }
}
