using System.Net.Http.Json;
using Flurl.Http;
using FourLines.Api.Models.RequestModels.Reservations;
using FourLines.Domain.Models;
using FourLines.Tests.Integration.Shared;
using FourLines.Tests.Integration.Shared.Seed;
using Microsoft.AspNetCore.Http;

namespace FourLines.Tests.Integration.ControllersTests;

[Collection(FourLinesCollection.Name)]
public class ReservationsControllerTests(FourLinesFixture fixture)
{
    [Fact]
    public async Task GetAllFromUser_ReturnsOk()
    {
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: "/api/v1/reservations/users/me",
            method: HttpMethod.Get,
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAllFromCourt_ReturnsOk()
    {
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Owner.Email);

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/reservations/courts/{CourtSeed.Default.Id}",
            method: HttpMethod.Get,
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task GetOneFromUser_ReturnsOk()
    {
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/reservations/{ReservationSeed.Default.Id}/users/me",
            method: HttpMethod.Get,
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateForUser_ReturnsOk()
    {
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);

        await ControllerTestHelpers.CreateReservationAsync(fixture, token);
    }

    [Fact]
    public async Task CreateForUser_MissingPrice_ReturnsBadRequest()
    {
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);
        var request = new
        {
            CourtId = CourtSeed.Default.Id,
            Period = new TimeRange(
                TestDates.Future.AddDays(10),
                TestDates.Future.AddDays(10).AddHours(1)
            ),
            Status = ReservationStatus.Pending,
        };

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: "/api/v1/reservations/users/me",
            method: HttpMethod.Post,
            content: JsonContent.Create(request),
            token: token
        );

        Assert.Equal(StatusCodes.Status400BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateStatus_ReturnsOk()
    {
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);
        UpdateReservationStatusRequest request = new() { Status = ReservationStatus.Confirmed };

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/reservations/{ReservationSeed.ToBeUpdated.Id}/users/me",
            method: HttpMethod.Patch,
            content: JsonContent.Create(request),
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteForUser_ReturnsOk()
    {
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/reservations/{ReservationSeed.ToBeDeleted.Id}/users/me",
            method: HttpMethod.Delete,
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAvailableTimes_ReturnsOk()
    {
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/reservations/courts/{CourtSeed.Default.Id}/selected-date/{TestDates.Future:yyyy-MM-dd}",
            method: HttpMethod.Get,
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }
}
