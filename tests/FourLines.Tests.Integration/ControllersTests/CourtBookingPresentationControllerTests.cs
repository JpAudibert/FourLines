using System.Net.Http.Json;
using Flurl.Http;
using FourLines.Api.Models.Presentation.Response;
using FourLines.Api.Models.RequestModels.Matches;
using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;
using FourLines.Tests.Integration.Shared;
using FourLines.Tests.Integration.Shared.Seed;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.Integration.ControllersTests;

[Collection(FourLinesCollection.Name)]
public class CourtBookingPresentationControllerTests(FourLinesFixture fixture)
{
    [Fact]
    public async Task GetHome_ReturnsFacilitySportsAndTodaySlotCount()
    {
        // Arrange
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: "/api/v1/presentation/home?cityName=Test%20City",
            method: HttpMethod.Get,
            token: token
        );

        // Assert
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);

        HomeResponse home = await response.GetJsonAsync<HomeResponse>();
        FacilityListItemResponse facility = Assert.Single(
            home.Facilities,
            item => item.FacilityId == FacilitySeed.Default.Id
        );

        Assert.Equal(FacilitySeed.Default.Name, facility.Name);
        Assert.Equal(FacilitySeed.Default.Address, facility.Address);
        Assert.Equal(2, facility.Sports.Count);
        Assert.Contains(
            facility.Sports,
            sport => sport.SportId == SportSeed.Default.Id && sport.SportName == SportSeed.Default.Name
        );
        Assert.Contains(
            facility.Sports,
            sport => sport.SportId == CourtSeed.CourtWithSportWithoutGoalkeeper.SportId
                && sport.SportName == SportSeed.WithoutGoalkeeper.Name
        );
        IFlurlResponse facilityCourtsResponse = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/presentation/facilities/{FacilitySeed.Default.Id}/courts",
            method: HttpMethod.Get,
            token: token
        );
        Assert.Equal(StatusCodes.Status200OK, facilityCourtsResponse.StatusCode);
        FacilityCourtsResponse facilityCourts =
            await facilityCourtsResponse.GetJsonAsync<FacilityCourtsResponse>();
        Assert.All(
            facilityCourts.Courts,
            court => Assert.True(court.RentingPeriodInMinutes > 0)
        );
        const int MinutesPerDay = 24 * 60;
        int maximumPossibleSlots = facilityCourts.Courts.Sum(court =>
            MinutesPerDay / court.RentingPeriodInMinutes
        );
        Assert.InRange(facility.AvailableTimeSlotsCount, 0, maximumPossibleSlots);
        Assert.DoesNotContain(
            home.Facilities,
            item => item.Name == FacilitySeed.Default2.Name
        );
    }

    [Fact]
    public async Task GetHome_WhenCityHasNoFacilities_ReturnsEmptyList()
    {
        // Arrange
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: "/api/v1/presentation/home?cityName=City%20With%20No%20Facilities",
            method: HttpMethod.Get,
            token: token
        );

        // Assert
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        HomeResponse home = await response.GetJsonAsync<HomeResponse>();
        Assert.Empty(home.Facilities);
    }

    [Fact]
    public async Task GetHome_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        const string url = "/api/v1/presentation/home";

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(url, HttpMethod.Get);

        // Assert
        Assert.Equal(StatusCodes.Status401Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetFacilityCourts_ReturnsFacilityAndCourtDetails()
    {
        // Arrange
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/presentation/facilities/{FacilitySeed.Default.Id}/courts",
            method: HttpMethod.Get,
            token: token
        );

        // Assert
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        FacilityCourtsResponse facility = await response.GetJsonAsync<FacilityCourtsResponse>();
        Assert.Equal(FacilitySeed.Default.Id, facility.FacilityId);
        Assert.Equal(FacilitySeed.Default.Name, facility.FacilityName);
        Assert.Equal(FacilitySeed.Default.Address, facility.FacilityAddress);

        CourtListItemResponse court = Assert.Single(
            facility.Courts,
            item => item.CourtId == CourtSeed.Default.Id
        );
        Assert.Equal(CourtSeed.Default.Name, court.Name);
        Assert.Equal(SportSeed.Default.Id, court.SportId);
        Assert.Equal(SportSeed.Default.Name, court.SportName);
        Assert.Equal(60, court.RentingPeriodInMinutes);
        Assert.Equal(50m, court.DefaultPrice.Value);
        Assert.Equal("BRL", court.DefaultPrice.Currency);
    }

    [Fact]
    public async Task GetFacilityCourts_WhenFacilityDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);
        Guid missingFacilityId = Guid.NewGuid();

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/presentation/facilities/{missingFacilityId}/courts",
            method: HttpMethod.Get,
            token: token
        );

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetCourtAvailability_ReturnsSelectedDateAndPricedSlots()
    {
        // Arrange
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);
        DateOnly selectedDate = DateOnly.FromDateTime(TestDates.Future.Date);

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/presentation/courts/{CourtSeed.Default.Id}/selected-date/{selectedDate:yyyy-MM-dd}",
            method: HttpMethod.Get,
            token: token
        );

        // Assert
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        CourtAvailabilityResponse availability =
            await response.GetJsonAsync<CourtAvailabilityResponse>();
        Assert.Equal(CourtSeed.Default.Id, availability.CourtId);
        Assert.Equal(selectedDate, availability.PlayingDate);
        Assert.NotEmpty(availability.TimeSlots);
        Assert.All(
            availability.TimeSlots,
            slot =>
            {
                Assert.True(slot.End > slot.Start);
                Assert.Equal(50m, slot.Price.Value);
                Assert.Equal("BRL", slot.Price.Currency);
            }
        );
    }

    [Fact]
    public async Task GetCourtAvailability_WhenCourtDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);
        Guid missingCourtId = Guid.NewGuid();

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/presentation/courts/{missingCourtId}/selected-date/{TestDates.Future:yyyy-MM-dd}",
            method: HttpMethod.Get,
            token: token
        );

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetCourtAvailability_WhenCourtHasNoSchedule_ReturnsNoSlots()
    {
        // Arrange
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);
        DateOnly selectedDate = new(2027, 2, 1);

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/presentation/courts/{CourtSeed.CourtWithNoSchedule2.Id}/selected-date/{selectedDate:yyyy-MM-dd}",
            method: HttpMethod.Get,
            token: token
        );

        // Assert
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        CourtAvailabilityResponse availability =
            await response.GetJsonAsync<CourtAvailabilityResponse>();
        Assert.Equal(CourtSeed.CourtWithNoSchedule2.Id, availability.CourtId);
        Assert.Equal(selectedDate, availability.PlayingDate);
        Assert.Empty(availability.TimeSlots);
    }

    [Fact]
    public async Task GetCourtAvailability_WhenDateIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/presentation/courts/{CourtSeed.Default.Id}/selected-date/not-a-date",
            method: HttpMethod.Get,
            token: token
        );

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetMyMatches_ReturnsParticipatingUsersMatchesWithDetails()
    {
        // Arrange
        (string token, Guid matchId) = await CreateMatchForPlayerAsync();

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: "/api/v1/presentation/matches/me",
            method: HttpMethod.Get,
            token: token
        );

        // Assert
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        List<MatchListItemResponse> matches = await response.GetJsonAsync<List<MatchListItemResponse>>();
        MatchListItemResponse match = Assert.Single(matches, item => item.MatchId == matchId);
        Assert.Equal("World Cup Match", match.Name);
        Assert.Equal(SportSeed.Default.Id, match.SportId);
        Assert.Equal(SportSeed.Default.Name, match.SportName);
        Assert.Equal(CourtSeed.Default.Name, match.CourtName);
        Assert.Equal(FacilitySeed.Default.Name, match.FacilityName);
        Assert.Contains(match.Players, player => player.UserId == UserSeed.Player.Id);
    }

    [Fact]
    public async Task GetMyMatches_WhenUserHasNoMatches_ReturnsEmptyList()
    {
        // Arrange
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.DummyAuth.Email);

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: "/api/v1/presentation/matches/me",
            method: HttpMethod.Get,
            token: token
        );

        // Assert
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Empty(await response.GetJsonAsync<List<MatchListItemResponse>>());
    }

    [Fact]
    public async Task GetMyMatches_WhenLimitIsNotPositive_ReturnsBadRequest()
    {
        // Arrange
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: "/api/v1/presentation/matches/me?limit=0",
            method: HttpMethod.Get,
            token: token
        );

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetMyMatches_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        const string url = "/api/v1/presentation/matches/me";

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(url, HttpMethod.Get);

        // Assert
        Assert.Equal(StatusCodes.Status401Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetMatch_ReturnsDetailedMatchAndPlayers()
    {
        // Arrange
        (string token, Guid matchId) = await CreateMatchForPlayerAsync();

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/presentation/matches/{matchId}",
            method: HttpMethod.Get,
            token: token
        );

        // Assert
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        MatchDetailResponse match = await response.GetJsonAsync<MatchDetailResponse>();
        Assert.Equal(matchId, match.MatchId);
        Assert.Equal(CourtSeed.Default.Id, match.CourtId);
        Assert.Equal("World Cup Match", match.Name);
        Assert.Equal(SportSeed.Default.Id, match.SportId);
        Assert.Equal(SportSeed.Default.Name, match.SportName);
        Assert.Equal(CourtSeed.Default.Name, match.CourtName);
        Assert.Equal(FacilitySeed.Default.Name, match.FacilityName);
        Assert.Contains(match.Players, player => player.UserId == UserSeed.Player.Id);
    }

    [Fact]
    public async Task GetMatch_WhenMatchDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);
        Guid missingMatchId = Guid.NewGuid();

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/presentation/matches/{missingMatchId}",
            method: HttpMethod.Get,
            token: token
        );

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetMatch_WhenNameIsNull_ReturnsDefaultName()
    {
        // Arrange
        (string token, Guid matchId) = await CreateMatchForPlayerAsync();
        await using (AsyncServiceScope scope = fixture.CreateAsyncServiceScope())
        {
            FourLinesContext context = scope.ServiceProvider.GetRequiredService<FourLinesContext>();
            Match match = await context.Matches.FirstAsync(m => m.Id == matchId);
            match.Name = null;
            await context.SaveChangesAsync();
        }

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/presentation/matches/{matchId}",
            method: HttpMethod.Get,
            token: token
        );

        // Assert
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        MatchDetailResponse matchDetails = await response.GetJsonAsync<MatchDetailResponse>();
        Assert.Equal("World Cup Match", matchDetails.Name);
    }

    private async Task<(string Token, Guid MatchId)> CreateMatchForPlayerAsync()
    {
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);
        var reservation = await ControllerTestHelpers.CreateReservationAsync(fixture, token);

        IFlurlResponse ingress = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/matches/{reservation.Match.Id}/ingress",
            method: HttpMethod.Post,
            content: JsonContent.Create(new CreateIngressRequest { Code = reservation.Match.Code }),
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, ingress.StatusCode);
        return (token, reservation.Match.Id);
    }
}
